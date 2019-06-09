using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace app.Foundation.Authentification
{
    public class Authentication
    {
        private readonly AuthentificationConfiguration _configuration;
        public Authentication(AuthentificationConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<AuthenticationResult> AcquireTokenAsync()
        {                            
            var clientCredential = new ClientCredential(_configuration.ClientId, _configuration.ClientSecret);
            var context = new AuthenticationContext($"https://login.microsoftonline.com/{_configuration.DirectoryId}", true);
            var authenticationResult = await context.AcquireTokenAsync(_configuration.ResourceUrl,  clientCredential);
            return authenticationResult;
        }
    }
}
