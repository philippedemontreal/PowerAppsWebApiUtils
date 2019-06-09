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
            using (var entityDefinitionRepository = new EntityDefinitionRepository(new ConfigurationReader()))
            {
          
                var contactdefinition = entityDefinitionRepository.GetByLogicalName("ic_deal").Result;
 
            }
            
         }  
    }
}
