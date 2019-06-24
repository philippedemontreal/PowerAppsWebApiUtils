using app.Security;
using Microsoft.Dynamics.CRM;
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
            return await Retrieve($"{OdataEntityName}(LogicalName='{logicalName}')?$expand=Attributes,Keys,ManyToManyRelationships,ManyToOneRelationships,OneToManyRelationships");     
        }

    }
}