using System;
using System.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration;
using PowerAppsWebApiUtils.Security;

namespace PowerAppsWebApiUtils.Configuration
{
    public static class PowerAppsConfigurationReader
    {
        public static PowerAppsAuthenticationSettings GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                IConfigurationRoot configuration = builder.Build();

            return 
                new PowerAppsAuthenticationSettings                     
                {
                    ClientId = configuration["clientId"],
                    ClientSecret = configuration["secret"],
                    DirectoryId = configuration["directory"],
                    ApiUrl = configuration["powerappsApiUrl"],
                    ResourceUrl = configuration["powerappsUrl"],  
                };
        }
    }
}