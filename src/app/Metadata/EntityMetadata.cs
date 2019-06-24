using System;
using System.Collections.Generic;

namespace Microsoft.Dynamics.CRM
{   

    ///<chref="https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/ownershiptypes?view=dynamics-ce-odata-9"/>
    public enum OwnershipTypes
    {
        None = 	0, //	The entity does not have an owner. For internal use only.
        UserOwned = 1, //	The entity is owned by a system user.
        TeamOwned = 2, //	The entity is owned by a team. For internal use only.
        BusinessOwned = 4, //	The entity is owned by a business unit. For internal use only.
        OrganizationOwned = 8, //	The entity is owned by an organization.
        BusinessParented = 16, //	The entity is parented by a business unit. For internal use only.
    }

    ///<chref="https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/entitymetadata?view=dynamics-ce-odata-9"/>
    public sealed class EntityMetadata
    {
        public int ActivityTypeMask { get; set; }	
        //Whether a custom activity should appear in the activity menus in the Web application.

        public bool AutoCreateAccessTeams { get; set; }	
        //Indicates whether the entity is enabled for auto created access teams.

        public bool AutoRouteToOwnerQueue { get; set; }	
        //Indicates whether to automatically move records to the owner’s default queue when a record of this type is created or assigned

        public BooleanManagedProperty CanBeInManyToMany { get; set; }	
        //Whether the entity can be in a Many-to-Many entity relationship.

        public BooleanManagedProperty CanBePrimaryEntityInRelationship { get; set; }	
        //Whether the entity can be the referenced entity in a One-to-Many entity relationship.

        public BooleanManagedProperty CanBeRelatedEntityInRelationship { get; set; }	
        //Whether the entity can be the referencing entity in a One-to-Many entity relationship.

        public BooleanManagedProperty CanChangeHierarchicalRelationship { get; set; }	
        //Whether the hierarchical state of entity relationships included in your managed solutions can be changed.

        public BooleanManagedProperty CanChangeTrackingBeEnabled { get; set; }	
        //For internal use only.

        public BooleanManagedProperty CanCreateAttributes { get; set; }	
        //Whether additional attributes can be added to the entity.

        public BooleanManagedProperty CanCreateCharts { get; set; }	
        //Whether new charts can be created for the entity.

        public BooleanManagedProperty CanCreateForms { get; set; }	
        //Whether new forms can be created for the entity.

        public BooleanManagedProperty CanCreateViews { get; set; }	
        //Whether new views can be created for the entity.

        public BooleanManagedProperty CanEnableSyncToExternalSearchIndex { get; set; }	
        //For internal use only.

        public BooleanManagedProperty CanModifyAdditionalSettings { get; set; }	
        //Whether any other entity properties not represented by a managed property can be changed.

        public bool CanTriggerWorkflow { get; set; }	
        //Whether the entity can trigger a workflow process.

        public bool ChangeTrackingEnabled { get; set; }	
        //Whether change tracking is enabled for an entity.

        public string CollectionSchemaName { get; set; }	
        //the collection schema name of the entity.

        public Guid? DataProviderId { get; set; }	
        public Guid? DataSourceId { get; set; }	
        public int DaysSinceRecordLastModified { get; set; }	
        public Label Description { get; set; }
        //the label containing the description for the entity.

        public Label DisplayCollectionName { get; set; }
        //A label containing the plural display name for the entity.

        public Label DisplayName { get; set; }
        //A label containing the display name for the entity.

        public bool EnforceStateTransitions { get; set; }	
        //Whether the entity will enforce custom state transitions.

        public string EntityColor { get; set; }	
        //the hexadecimal code to represent the color to be used for this entity in the application.

        public string EntityHelpUrl { get; set; }	
        //the URL of the resource to display help content for this entity

        public bool EntityHelpUrlEnabled { get; set; }	
        //Whether the entity supports custom help content.

        public string EntitySetName { get; set; }	
        //The name of the Web API entity set for this entity.

        public string ExternalCollectionName { get; set; }	
        public string ExternalName { get; set; }	
        public bool HasActivities { get; set; }	
        //Whether activities are associated with this entity.

        public bool? HasChanged { get; set; }	
        //Indicates whether the item of metadata has changed.

        public bool HasFeedback { get; set; }	
        //Whether the entity will have a special relationship to the Feedback entity.

        public bool HasNotes { get; set; }	
        //Whether notes are associated with this entity.

        public string IconLargeName { get; set; }	
        //The name of the image web resource for the large icon for the entity.

        public string IconMediumName { get; set; }	
        //The name of the image web resource for the medium icon for the entity.

        public string IconSmallName { get; set; }	
        //The name of the image web resource for the small icon for the entity.

        public string IconVectorName { get; set; }	
        public string IntroducedVersion { get; set; }	
        //A string identifying the solution version that the solution component was added in.

        public bool IsActivity { get; set; }	
        //Whether the entity is an activity.

        public bool IsActivityParty { get; set; }	
        //Whether the email messages can be sent to an email address stored in a record of this type.

        public bool IsAIRUpdated { get; set; }	
        //Whether the entity uses the updated user interface.

        public BooleanManagedProperty IsAuditEnabled { get; set; }	
        //Whether auditing has been enabled for the entity.

        public bool IsAvailableOffline { get; set; }	
        //Whether the entity is available offline.

