using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using PowerAppsWebApiUtils.Extensions;
using PowerAppsWebApiUtils.Security;
using System;

[assembly: WebJobsStartup(typeof(FunctionApp1.Startup))]
namespace FunctionApp1
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var config =
                new PowerAppsAuthenticationSettings
                {
                    ClientId = configuration["clientId"],
                    ClientSecret = configuration["secret"],
                    DirectoryId = configuration["directory"],
                    ApiUrl = configuration["powerappsApiUrl"],
                    ResourceUrl = configuration["powerappsUrl"],
                };

            builder.Services.AddPowerAppsWebApiConfiguration(config);
        }
    }
}