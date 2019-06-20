using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using app.codegen;
using app.Configuration;
using app.Metadata;
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
          
                var list = new List<EntityMetadata>();
                list.Add(entityDefinitionRepository.GetByLogicalName("account").Result);

                var codeGen = new CodeGen();
                codeGen.Execute(list);

 
            }
            
         }  
    }
}
