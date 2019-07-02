using System;

namespace Microsoft.Dynamics.CRM
{   
    ///<chref="https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/entitykeyindexstatus?view=dynamics-ce-odata-9"/>
    public enum EntityKeyIndexStatus
    {
        Pending	= 0, //Specifies that the key index creation is pending.
        InProgress	= 1, //Specifies that the key index creation is in progress.
        Active = 2, //Specifies that the key index creation is active.
        Failed = 3,	//Specifies that the key index creation failed.
    }

    ///<chref="https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/entitykeymetadata?view=dynamics-ce-odata-9"/>
    public sealed class EntityKeyMetadata
    {
        //The asynchronous job.
        public Guid? AsyncJob { get; set; }

        //A label containing the display name for the key.
        public Label DisplayName { get; set; }

        //The entity key index status.
        public EntityKeyIndexStatus EntityKeyIndexStatus { get; set; }

        //The entity logical name.
        public string EntityLogicalName	{ get; set; }

        //Indicates whether the item of metadata has changed.
        public bool? HasChanged	{ get; set; }


        //A string identifying the solution version that the solution component was added in.
        public string IntroducedVersion { get; set; }

        //Whether the entity key metadata is customizable.
        public BooleanManagedProperty IsCustomizable{ get; set; }

        //Whether entity key metadata is managed or not.
        public bool IsManaged { get; set; }

        //The key attributes.
        public string KeyAttributes { get; set; }

        //The logical name.
        public string LogicalName { get; set; }

        //A unique identifier for the metadata item.
        public Guid MetadataId { get; set; }	

        //The schema name.
        public string SchemaName { get; set; }
    }
}