using Microsoft.Dynamics.CRM;
using PowerAppsWebApiUtils.Security;
using System;
using System.Threading.Tasks;

namespace PowerAppsWebApiUtils.Repositories
{
    internal class OptionSetMetadataRepository : GenericRepository<PicklistAttributeMetadata>
    {
        internal OptionSetMetadataRepository(AuthenticationMessageHandler tokenProvider) : 
        base(tokenProvider)
        {
        }
                    
        internal async Task<PicklistAttributeMetadata> GetOptionsetMetadata(AttributeMetadata attribute)
        {
            return await Retrieve($"{OdataEntityName}(LogicalName='{attribute.EntityLogicalName}')/Attributes(LogicalName='{attribute.LogicalName}')/Microsoft.Dynamics.CRM.{attribute.AttributeType}AttributeMetadata?$&$expand=OptionSet,GlobalOptionSet"); 
        }
    }
}