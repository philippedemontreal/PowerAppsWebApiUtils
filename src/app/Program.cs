using System;
using System.Linq;
using System.Collections.Generic;
using app.codegen;
using app.Configuration;
using app.Repositories;
using Microsoft.Dynamics.CRM;
using app.Security;

namespace app
{
    class Program
    
    {

     static void Main(string[] args)
        {
            var config =  PowerAppsConfigurationReader.GetConfiguration();
            
            using (var tokenProvider = new AuthenticationMessageHandler(config))
            using (var entityDefinitionRepository = new EntityMetadataRepository(tokenProvider))
            using (var picklistRepository = new OptionSetMetadataRepository(tokenProvider))
            {
          
                var entities = new List<EntityMetadata>();
                var pickLists = new Dictionary<string, PicklistAttributeMetadata>();

                entities.Add(entityDefinitionRepository.GetByLogicalName("customeraddress").Result);
                entities.Add(entityDefinitionRepository.GetByLogicalName("account").Result);
                entities.Add(entityDefinitionRepository.GetByLogicalName("contact").Result);
                
                entities
                .ForEach(p => 
                    p.Attributes
                        .Where(q => q.AttributeType == AttributeTypeCode.Picklist || q.AttributeType == AttributeTypeCode.State || q.AttributeType == AttributeTypeCode.Status)
                        .GroupBy(q => q.LogicalName)
                        .Select(q => q.First()).ToList()
                        .ForEach(
                            q => 
                            {
                                var key = $"{p.LogicalName}-{q.LogicalName}";
                                if (pickLists.ContainsKey(key))
                                    return;

                                pickLists.Add(key, picklistRepository.GetOptionsetMetadata(q).Result);
                            }));

                var codeGen = new CodeGen();
                codeGen.Execute("webapi.entities", entities, pickLists);
            }            
         }  
    }
}
