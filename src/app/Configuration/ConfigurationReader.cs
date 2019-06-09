using System;
using app.Security;

namespace app.Configuration
{
    public class ConfigurationReader : IConfigurationReader
    {
        public AuthentificationConfiguration GetConfiguration()
        {

            return new AuthentificationConfiguration
            {
                ClientId = "552770b2-b6fa-4c46-a61d-c38769cf8776",
                ClientSecret = "Cpim?_WKDwrju@T[WTpPZ54pOmF6nIX1",
                DirectoryId = "0be6964e-8304-424c-b918-47662f235d24",
                ApiUrl = "https://dev4cactus.crm3.dynamics.com/api/data/v9.0/",
                ResourceUrl = "https://dev4cactus.crm3.dynamics.com",
            };
        }
    }
}