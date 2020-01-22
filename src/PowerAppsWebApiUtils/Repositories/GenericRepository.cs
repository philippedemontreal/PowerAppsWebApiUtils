using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using Microsoft.Dynamics.CRM;
using PowerAppsWebApiUtils.Entities;
using PowerAppsWebApiUtils.Json;
using PowerAppsWebApiUtils.Linq;
using System.Reflection;
using System.Diagnostics;

namespace PowerAppsWebApiUtils.Repositories
{
    public class GenericRepository: IDisposable
    {
        protected readonly IHttpClientFactory _httpClientFactory;
        
        public const string ApplicationJson = "application/json";
        protected const string clientName = "webapi";

        public Guid CallerObjectId { get; set; }
        public Guid MSCRMCallerID { get; set; }


         public GenericRepository()   
         {}
        public GenericRepository(IHttpClientFactory httpClientFactory)
            => _httpClientFactory = httpClientFactory;              

        public async Task<Guid> Create(crmbaseentity entity)
        {
           var client = _httpClientFactory.CreateClient(clientName);

            {
                var json = JObject.FromObject(entity, new JsonSerializer { ContractResolver = new NavigationPropertyContractResolver() }).ToString(Newtonsoft.Json.Formatting.None);
                var request = 
                    new HttpRequestMessage(HttpMethod.Post, entity.EntityCollectionName)
                    {
                        Content = new StringContent(json, Encoding.Default, ApplicationJson)                       
                    };

                if (CallerObjectId != Guid.Empty)
                    request.Headers.Add("CallerObjectId", $"{CallerObjectId}");
                if (MSCRMCallerID != Guid.Empty)
                    request.Headers.Add("MSCRMCallerID", $"{MSCRMCallerID}");

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var entityId = response.Headers.GetValues("OData-EntityId").FirstOrDefault();
                return Guid.Parse(entityId.Split('(', ')')[1]);
            }
        }

        private object[] GetCustomAttributes<T>(Expression<Func<T, NavigationProperty>> expression)
        {

            var body = expression.Body as MemberExpression ?? (expression.Body as UnaryExpression)?.Operand as MemberExpression;
            return body?.Member.GetCustomAttributes(false);

            // var navigationPropertyAttribute = body.Member.GetCustomAttributes(false);
            //     .Where(x => x is NavigationPropertyAttribute)
            //     .FirstOrDefault() as NavigationPropertyAttribute;

            // return navigationPropertyAttribute?.RelationSchemaName;
        }