        public bool IsBPFEntity { get; set; }	
        public bool IsBusinessProcessEnabled { get; set; }	
        //Whether the entity is enabled for business process flows.

        public bool IsChildEntity { get; set; }	
        //Whether the entity is a child entity.

        public BooleanManagedProperty IsConnectionsEnabled { get; set; }	
        //Whether connections are enabled for this entity.

        public bool IsCustomEntity { get; set; }	
        //Whether the entity is a custom entity.

        public BooleanManagedProperty IsCustomizable { get; set; }	
        //Whether the entity is customizable.

        public bool IsDocumentManagementEnabled { get; set; }	
        //Whether document management is enabled.

        public bool IsDocumentRecommendationsEnabled { get; set; }	
        public BooleanManagedProperty IsDuplicateDetectionEnabled { get; set; }	
        //Whether duplicate detection is enabled.

        public bool IsEnabledForCharts { get; set; }	
        //Whether charts are enabled.

        public bool IsEnabledForExternalChannels { get; set; }	
        //Whether this entity is enabled for external channels

        public bool IsEnabledForTrace { get; set; }	
        //For internal use only.

        public bool IsImportable { get; set; }	
        //Whether the entity can be imported using the Import Wizard.

        public bool IsInteractionCentricEnabled { get; set; }	
        //Whether the entity is enabled for interactive experience.

        public bool IsIntersect { get; set; }	
        //Whether the entity is an intersection table for two other entities.

        public bool IsKnowledgeManagementEnabled { get; set; }	
        //Whether Parature knowledge management integration is enabled for the entity.

        public bool IsLogicalEntity { get; set; }	
        public BooleanManagedProperty IsMailMergeEnabled { get; set; }	
        //Whether mail merge is enabled for this entity.

        public bool IsManaged { get; set; }	
        //Whether the entity is part of a managed solution.

        public BooleanManagedProperty IsMappable { get; set; }	
        //Whether entity mapping is available for the entity.

        public BooleanManagedProperty IsOfflineInMobileClient { get; set; }	
        //Whether this entity is enabled for offline data with Dynamics 365 for tablets and Dynamics 365 for phones.

        public bool IsOneNoteIntegrationEnabled { get; set; }	
        //Whether OneNote integration is enabled for the entity.

        public bool IsOptimisticConcurrencyEnabled { get; set; }	
        //Whether optimistic concurrency is enabled for the entity

        public bool IsPrivate { get; set; }	
        //For internal use only.

        public bool IsQuickCreateEnabled { get; set; }	
        //Whether the entity is enabled for quick create forms.

        public bool IsReadingPaneEnabled { get; set; }	
        //For internal use only.

        public BooleanManagedProperty IsReadOnlyInMobileClient { get; set; }	
        //Whether Microsoft Dynamics 365 for tablets users can update data for this entity.

        public BooleanManagedProperty IsRenameable { get; set; }	
        //Whether the entity DisplayName and DisplayCollectionName can be changed by editing the entity in the application.

        public bool IsSLAEnabled { get; set; }	
        public bool IsStateModelAware { get; set; }	
        //Whether the entity supports setting custom state transitions.

        public bool IsValidForAdvancedFind { get; set; }	
        //Whether the entity is will be shown in Advanced Find.

        public BooleanManagedProperty IsValidForQueue { get; set; }	
        //Whether the entity is enabled for queues.

        public BooleanManagedProperty IsVisibleInMobile { get; set; }	
        //Whether Microsoft Dynamics 365 for phones users can see data for this entity.

        public BooleanManagedProperty IsVisibleInMobileClient { get; set; }	
        //Whether Microsoft Dynamics 365 for tablets users can see data for this entity.

        public string LogicalCollectionName { get; set; }	
        //the logical collection name.

        public string LogicalName { get; set; }	
        //the logical name for the entity.

        public Guid MetadataId { get; set; }	
        //A unique identifier for the metadata item.

        public string MobileOfflineFilters { get; set; }	
        public int ObjectTypeCode { get; set; }	
        //the entity type code.

        public OwnershipTypes OwnershipType	 { get; set; }		
        //the ownership type for the entity.

        public string PrimaryIdAttribute { get; set; }	
        //The name of the attribute that is the primary id for the entity.

        public string PrimaryImageAttribute { get; set; }	
        //The name of the primary image attribute for an entity.

        public string PrimaryNameAttribute { get; set; }	
        //The name of the primary attribute for an entity.

        public List<SecurityPrivilegeMetadata> Privileges	{ get; set; }	
        //the privilege metadata for the entity.

        public string RecurrenceBaseEntityLogicalName { get; set; }	
        //The name of the entity that is recurring.

        public string ReportViewName { get; set; }	
        //The name of the report view for the entity.

        public string SchemaName { get; set; }	
        //the schema name for the entity.

        public bool SyncToExternalSearchIndex { get; set; }	
        public bool UsesBusinessDataLabelTable { get; set; }   


        public List<AttributeMetadata> Attributes { get; set; }   
        public List<EntityKeyMetadata> Keys	{ get; set; }   
        public List<ManyToManyRelationshipMetadata> ManyToManyRelationships { get; set; }
        public List<OneToManyRelationshipMetadata> ManyToOneRelationships { get; set; }   
        public List<OneToManyRelationshipMetadata> OneToManyRelationships	{ get; set; }   
    
    }
}