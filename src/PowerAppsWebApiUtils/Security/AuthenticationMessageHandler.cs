using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Collections.Concurrent;

namespace PowerAppsWebApiUtils.Security
{
    public class AuthenticationMessageHandler : DelegatingHandler
    {
        private readonly ConcurrentDictionary<string, AuthenticationResult> _tokenCache;
        public string ApiUrl 
            { 
                get { return _configuration.ApiUrl; }
            }

        private readonly PowerAppsAuthenticationSettings _configuration;

        public AuthenticationMessageHandler(PowerAppsAuthenticationSettings configuration) : 
            base(new HttpClientHandler())
        {
            _configuration = configuration;
            _tokenCache = new ConcurrentDictionary<string, AuthenticationResult>();
        }

        private async Task<AuthenticationResult> AcquireTokenAsync()
        {                    
            var clientCredential = new ClientCredential(_configuration.ClientId, _configuration.ClientSecret);
            var context = new AuthenticationContext($"https://login.microsoftonline.com/{_configuration.DirectoryId}", true);
            var authenticationResult = await context.AcquireTokenAsync(_configuration.ResourceUrl,  clientCredential);
            return authenticationResult;
        }

        protected override Task<HttpResponseMessage> SendAsync
        (
            HttpRequestMessage request, 
            CancellationToken cancellationToken
        )
        {
            var authenticationResult = 
                _tokenCache.GetOrAdd(_configuration.ClientId,
                (principalName) => 
                {
                    return AcquireTokenAsync().Result;
                });

            if (authenticationResult.ExpiresOn.UtcDateTime <= DateTime.Now)
            {
                authenticationResult = 
                    _tokenCache.AddOrUpdate(
                        _configuration.ClientId, 
                        authenticationResult,
                        (principalName, authResult) => 
                        {
                            return AcquireTokenAsync().Result;
                        });

            }

            if (authenticationResult == null || string.IsNullOrEmpty(authenticationResult.AccessToken))
                throw new AuthenticationException();

            request.Headers.Authorization = new AuthenticationHeaderValue(authenticationResult.AccessTokenType, authenticationResult.AccessToken);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
