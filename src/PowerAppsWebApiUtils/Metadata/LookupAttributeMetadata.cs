using System;
using System.Collections.Generic;

namespace Microsoft.Dynamics.CRM
{   

    ///<chref="https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/lookupattributemetadata?view=dynamics-ce-odata-9"/>
    public sealed class LookupAttributeMetadata: AttributeMetadata
    {

        public string[]  Targets { get; set; }	
        
        public List<OneToManyRelationshipMetadata> OneToManyRelationships	{ get; set; }   
    
    }
}