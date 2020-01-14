using System;
using Microsoft.Extensions.DependencyInjection;
using PowerAppsWebApiUtils.Client;
using PowerAppsWebApiUtils.Repositories;
using PowerAppsWebApiUtils.Security;

namespace PowerAppsWebApiUtils.Extensions
{

    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddPowerAppsWebApiConfiguration(this IServiceCollection services, PowerAppsAuthenticationSettings authenticationSettings)
        {           
            services
                .AddSingleton<PowerAppsAuthenticationSettings>(p => authenticationSettings)
                .AddTransient<AuthenticationMessageHandler>()
                .AddTransient<WebApiContext>()
                .AddTransient(typeof(GenericRepository<>))
                .AddHttpClient(
                    "webapi",
                    p => 
                    {
                            p.BaseAddress = new Uri(authenticationSettings.ApiUrl);
                            p.Timeout = new TimeSpan(0, 2, 0);
                            p.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                            p.DefaultRequestHeaders.Add("OData-Version", "4.0");

                    })           
                .AddHttpMessageHandler<AuthenticationMessageHandler>(); 

            return services;
        }
        
    }

}