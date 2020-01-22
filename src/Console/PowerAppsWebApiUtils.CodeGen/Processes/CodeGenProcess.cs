using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Dynamics.CRM;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PowerAppsWebApiUtils.Codegen;
using PowerAppsWebApiUtils.Configuration;
using PowerAppsWebApiUtils.Extensions;
using PowerAppsWebApiUtils.Repositories;

namespace PowerAppsWebApiUtils.Processes
{


    public sealed class CodeGenProcess
    {
        public void Execute(CodeGenSettings settings)
        {
            var serviceProvider = 
                new ServiceCollection()
                .AddLogging(p => p.AddConsole()) 
                .Configure<LoggerFilterOptions>(p => p.AddFilter((category, logLevel) => category.Contains(nameof(CodeGenProcess))))
                .AddPowerAppsWebApiConfiguration(settings.AuthenticationSettings)
                .AddTransient<EntityMetadataRepository>()
                .AddTransient<OptionSetMetadataRepository>()
                .BuildServiceProvider();

            var logger = serviceProvider.GetRequiredService<ILogger<CodeGenProcess>>();
            logger.LogInformation($"{nameof(CodeGenProcess)}. Starting process.");
            logger.LogInformation($"PowerAspps Url: {settings.AuthenticationSettings.ResourceUrl}");
            logger.LogInformation($"Entities: {string.Join(", ", settings.Entities)}");
            var now = DateTime.Now;
            
            using (var entityDefinitionRepository = serviceProvider.GetRequiredService<EntityMetadataRepository>())
            using (var picklistRepository = serviceProvider.GetRequiredService<OptionSetMetadataRepository>())
            {
                var tasks = 
                    settings
                    .Entities
                    .GroupBy(p => p)
                    .Select(p => p.First())
                    .Select(p => 
                        {
                            logger.LogInformation($"Fetching metadata of entity '{p}'");
                            return entityDefinitionRepository.GetByLogicalName(p);
                        }).ToArray();
                Task.WaitAll(tasks);
                var entities = tasks.Select(p => p.Result).ToList();
                
                logger.LogInformation($"Entities metadata done. Ready to fetch picklists metadata.");
                var pickLists = new Dictionary<string, Task<PicklistAttributeMetadata>>();
                var referencedEntities = new Dictionary<string, Task<string>>();
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

                                logger.LogInformation($"Fetching metadata of picklist '{key}'");
                                pickLists.Add(key, picklistRepository.GetOptionsetMetadata(q));                           
                            });

                        p.Attributes
                        .Where(q => q.AttributeType == AttributeTypeCode.Lookup)
                        .GroupBy(q => q.LogicalName)
                        .Select(q => q.First())
                        .ToList()
                        .ForEach(
                            q => 
                            {                            
                                foreach (var target in ((LookupAttributeMetadata)q).Targets)
                                {
                                    if (referencedEntities.ContainsKey(target))
                                        return;
                                    referencedEntities.Add(target, entityDefinitionRepository.GetLogicalCollectionName(target));                                        
                                }
                        
                            });                            


                    });

                Task.WaitAll(pickLists.Select(p => p.Value).ToArray());
                Task.WaitAll(referencedEntities.Select(p => p.Value).ToArray());


                logger.LogInformation($"Fechting metadata done. Ready to generate code");
                new CodeGen()
                .Execute(
                    settings, 
                    entities, 
                    pickLists.ToDictionary(p => p.Key, p => p.Value.Result),
                    referencedEntities.ToDictionary(p => p.Key, p => p.Value.Result)
                    );
                logger.LogInformation($"Code generated in file {settings.FileName}");
                logger.LogInformation($"Processing time: {(DateTime.Now - now).ToString("c")}");
            logger.LogInformation($"{nameof(CodeGenProcess)}. Ending process.");
            }            
        }

    }
}
