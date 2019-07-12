using System;
using Microsoft.Extensions.DependencyInjection;
using PowerAppsWebApiUtils.Client;
using PowerAppsWebApiUtils.Repositories;
using PowerAppsWebApiUtils.Security;

namespace PowerAppsWebApiUtils.Extensions
{

    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddWebApiContext(this IServiceCollection services, PowerAppsAuthenticationSettings authenticationSettings)
        {           
            services
                .AddSingleton<PowerAppsAuthenticationSettings>(p => authenticationSettings)
                .AddSingleton<AuthenticationMessageHandler>()
                .AddScoped<WebApiContext>()
                .AddScoped(typeof(GenericRepository<>))
                .AddHttpClient(
                    "webapi",
                    p => 
                    {
                            p.BaseAddress = new Uri(authenticationSettings.ApiUrl);
                            p.Timeout = new TimeSpan(0, 2, 0);
                            p.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                            p.DefaultRequestHeaders.Add("OData-Version", "4.0");
                            //p.DefaultRequestHeaders.Add("Prefer", "odata.include-annotations=\"*\"");

                    })           
                .AddHttpMessageHandler<AuthenticationMessageHandler>(); 

            return services;
        }
        
    }

}