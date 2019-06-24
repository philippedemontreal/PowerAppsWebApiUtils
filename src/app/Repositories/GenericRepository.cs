using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml;
using app.Entities;
using app.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace  app.Repositories
{
    public class GenericRepository<T>: IDisposable  
    where T: class
    {
        private readonly PowerAppsConfiguration _powerAppsConfiguration;
        
        public const string ApplicationJson = "application/json";


        public string OdataEntityName { get; private set; }
        public GenericRepository(PowerAppsConfiguration powerAppsConfiguration, string odataEntityName)
        {
            _powerAppsConfiguration = powerAppsConfiguration;
            OdataEntityName = odataEntityName;
        }

        public async Task<List<T>> GetList()
        {
            List<T> result = null;

           using (var client = GetHttpClient())
            {
                    var getQuery = $"{OdataEntityName}";
                    var response = await client.GetAsync(getQuery, HttpCompletionOption.ResponseHeadersRead);
                    var rootValues = await DeserializeContent<RootObject<T>>(response);
                                        
                    if (rootValues != null)
                    {
                        if (result == null)
                            result = rootValues.Value;
                        else
                            result.AddRange(rootValues.Value);
                    }
                    else
                        throw new Exception("RootValues not set");
            }

            return result;            
        }

        public async Task<T> GetById(Guid entityId, Expression<Func<T, object>>[] exprs = null)
        {
            var fields = new StringBuilder();
            var typeT = typeof(T);

            foreach (var expr in exprs)
            {
                var binding = (UnaryExpression)expr.Body;
                var propName = ((MemberExpression)binding.Operand).Member.Name;

                var field = typeT.GetProperty(propName);
                if (field == null)
                    throw new InvalidOperationException();
                
                var dm = field.GetCustomAttributes(typeof(DataMemberAttribute), false).FirstOrDefault() as DataMemberAttribute;
                if (dm == null)
                    throw new InvalidOperationException();                
                 if (fields.Length > 0)
                     fields.Append(",");

                fields.Append(dm.Name);
            }

            // foreach (MemberBinding binding in ((MemberInitExpression)expr.Body).Bindings)
            // {
            //     if (fields.Length > 0)
            //         fields.Append(",");

            //     var field = typeT.GetProperty(binding.Member.Name);
            //     if (field == null)
            //         throw new InvalidOperationException();
                
            //     var dm = field.GetCustomAttributes(typeof(DataMemberAttribute), false).FirstOrDefault() as DataMemberAttribute;
            //     if (dm == null)
            //         throw new InvalidOperationException();                

            //     fields.Append(dm.Name);
            // }

            var getQuery = exprs == null ? $"{OdataEntityName}({entityId})" : $"{OdataEntityName}({entityId})?$select={fields}";
            using (var client = GetHttpClient())
            {
                var response = await client.GetAsync(getQuery, HttpCompletionOption.ResponseHeadersRead);
                return await DeserializeContent<T>(response);
            }
        }

        protected async Task<T> Retrieve(string selector)
        {                        
            using (var client = GetHttpClient())
            {
                var response = await client.GetAsync(selector, HttpCompletionOption.ResponseHeadersRead);
                return await DeserializeContent<T>(response);
            }
        }

                
        protected async Task<List<T>> RetrieveMultiple(string selector)
        {                        
            using (var client = GetHttpClient())
            {
                var response = await client.GetAsync(selector, HttpCompletionOption.ResponseHeadersRead);
                return await DeserializeContent<List<T>>(response);
            }
        }

        private async Task<P> DeserializeContent<P>(HttpResponseMessage response)
            where P: class
        {
            using (var reader = response.Content)
            {
                var content = await reader.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                        return null;

                    var exData = JObject.Parse(content);
                    throw new Exception(exData["error"]?["message"]?.ToString() ?? response.ReasonPhrase);
                }

                return JsonConvert.DeserializeObject<P>(content);
            }
        }


        private async Task<List<T>> GetListUsingFetchXml(string fetchXmlQuery)
        {
            List<T> result = null;
            using (var client = GetHttpClient())
            {
                var hasMore = false;
                string page = null;
                string cookie = null;
                do
                {
                    var query = fetchXmlQuery;
                    if (!string.IsNullOrEmpty(page) && !string.IsNullOrEmpty(cookie))
                    {
                        var xml = new XmlDocument();
                        xml.LoadXml(query);
                        var node = xml.FirstChild.NextSibling;
                        node.Attributes.Append(xml.CreateAttribute("page")).Value = page;
                        node.Attributes.Append(xml.CreateAttribute("paging-cookie")).Value = WebUtility.UrlDecode(WebUtility.UrlDecode(cookie));
                        query = xml.OuterXml;
                    }

                    var getQuery = $"{OdataEntityName}?fetchXml={WebUtility.UrlEncode(query)}";
                    var response = await client.GetAsync(getQuery, HttpCompletionOption.ResponseHeadersRead);
                    var rootValues = await DeserializeContent<RootObject<T>>(response);

                    if (rootValues != null)
                    {
                        if (result == null)
                            result = rootValues.Value;
                        else
                            result.AddRange(rootValues.Value);
                    }
                    else
                        throw new Exception("RootValues not set");

                    hasMore = !string.IsNullOrEmpty(rootValues.FetchXmlPagingCookie);

                    if (hasMore)
                        rootValues.GetPagingInfo(out page, out cookie);

                } while (hasMore);
            }
            return result;
        }

        private HttpClient GetHttpClient()
        {
            var handler = new AuthenticationMessageHandler(_powerAppsConfiguration);
  
            var httpClient = new HttpClient(handler, true);
            httpClient.BaseAddress = new Uri(_powerAppsConfiguration.ApiUrl);
            httpClient.Timeout = new TimeSpan(0, 2, 0);
            httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApplicationJson));
            httpClient.DefaultRequestHeaders.Add("Prefer", "odata.include-annotations=\"*\"");

            return httpClient;
        }
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~GenericRepository()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }

}