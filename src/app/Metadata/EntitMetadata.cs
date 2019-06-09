using System;
using System.Collections.Generic;

namespace app.Metadata
{
    public enum OwnershipTypes
    {
        BusinessOwned,
        BusinessParented,
        None,
        OrganizationOwned,
        TeamOwned,
        UserOwned,

    }
    public sealed class EntitMetadata
    {
        public int? ActivityTypeMask { get; set; }
        public bool AutoRouteToOwnerQueue { get; set; }
        public bool CanTriggerWorkflow { get; set; }
        public bool EntityHelpUrlEnabled{ get; set; }
        public string EntityHelpUrl{ get; set; }
        public bool IsDocumentManagementEnabled{ get; set; }
        public bool IsOneNoteIntegrationEnabled{ get; set; }
        public bool IsInteractionCentricEnabled{ get; set; }
        public bool IsKnowledgeManagementEnabled{ get; set; }
        public bool IsSLAEnabled{ get; set; }
        public bool IsBPFEntity{ get; set; }
        public bool IsDocumentRecommendationsEnabled{ get; set; }
        public bool IsMSTeamsIntegrationEnabled{ get; set; }
        public string DataProviderId{ get; set; }
        public string DataSourceId{ get; set; }
        public bool AutoCreateAccessTeams{ get; set; }
        public bool IsActivity{ get; set; }
        public bool IsActivityParty{ get; set; }
        public bool IsAvailableOffline{ get; set; }
        public bool IsChildEntity{ get; set; }
        public bool IsAIRUpdated{ get; set; }
        public string IconLargeName{ get; set; }
        public string IconMediumName{ get; set; }
        public string IconSmallName{ get; set; }
        public string IconVectorName{ get; set; }
        public string IsCustomEntity{ get; set; }
        public bool IsBusinessProcessEnabled{ get; set; }
        public bool SyncToExternalSearchIndex{ get; set; }
        public bool IsOptimisticConcurrencyEnabled{ get; set; }
        public bool ChangeTrackingEnabled{ get; set; }
        public bool IsImportable{ get; set; }
        public bool IsIntersect{ get; set; }
        public bool IsManaged{ get; set; }
        public bool IsEnabledForCharts{ get; set; }
        public bool IsEnabledForTrace{ get; set; }
        public bool IsValidForAdvancedFind { get; set; }
        public int DaysSinceRecordLastModified{ get; set; }
        public string MobileOfflineFilters{ get; set; }
        public bool IsReadingPaneEnabled{ get; set; }
        public bool IsQuickCreateEnabled{ get; set; }
        public string LogicalName{ get; set; }
        public int ObjectTypeCode{ get; set; }
        public OwnershipTypes OwnershipType{ get; set; }
        public string PrimaryNameAttribute{ get; set; }
        public string PrimaryImageAttribute{ get; set; }
        public string PrimaryIdAttribute{ get; set; }
        public string RecurrenceBaseEntityLogicalName{ get; set; }
        public string ReportViewName{ get; set; }
        public string SchemaName{ get; set; }
        public string IntroducedVersion{ get; set; }
        public bool IsStateModelAware{ get; set; }
        public bool EnforceStateTransitions{ get; set; }
        public string ExternalName{ get; set; }
        public string EntityColor{ get; set; }
        public string LogicalCollectionName{ get; set; }
        public string ExternalCollectionName{ get; set; }
        public string CollectionSchemaName{ get; set; }
        public string EntitySetName{ get; set; }
        public bool IsEnabledForExternalChannel{ get; set; }
        public bool IsPrivate{ get; set; }
        public bool UsesBusinessDataLabelTable{ get; set; }
        public bool IsLogicalEntity{ get; set; }
        public bool HasNotes{ get; set; }
        public bool HasActivities{ get; set; }
        public bool HasFeedback{ get; set; }
        public bool IsSolutionAware{ get; set; }
        public Guid MetadataId{ get; set; }
        public bool? HasChanged { get; set; }
        public Label Description { get; set; }
        public Label DisplayCollectionName { get; set; }
        public Label DisplayName { get; set; }
        public ManagedProperty IsAuditEnabled { get; set; }
        public ManagedProperty IsValidForQueue { get; set; }
        public ManagedProperty IsConnectionsEnabled { get; set; }
        public ManagedProperty IsCustomizable { get; set; }
        public ManagedProperty IsRenameable { get; set; }
        public ManagedProperty IsMappable { get; set; }
        public ManagedProperty IsDuplicateDetectionEnabled { get; set; }
        public ManagedProperty CanCreateAttributes { get; set; }
        public ManagedProperty CanCreateForms { get; set; }
        public ManagedProperty CanCreateViews { get; set; }
        public ManagedProperty CanCreateCharts { get; set; }
        public ManagedProperty CanBeRelatedEntityInRelationship { get; set; }
        public ManagedProperty CanBePrimaryEntityInRelationship { get; set; }
        public ManagedProperty CanBeInManyToMany { get; set; }
        public ManagedProperty CanBeInCustomEntityAssociation { get; set; }
        public ManagedProperty CanEnableSyncToExternalSearchIndex { get; set; }
        public ManagedProperty CanModifyAdditionalSettings { get; set; }
        public ManagedProperty CanChangeHierarchicalRelationship { get; set; }
        public ManagedProperty CanChangeTrackingBeEnabled { get; set; }
        public ManagedProperty IsMailMergeEnabled { get; set; }
        public ManagedProperty IsVisibleInMobile { get; set; }
        public ManagedProperty IsVisibleInMobileClient { get; set; }
        public ManagedProperty IsOfflineInMobileClient { get; set; }
        public List<SecurityPrivilegeMetadata> Privileges { get; set; }
        public List<AttributeMetadata> Attributes { get; set; }       
    
    }
}