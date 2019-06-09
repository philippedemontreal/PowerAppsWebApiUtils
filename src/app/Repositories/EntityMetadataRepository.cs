using app.Configuration;
using app.Metadata;
using app.Security;
using System;
using System.Threading.Tasks;

namespace app.Repositories
{
    public class EntityMetadataRepository : GenericRepository<EntitMetadata>
    {
        public EntityMetadataRepository(PowerAppsConfiguration powerAppsConfiguration) : 
        base(powerAppsConfiguration, "EntityDefinitions")
        {
        }
                    
        public async Task<EntitMetadata> GetByLogicalName(string logicalName)
        {
            return await Retrieve($"{OdataEntityName}(LogicalName='{logicalName}')?$expand=Attributes");     
        }
    }
}