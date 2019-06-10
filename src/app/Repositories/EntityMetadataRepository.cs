using app.Configuration;
using app.Metadata;
using app.Security;
using System;
using System.Threading.Tasks;

namespace app.Repositories
{
    public class EntityMetadataRepository : GenericRepository<EntityMetadata>
    {
        public EntityMetadataRepository(PowerAppsConfiguration powerAppsConfiguration) : 
        base(powerAppsConfiguration, "EntityDefinitions")
        {
        }
                    
        public async Task<EntityMetadata> GetByLogicalName(string logicalName)
        {
            return await Retrieve($"{OdataEntityName}(LogicalName='{logicalName}')?$expand=Attributes");     
        }
    }
}