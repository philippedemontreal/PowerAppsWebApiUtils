using app.Configuration;
using app.Security;
using Microsoft.Dynamics.CRM;
using System;
using System.Threading.Tasks;

namespace app.Repositories
{
    public class OptionSetMetadataRepository : GenericRepository<PicklistAttributeMetadata>
    {
        public OptionSetMetadataRepository(AuthenticationMessageHandler tokenProvider) : 
        base(tokenProvider)
        {
        }
                    
        public async Task<PicklistAttributeMetadata> GetOptionsetMetadata(AttributeMetadata attribute)
        {
            return await Retrieve($"{OdataEntityName}(LogicalName='{attribute.EntityLogicalName}')/Attributes(LogicalName='{attribute.LogicalName}')/Microsoft.Dynamics.CRM.{attribute.AttributeType}AttributeMetadata?$&$expand=OptionSet,GlobalOptionSet"); 
        }
    }
}