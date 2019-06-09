using System;
using System.Net.Http;
using System.Net.Http.Headers;
using app.Configuration;
using app.Repositories;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace app
{
    class Program
    
    {

     static void Main(string[] args)
        {
            var configuration =  ConfigurationReader.GetConfiguration();

            using (var entityDefinitionRepository = new EntityMetadataRepository(configuration))
            {
          
                var definition = entityDefinitionRepository.GetByLogicalName("account").Result;
 
            }
            
         }  
    }
}
