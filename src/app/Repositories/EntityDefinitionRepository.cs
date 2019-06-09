using app.Configuration;
using app.Entities;
using System;
using System.Threading.Tasks;

namespace app.Repositories
{
    public class EntityDefinitionRepository : GenericRepository<EntityDefinition>
    {
        public EntityDefinitionRepository(IConfigurationReader configurationReader) : 
        base(configurationReader, "EntityDefinitions")
        {

        }
                    
        public async Task<EntityDefinition> GetByLogicalName(string logicalName)
        {
            return await Retrieve($"{OdataEntityName}(LogicalName='{logicalName}')?$expand=Attributes");     
        }
    }
}