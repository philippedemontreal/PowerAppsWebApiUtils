using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Dynamics.CRM;
using PowerAppsWebApiUtils.Entities;
using PowerAppsWebApiUtils.Security;
using PowerAppsWebApiUtils.Json;
using PowerAppsWebApiUtils.Linq;

namespace PowerAppsWebApiUtils.Repositories
{
    public class GenericRepository: IDisposable
    {
        private readonly AuthenticationMessageHandler _tokenProvider;
        
        public const string ApplicationJson = "application/json";

        public GenericRepository(AuthenticationMessageHandler tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }        

        public async Task<Guid> Create(crmbaseentity entity)
        {
            using (var client = GetHttpClient())
            {
                var json = JObject.FromObject(entity, new JsonSerializer { ContractResolver = NavigationPropertyContractResolver.Instance }).ToString(Newtonsoft.Json.Formatting.None);
                var request = 
                    new HttpRequestMessage(HttpMethod.Post, entity.EntityCollectionName)
                    {
                        Content = new StringContent(json, Encoding.Default, ApplicationJson)
                    };

                var response = await client.SendAsync(request);
                await EnsureSuccessStatusCode(response);

                var entityId = response.Headers.GetValues("OData-EntityId").FirstOrDefault();
                return Guid.Parse(entityId.Split('(', ')')[1]);
            }
        }

        public async Task Update(crmbaseentity entity)
        {
            using (var client = GetHttpClient())
            {
                var json = JObject.FromObject(entity, new JsonSerializer{ ContractResolver = NavigationPropertyContractResolver.Instance }).ToString(Newtonsoft.Json.Formatting.None);
                var request = 
                    new HttpRequestMessage(new HttpMethod("PATCH"), string.Format("{0}({1})", entity.EntityCollectionName, entity.Id))
                    {
                        Content = new StringContent(json, Encoding.Default, ApplicationJson)
                    };

                var response = await client.SendAsync(request);
                await EnsureSuccessStatusCode(response);
            }
        }

        public async Task Delete(crmbaseentity entity)
        {
            using (var client = GetHttpClient())
            {
                var request = new HttpRequestMessage(new HttpMethod("DELETE"), string.Format("{0}({1})", entity.EntityCollectionName, entity.Id));
                var response = await client.SendAsync(request);
                await EnsureSuccessStatusCode(response);
            }
        }

        protected HttpClient GetHttpClient()
        {  
            var httpClient = new HttpClient(_tokenProvider, false)
            {
                BaseAddress = new Uri(_tokenProvider.ApiUrl),
                Timeout = new TimeSpan(0, 2, 0),
                DefaultRequestHeaders = 
                {
                    {"OData-MaxVersion", "4.0"},
                    {"OData-Version", "4.0"},
                    {"Prefer", "odata.include-annotations=\"*\"" }
                }
            };

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApplicationJson));

            return httpClient;
        }       

        protected static async Task EnsureSuccessStatusCode(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                using (var reader = response.Content)
                {
                    var content = await reader.ReadAsStringAsync();
                    var exData = JObject.Parse(content);
                    throw new PAWAUException(response.StatusCode, exData["error"]?["message"]?.ToString() ?? response.ReasonPhrase);
                }
            }
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
    public class GenericRepository<T>: GenericRepository  
    {


        public readonly string OdataEntityName;
        public GenericRepository(AuthenticationMessageHandler tokenProvider)
        : base(tokenProvider)
        {
            OdataEntityName = (Activator.CreateInstance<T>() as crmbaseentity).EntityCollectionName;
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
                        throw new PAWAUException("RootValues not set");
            }

            return result;            
        }

        public async Task<T> GetById<TResult>(Guid entityId, Expression<Func<T, TResult>> expr)
        {
            var fields = new StringBuilder();
            var typeT = typeof(T);

            var projection = new ColumnProjector().ProjectColumns(expr, Expression.Parameter(typeof(ProjectionRow), "row"));                
            var getQuery = $"{OdataEntityName}({entityId})?$select={projection.Columns}";
            using (var client = GetHttpClient())
            {
                var response = await client.GetAsync(getQuery, HttpCompletionOption.ResponseHeadersRead);
                var result = await DeserializeContent<T>(response);
                return result;
            }
            
        }

        // public async Task<T> GetById(Guid entityId, Expression<Func<T, object>>[] exprs = null)
        // {
        //     var fields = new StringBuilder();
        //     var typeT = typeof(T);

        //     if (exprs != null)
        //     {
        //         foreach (var expr in exprs)
        //         {
        //             var propName = "";

        //             if (expr.Body is UnaryExpression)
        //             {
        //                 var binding = (UnaryExpression)expr.Body;
        //                 propName = ((MemberExpression)binding.Operand).Member.Name;
        //             }
        //             else if (expr.Body is MemberExpression)
        //             {
        //                 propName = ((MemberExpression)expr.Body).Member.Name;
        //             }
        //             else 
        //             {
        //                 throw new NotImplementedException();
        //             }


        //             var field = typeT.GetProperty(propName);
        //             if (field == null)
        //                 throw new InvalidOperationException();
                    
        //             var dm = field.GetCustomAttributes(typeof(DataMemberAttribute), false).FirstOrDefault() as DataMemberAttribute;
        //             if (dm == null)
        //                 throw new InvalidOperationException();

        //             if (fields.Length > 0)
        //                 fields.Append(",");

        //             if (field.PropertyType == typeof(NavigationProperty))
        //                 fields.Append($"_{dm.Name}_value");
        //             else                    
        //                 fields.Append(dm.Name);
        //         }
        //     }


        //     var getQuery = exprs == null ? $"{OdataEntityName}({entityId})" : $"{OdataEntityName}({entityId})?$select={fields}";
        //     using (var client = GetHttpClient())
        //     {
        //         var response = await client.GetAsync(getQuery, HttpCompletionOption.ResponseHeadersRead);
        //         return await DeserializeContent<T>(response);
        //     }
            
        // }

        public async Task<T> Retrieve(string selector)
        {                        
            using (var client = GetHttpClient())
            {
                
                var response = await client.GetAsync(selector, HttpCompletionOption.ResponseHeadersRead);
                return await DeserializeContent<T>(response);
            }
        }
                
        public async Task<List<T>> RetrieveMultiple(string selector)
        {                        
            using (var client = GetHttpClient())
            {
                var result = new List<T>();
                do
                {
                    var response = await client.GetAsync(selector, HttpCompletionOption.ResponseHeadersRead);
                    var robject = await DeserializeContent<RootObject<T>>(response);
                    result.AddRange(robject.Value);
                    selector = robject.NextLink;
                }  while (!string.IsNullOrEmpty(selector));

                return result;
            }
        }
        private async Task<P> DeserializeContent<P>(HttpResponseMessage response)
        {
            using (var reader = response.Content)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return default(P);

                await EnsureSuccessStatusCode(response);    

                var content = await reader.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<P>(content, new JsonSerializerSettings { ContractResolver = ExtendedEntityContractResolver.Instance });
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
                    var rootValues = await DeserializeContent<FetchXmlRootObject<T>>(response);

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
    }
}