using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using app.codegen;
using app.Configuration;
using app.Metadata;
using app.Repositories;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using webapi.entities;

namespace app
{
    class Program
    
    {

     static void Main(string[] args)
        {
            var configuration =  ConfigurationReader.GetConfiguration();

            using (var rep = new GenericRepository<Contact>(configuration, "contacts"))
            {
                var contact = rep.GetById(Guid.Parse("5e5ed248-1339-e911-a97c-000d3af490cc")).Result;
            }
                        using (var rep = new GenericRepository<ic_deal>(configuration, "ic_deals"))
            {
                var contact = rep.GetById(Guid.Parse("6a6bb035-0bf4-e811-a977-000d3af490cc")).Result;
            }



            using (var entityDefinitionRepository = new EntityMetadataRepository(configuration))
            {
          
                var list = new List<EntityMetadata>();
                list.Add(entityDefinitionRepository.GetByLogicalName("ic_deal").Result);

                var codeGen = new CodeGen();
                codeGen.Execute(list);

 
            }
            
         }  
    }
}
