using System;
using System.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration;
using PowerAppsWebApiUtils.Security;

namespace PowerAppsWebApiUtils.Configuration
{
    public static class PowerAppsConfigurationReader
    {
        public static CodeGenSettings GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                IConfigurationRoot configuration = builder.Build();

            return new CodeGenSettings
            {
                AuthenticationSettings = new PowerAppsAuthenticationSettings                     
                    {
                        ClientId = configuration["clientId"],
                        ClientSecret = configuration["secret"],
                        DirectoryId = configuration["directory"],
                        ApiUrl = configuration["powerappsApiUrl"],
                        ResourceUrl = configuration["powerappsUrl"],  

                    },
                Entities = configuration["entities"]?.Split(','),  
                Namespace = configuration["namespace"],  
                FileName = configuration["filePath"],              
            };
        }
    }
}