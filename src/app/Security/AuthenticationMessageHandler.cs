using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace app.Security
{
    public class AuthenticationMessageHandler : DelegatingHandler
    {


        private readonly AuthentificationConfiguration _configuration;

        public AuthenticationMessageHandler(AuthentificationConfiguration configuration) : 
            base(new HttpClientHandler())
        {
            _configuration = configuration;
        }

        private async Task<AuthenticationResult> AcquireTokenAsync()
        {
                       
            var clientCredential = new ClientCredential(_configuration.ClientId, _configuration.ClientSecret);
            var context = new AuthenticationContext($"https://login.microsoftonline.com/{_configuration.DirectoryId}", true);
            var authenticationResult = await context.AcquireTokenAsync(_configuration.ResourceUrl,  clientCredential);
            return authenticationResult;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var token = AcquireTokenAsync().Result;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
