using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace app.Foundation.Authentification
{
    public class AuthenticationMessageHandler : DelegatingHandler
    {
        public Authentication Authentication { get; private set; }
        public AuthenticationMessageHandler(Authentication authentication) : 
            base(new HttpClientHandler())
        {
            Authentication = authentication;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            // It is a best practice to refresh the access token before every message request is sent. Doing so
            // avoids having to check the expiration date/time of the token. This operation is quick.
            var token = Authentication.AcquireTokenAsync().Result;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