        public async Task Diassociate<T>(T entity, Expression<Func<T, NavigationProperty>> expression)
        where T:crmbaseentity
        {           
            var navigationProperty = expression.Compile().Invoke(entity) as NavigationProperty;
            if (navigationProperty == null)
                return;

            var attributes = GetCustomAttributes<T>(expression);
            var navigationPropertyAttribute = attributes.FirstOrDefault(p => p is NavigationPropertyAttribute) as NavigationPropertyAttribute;
            if (navigationPropertyAttribute == null)
                throw new InvalidOperationException("Unable to find an attribute of type NavigationPropertyAttribute on this NavigationProperty.");

            var navigationPropertyTargetAttribute = 
                attributes
                .FirstOrDefault(p => (p is NavigationPropertyTargetAttribute) && ((NavigationPropertyTargetAttribute)p).Target == navigationProperty.EntityLogicalName) as NavigationPropertyTargetAttribute;
            
            if (navigationPropertyTargetAttribute == null)
                throw new InvalidOperationException("Unable to find an attribute of type NavigationPropertyTargetAttribute on this NavigationProperty that matches the logicalname of the target entity.");

            var client = _httpClientFactory.CreateClient(clientName);
            {
                var requestUri = $"{navigationPropertyTargetAttribute.CollectionName}({navigationProperty.Id})/{navigationPropertyAttribute.RelationSchemaName}({entity.Id})/$ref";
                var request = new HttpRequestMessage(HttpMethod.Delete, requestUri);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task Update(crmbaseentity entity)
        {
           var client = _httpClientFactory.CreateClient(clientName);
           {
                var json = JObject.FromObject(entity, new JsonSerializer{ ContractResolver = new NavigationPropertyContractResolver() }).ToString(Newtonsoft.Json.Formatting.None);
                var request = 
                    new HttpRequestMessage(new HttpMethod("PATCH"), $"{entity.EntityCollectionName}({entity.Id})")
                    {
                        Content = new StringContent(json, Encoding.Default, ApplicationJson)
                    };

                if (CallerObjectId != Guid.Empty)
                    request.Headers.Add("CallerObjectId", $"{CallerObjectId}");
                if (MSCRMCallerID != Guid.Empty)
                    request.Headers.Add("MSCRMCallerID", $"{MSCRMCallerID}");

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task Delete(crmbaseentity entity)
        {
           var client = _httpClientFactory.CreateClient(clientName);
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, $"{entity.EntityCollectionName}({entity.Id})");
                if (CallerObjectId != Guid.Empty)
                    request.Headers.Add("CallerObjectId", $"{CallerObjectId}");
                if (MSCRMCallerID != Guid.Empty)
                    request.Headers.Add("MSCRMCallerID", $"{MSCRMCallerID}");

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
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
        where T: crmbaseentity
    {

        ///Used with NSubstitute mocks 
        public GenericRepository()
        :base(null)
        {}

        public readonly string OdataEntityName;
        public GenericRepository(IHttpClientFactory httpClientFactory)
        : base(httpClientFactory)
            =>  OdataEntityName = (Activator.CreateInstance<T>() as crmbaseentity).EntityCollectionName;

        public async Task<List<T>> GetList()
            => await RetrieveMultiple(OdataEntityName);

        public async Task<T> GetById<TResult>(Guid entityId, Expression<Func<T, TResult>> expr)
        {
            var projection = new ColumnProjector().ProjectColumns(expr, Expression.Parameter(typeof(ProjectionRow), "row"));                
            var query = $"{OdataEntityName}({entityId})?$select={projection.Columns}";
            return await Retrieve(query);          
        }

        public virtual async Task<T> Retrieve(string query)
        {                        
            var client = _httpClientFactory.CreateClient(clientName);
            return await SendGetRequest<T>(client, query, new []{ new KeyValuePair<string, string>("Prefer", "odata.include-annotations=\"*\"") }.ToDictionary(p => p.Key, p => p.Value));
        }
                
        public virtual async Task<List<T>> RetrieveMultiple(string query)
        {                        
            var client = _httpClientFactory.CreateClient(clientName);
            
            var result = new List<T>();
            do
            {
                var rootObject = await SendGetRequest<RootObject<T>>(client, query, new []{ new KeyValuePair<string, string>("Prefer", "odata.include-annotations=\"*\"") }.ToDictionary(p => p.Key, p => p.Value));
                result.AddRange(rootObject.Value);
                query = rootObject.NextLink;
            }  while (!string.IsNullOrEmpty(query));

            return result;            
        }

        private async Task<P> SendGetRequest<P>(HttpClient client, string uri, Dictionary<string, string> additionalHeaders = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            if (CallerObjectId != Guid.Empty)
                request.Headers.Add("CallerObjectId", $"{CallerObjectId}");
                if (MSCRMCallerID != Guid.Empty)
                    request.Headers.Add("MSCRMCallerID", $"{MSCRMCallerID}");

            if (additionalHeaders != null)
            {
                foreach (var item in additionalHeaders)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            return await DeserializeContent<P>(response);
        }

        private async Task<P> DeserializeContent<P>(HttpResponseMessage response)
        {
            using (var reader = response.Content)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return default(P);

                response.EnsureSuccessStatusCode();

                var content = await reader.ReadAsStringAsync();
                //Trace.TraceInformation($"Content: {content}");
                return JsonConvert.DeserializeObject<P>(content, new JsonSerializerSettings { ContractResolver = ExtendedEntityContractResolver.Instance });
            }
        }

        // private async Task<List<T>> GetListUsingFetchXml(string fetchXmlQuery)
        // {
        //     List<T> result = null;
        //    var client = _httpClientFactory.CreateClient(clientName);
        //     {
        //         var hasMore = false;
        //         string page = null;
        //         string cookie = null;
        //         do
        //         {
        //             var query = fetchXmlQuery;
        //             if (!string.IsNullOrEmpty(page) && !string.IsNullOrEmpty(cookie))
        //             {
        //                 var xml = new XmlDocument();
        //                 xml.LoadXml(query);
        //                 var node = xml.FirstChild.NextSibling;
        //                 node.Attributes.Append(xml.CreateAttribute("page")).Value = page;
        //                 node.Attributes.Append(xml.CreateAttribute("paging-cookie")).Value = WebUtility.UrlDecode(WebUtility.UrlDecode(cookie));
        //                 query = xml.OuterXml;
        //             }

        //             var getQuery = $"{OdataEntityName}?fetchXml={WebUtility.UrlEncode(query)}";
        //             var rootValues = await SendGetRequest<FetchXmlRootObject<T>>(client, query, new []{ new KeyValuePair<string, string>("Prefer", "odata.include-annotations=\"*\"") }.ToDictionary(p => p.Key, p => p.Value));

        //             if (rootValues != null)
        //             {
        //                 if (result == null)
        //                     result = rootValues.Value;
        //                 else
        //                     result.AddRange(rootValues.Value);
        //             }
        //             else
        //                 throw new Exception("RootValues not set");

        //             hasMore = !string.IsNullOrEmpty(rootValues.FetchXmlPagingCookie);

        //             if (hasMore)
        //                 rootValues.GetPagingInfo(out page, out cookie);

        //         } while (hasMore);
        //     }
        //     return result;
        // }
    }
}