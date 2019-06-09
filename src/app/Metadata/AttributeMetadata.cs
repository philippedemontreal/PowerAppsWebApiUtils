using System;
using System.Collections.Generic;

namespace app.Metadata
{
    public sealed class AttributeMetadata
    {
        public Guid MetadataId { get; set; }
		public bool? HasChanged { get; set; }
		public string AttributeOf { get; set; }
		public AttributeTypeCode AttributeType { get; set; }
		public int ColumnNumber { get; set; }
		public string DeprecatedVersion { get; set; }
		public string IntroducedVersion { get; set; }
		public string EntityLogicalName { get; set; }
		public bool IsCustomAttribute { get; set; }
		public bool IsPrimaryId { get; set; }
		public bool IsPrimaryName { get; set; }
		public bool IsValidForCreate { get; set; }
		public bool IsValidForRead { get; set; }
		public bool IsValidForUpdate { get; set; }
		public bool CanBeSecuredForRead { get; set; }
		public bool CanBeSecuredForCreate { get; set; }
		public bool CanBeSecuredForUpdate { get; set; }
		public bool IsSecured { get; set; }
		public bool IsRetrievable { get; set; }
		public bool IsFilterable { get; set; }
		public bool IsSearchable { get; set; }
		public bool IsManaged { get; set; }
		public Guid? LinkedAttributeId { get; set; }
		public string LogicalName { get; set; }
		public bool IsValidForForm { get; set; }
		public bool IsRequiredForForm { get; set; }
		public bool IsValidForGrid { get; set; }
		public string SchemaName { get; set; }
		public string ExternalName { get; set; }
		public bool IsLogical { get; set; }
		public bool IsDataSourceSecret { get; set; }
		public string InheritsFrom { get; set; }
		public string SourceType { get; set; }
		public string AutoNumberFormat { get; set; }  		
                
        public Label Description { get; set; }
        public Label DisplayName { get; set; }
        public ManagedProperty IsAuditEnabled { get; set; }
        public ManagedProperty IsGlobalFilterEnabled { get; set; }
        public ManagedProperty IsSortableEnabled { get; set; }
        public ManagedProperty IsCustomizable { get; set; }
        public ManagedProperty IsRenameable { get; set; }
        public ManagedProperty IsValidForAdvancedFind { get; set; }
        public RequiredLevelProperty RequiredLevel { get; set; }
        public ManagedProperty CanModifyAdditionalSettings { get; set; }

    }
}

//{

		// "AttributeTypeName": {
		// 	"Value": "UniqueidentifierType"
		// },