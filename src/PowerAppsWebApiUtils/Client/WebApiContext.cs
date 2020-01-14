using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Dynamics.CRM;
using PowerAppsWebApiUtils.Linq;
using PowerAppsWebApiUtils.Repositories;
using PowerAppsWebApiUtils.Security;

namespace PowerAppsWebApiUtils.Client
{
    public class WebApiContext : IDisposable
    {
        private readonly WebApiQueryProvider _queryProvider;
        private readonly GenericRepository _genericRepository;
        
        public WebApiContext(IHttpClientFactory httpClientFactory, IServiceProvider serviceProvider)
        {
            _queryProvider = new WebApiQueryProvider(serviceProvider);  
            _genericRepository = new GenericRepository(httpClientFactory);          
        }

        public Guid MSCRMCallerID { get => _genericRepository.MSCRMCallerID; set => _genericRepository.MSCRMCallerID = value; }
        public Guid CallerObjectId { get => _genericRepository.CallerObjectId; set => _genericRepository.CallerObjectId = value; }

        public IQueryable<T> CreateQuery<T>()
            => new Query<T>(_queryProvider);

        public Task<Guid> Create(crmbaseentity entity)
            => _genericRepository.Create(entity);

        public Task Update(crmbaseentity entity)
            => _genericRepository.Update(entity);
        
        public Task Delete(crmbaseentity entity)
            => _genericRepository.Delete(entity);        
        public string GetQueryText(Expression expression)
            => _queryProvider.GetQueryText(expression);
        
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _genericRepository.Dispose();
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~WebApiClient()
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