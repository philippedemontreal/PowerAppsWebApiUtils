using System;

namespace Microsoft.Dynamics.CRM
{   

    ///<chref="https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/onetomanyrelationshipmetadata?view=dynamics-ce-odata-9"/>
    public sealed class OneToManyRelationshipMetadata
    {
        //The associated menu configuration.
        public AssociatedMenuConfiguration AssociatedMenuConfiguration { get; set; }	

        //The cascading behaviors for the entity relationship.
        public CascadeConfiguration CascadeConfiguration { get; set; }	

        //Indicates whether the item of metadata has changed.
        public bool? HasChanged { get; set; }	

        //A string identifying the solution version that the solution component was added in.
        public string IntroducedVersion { get; set; }	

        //Whether the entity relationship is customizable.
        public BooleanManagedProperty IsCustomizable { get; set; }	

        //Whether the relationship is a custom relationship.
        public bool IsCustomRelationship { get; set; }	

        //Whether this relationship is the designated hierarchical self-referential relationship for this entity.
        public bool IsHierarchical { get; set; }	

        //Whether the entity relationship is part of a managed solution.
        public bool IsManaged { get; set; }	

        //Whether the entity relationship should be shown in Advanced Find.
        public bool IsValidForAdvancedFind { get; set; }	

        //A unique identifier for the metadata item.
        public Guid MetadataId { get; set; }	

        //The name of primary attribute for the referenced entity.
        public string ReferencedAttribute { get; set; }	

        //The name of the referenced entity.
        public string ReferencedEntity { get; set; }	

        //The collection-valued navigation property used by this relationship.
        public string ReferencedEntityNavigationPropertyName { get; set; }	

        //The name of the referencing attribute.
        public string ReferencingAttribute { get; set; }	

        //The name of the referencing entity.
        public string ReferencingEntity { get; set; }	

        //The single-valued navigation property used by this relationship.
        public string ReferencingEntityNavigationPropertyName { get; set; }	

        //The type of relationship.
        public RelationshipType RelationshipType { get; set; }	

        //The schema name for the entity relationship.
        public string SchemaName { get; set; }	

        //The security type for the relationship.        
        public SecurityTypes SecurityTypes { get; set; }	
    }
}