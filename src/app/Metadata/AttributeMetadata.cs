using System;
using System.Collections.Generic;

namespace Microsoft.Dynamics.CRM
{
	///<chref="https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/attributemetadata?view=dynamics-ce-odata-9" />
	///<para>Attribute IsValidODataAttribute is not documented</para>
    public sealed class AttributeMetadata
    {
		//Undocumented attribute
		public bool IsValidODataAttribute	{ get; set; }

		public string AttributeOf { get; set; }
		//The name of the attribute that this attribute extends.

		public AttributeTypeCode AttributeType { get; set; }
		//The type for the attribute.

		public AttributeTypeDisplayName AttributeTypeName	{ get; set; }	
		//The name of the type for the attribute.

		public string AutoNumberFormat { get; set; }
		public bool CanBeSecuredForCreate	{ get; set; }
		//Whether field-level security can be applied to prevent a user from adding data to this attribute.

		public bool CanBeSecuredForRead	{ get; set; }
		//Whether field-level security can be applied to prevent a user from viewing data from this attribute.

		public bool CanBeSecuredForUpdate { get; set; }
		//Whether field-level security can be applied to prevent a user from updating data for this attribute.

		public BooleanManagedProperty CanModifyAdditionalSettings  { get; set; }	
		//Whether any settings not controlled by managed properties can be changed.

		public int ColumnNumber { get; set; }
		//An organization-specific ID for the attribute used for auditing.

		public string DeprecatedVersion { get; set; }
		//The Microsoft Dynamics 365 version that the attribute was deprecated in.

		public Label Description { get; set; }	
		//The label containing the description for the attribute.

		public Label DisplayName { get; set; }
		//A label containing the display name for the attribute.

		public string EntityLogicalName { get; set; }
		//The logical name of the entity that contains the attribute.

		public string ExternalName { get; set; }
		public bool? HasChanged	{ get; set; }
		//Indicates whether the item of metadata has changed.

		public string InheritsFrom { get; set; }
		//For internal use only.

		public string IntroducedVersion { get; set; }
		//A string identifying the solution version that the solution component was added in.

		public BooleanManagedProperty IsAuditEnabled  { get; set; }	
		//Whether the attribute is enabled for auditing.

		public bool IsCustomAttribute	{ get; set; }
		//Whether the attribute is a custom attribute.

		public BooleanManagedProperty IsCustomizable  { get; set; }	
		//Whether the attribute allows customization.

		public bool IsDataSourceSecret	{ get; set; }
		public bool IsFilterable { get; set; }
		//For internal use only.

		public BooleanManagedProperty IsGlobalFilterEnabled  { get; set; }	
		//For internal use only.

		public bool? IsLogical  { get; set; }	
		//Whether the attribute is a logical attribute.

		public bool? IsManaged  { get; set; }	
		//Whether the attribute is part of a managed solution.

		public bool IsPrimaryId  { get; set; }	
		//Whether the attribute represents the unique identifier for the record.

		public bool? IsPrimaryName  { get; set; }	
		//Whether the attribute represents the primary attribute for the entity.

		public BooleanManagedProperty IsRenameable  { get; set; }	
		//Whether the attribute display name can be changed.

		public bool? IsRequiredForForm  { get; set; }	
		public bool? IsRetrievable  { get; set; }	
		//For internal use only.

		public bool? IsSearchable  { get; set; }	
		//For internal use only.

		public bool? IsSecured  { get; set; }	
		//Whether the attribute is secured for field-level security.

		public BooleanManagedProperty IsSortableEnabled  { get; set; }	
		//For internal use only.

		public BooleanManagedProperty IsValidForAdvancedFind  { get; set; }	
		//Whether the attribute appears in Advanced Find.

		public bool IsValidForCreate  { get; set; }	
		//Whether the value can be set when a record is created.

		public bool IsValidForForm  { get; set; }	
		public bool IsValidForGrid  { get; set; }	
		public bool IsValidForRead  { get; set; }	
		//Whether the value can be retrieved.

		public bool IsValidForUpdate  { get; set; }	
		//Whether the value can be updated.

		public Guid? LinkedAttributeId  { get; set; }	
		//The id of the attribute that is linked between appointments and recurring appointments.

		public string LogicalName { get; set; }
		//The logical name for the attribute.

		public Guid MetadataId  { get; set; }	
		//A unique identifier for the metadata item.

		public AttributeRequiredLevelManagedProperty RequiredLevel { get; set; }		
		//The property that determines the data entry requirement level enforced for the attribute.

		public string SchemaName { get; set; }
		//The schema name for the attribute.

		public int? SourceType	 { get; set; }
		//A value that indicates the source type for a calculated or rollup attribute.

    }
}
