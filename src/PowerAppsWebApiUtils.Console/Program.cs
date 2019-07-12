using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Dynamics.CRM;
using PowerAppsWebApiUtils.Codegen;
using PowerAppsWebApiUtils.Configuration;
using PowerAppsWebApiUtils.Repositories;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PowerAppsWebApiUtils.Extensions;

namespace PowerappsWebApiUtils
{
    class Program   
    {

     static void Main(string[] args)
        {
            var config =  PowerAppsConfigurationReader.GetConfiguration();

            var serviceProvider = 
                new ServiceCollection()            
                .AddWebApiContext(config.AuthenticationSettings)
                .AddTransient<EntityMetadataRepository>()
                .AddTransient<OptionSetMetadataRepository>()
                .BuildServiceProvider();
            
            using (var entityDefinitionRepository = serviceProvider.GetRequiredService<EntityMetadataRepository>())
            using (var picklistRepository = serviceProvider.GetRequiredService<OptionSetMetadataRepository>())
            {
                var tasks = config.Entities.GroupBy(p => p).Select(p => p.First()).Select(p => entityDefinitionRepository.GetByLogicalName(p)).ToArray();
                Task.WaitAll(tasks);
                var entities = tasks.Select(p => p.Result).ToList();
                
                var pickLists = new Dictionary<string, Task<PicklistAttributeMetadata>>();
                entities.ForEach(
                    p => 
                    {
                        p.Attributes
                        .Where(q => q.AttributeType == AttributeTypeCode.Picklist || q.AttributeType == AttributeTypeCode.State || q.AttributeType == AttributeTypeCode.Status)
                        .GroupBy(q => q.LogicalName)
                        .Select(q => q.First())
                        .ToList()
                        .ForEach(
                            q => 
                            {
                                var key = $"{p.LogicalName}-{q.LogicalName}";
                                if (pickLists.ContainsKey(key))
                                    return;

                                pickLists.Add(key, picklistRepository.GetOptionsetMetadata(q));
                            });
                    });

                Task.WaitAll(pickLists.Select(p => p.Value).ToArray());

                new CodeGen().Execute(config, entities, pickLists.ToDictionary(p => p.Key, p => p.Value.Result));
            }            
         }  
    }
}
