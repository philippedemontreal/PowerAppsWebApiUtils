using System;
using System.Runtime.Serialization;


namespace Microsoft.Dynamics.CRM
{
    public class crmbaseentity
    {
        [IgnoreDataMember]
        public virtual string EntityLogicalName { get;  }
        
        [IgnoreDataMember]
        public virtual string EntityCollectionName { get; } 

        [IgnoreDataMember]               
        public virtual Guid Id { get; set; }

    }  
}