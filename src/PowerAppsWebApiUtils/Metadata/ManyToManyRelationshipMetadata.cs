using System;

namespace Microsoft.Dynamics.CRM
{   

    ///<chref="https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/relationshiptype?view=dynamics-ce-odata-9"/>
    public enum RelationshipType
    {
        OneToManyRelationship = 0,	//The entity relationship is a One-to-Many relationship.
        ManyToManyRelationship = 1,	//The entity relationship is a Many-to-Many relationship.
    }

    ///<chref="https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/securitytypes?view=dynamics-ce-odata-9"/>
    public enum SecurityTypes
    {
        None = 0,	//No security privileges are checked during create or update operations.
        Append = 1,	//The Append and AppendTo privileges are checked for create or update operations.
        ParentChild = 2,	//Security for the referencing entity record is derived from the referenced entity record.
        Pointer = 4,	//Security for the referencing entity record is derived from a pointer record.
        Inheritance = 8,	//The referencing entity record inherits security from the referenced security record.
    }
    
    ///<chref="https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/manytomanyrelationshipmetadata?view=dynamics-ce-odata-9"/>
    public sealed class ManyToManyRelationshipMetadata
    {
        //The associated menu configuration for the first entity.
        public AssociatedMenuConfiguration Entity1AssociatedMenuConfiguration { get; set; }	

        //The attribute that defines the relationship in the first entity.
        public string Entity1IntersectAttribute { get; set; }	

        //The logical name of the first entity in the relationship.
        public string Entity1LogicalName { get; set; }	

        public string Entity1NavigationPropertyName { get; set; }	
        //The associated menu configuration for the second entity.
        public AssociatedMenuConfiguration Entity2AssociatedMenuConfiguration { get; set; }	

        //The attribute that defines the relationship in the second entity.
        public string Entity2IntersectAttribute { get; set; }	

        //The logical name of the second entity in the relationship.
        public string Entity2LogicalName { get; set; }	

        public string Entity2NavigationPropertyName { get; set; }	
        //Indicates whether the item of metadata has changed.
        public bool? HasChanged { get; set; }	

        public string IntersectEntityName { get; set; }	
        //The name of the intersect entity for the relationship.

        //A string identifying the solution version that the solution component was added in.
        public string IntroducedVersion { get; set; }	

        //whether the entity relationship is customizable.
        public BooleanManagedProperty IsCustomizable { get; set; }	

        //whether the relationship is a custom relationship.
        public bool IsCustomRelationship { get; set; }	

        //whether the entity relationship is part of a managed solution.
        public bool IsManaged { get; set; }	

        //whether the entity relationship should be shown in Advanced Find.
        public bool IsValidForAdvancedFind { get; set; }	

        //A unique identifier for the metadata item.
        public Guid MetadataId { get; set; }

        //The type of relationship.
        public RelationshipType RelationshipType { get; set; }	

        //The schema name for the entity relationship.
        public string SchemaName { get; set; }	

        //The security type for the relationship.    
        public SecurityTypes SecurityTypes { get; set; }	
    }
}