using Microsoft.Dynamics.CRM;
using PowerAppsWebApiUtils.Security;
using System;
using System.Threading.Tasks;

namespace PowerAppsWebApiUtils.Repositories
{
    internal class EntityMetadataRepository : GenericRepository<EntityMetadata>
    {
        internal EntityMetadataRepository(AuthenticationMessageHandler tokenProvider) : 
        base(tokenProvider)
        {
        }
                    
        internal async Task<EntityMetadata> GetByLogicalName(string logicalName)
        {
            return await Retrieve($"{OdataEntityName}(LogicalName='{logicalName}')?$expand=Attributes,Keys,ManyToManyRelationships,ManyToOneRelationships,OneToManyRelationships");     
        }

    }
}