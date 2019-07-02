using System;

namespace Microsoft.Dynamics.CRM 
{

    ///<chref="https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/picklistattributemetadata?view=dynamics-ce-odata-9" />
    public sealed class PicklistAttributeMetadata: crmbaseentity
    {
         public override string EntityCollectionName
        {
            get
            {
                return "EntityDefinitions";
            }
        } 
    
        //The name of the attribute that this attribute extends.
        public string AttributeOf { get; set; }

        //The type for the attribute.

        public AttributeTypeCode AttributeType { get; set; }

        //The name of the type for the attribute.
        public AttributeTypeDisplayName AttributeTypeName { get; set; }

        public string AutoNumberFormat { get; set; }
        //Whether field-level security can be applied to prevent a user from adding data to this attribute.
        public bool CanBeSecuredForCreate { get; set; }

        //Whether field-level security can be applied to prevent a user from viewing data from this attribute.
        public bool CanBeSecuredForRead { get; set; }

        //Whether field-level security can be applied to prevent a user from updating data for this attribute.
        public bool CanBeSecuredForUpdate { get; set; }

        //Whether any settings not controlled by managed properties can be changed.
        public BooleanManagedProperty CanModifyAdditionalSettings { get; set; }

        //An organization-specific ID for the attribute used for auditing.
        public int ColumnNumber { get; set; }

        //The default form value for the attribute.
        public int DefaultFormValue { get; set; }

        //The Microsoft Dynamics 365 version that the attribute was deprecated in.
        public string DeprecatedVersion { get; set; }

        //The label containing the description for the attribute.
        public Label Description { get; set; }

        //A label containing the display name for the attribute.
        public Label DisplayName { get; set; }

        //The logical name of the entity that contains the attribute.
        public new string EntityLogicalName { get; set; }

        public string ExternalName { get; set; }
        //The formula definition for calculated and rollup attributes.
        public string FormulaDefinition { get; set; }

        //Indicates whether the item of metadata has changed.
        public bool? HasChanged { get; set; }

        //For internal use only.
        public string InheritsFrom { get; set; }


        //A string identifying the solution version that the solution component was added in.
        public string IntroducedVersion { get; set; }

        //Whether the attribute is enabled for auditing.
        public BooleanManagedProperty IsAuditEnabled { get; set; }

        public bool IsCustomAttribute { get; set; }
        //Whether the attribute is a custom attribute.

        //Whether the attribute allows customization.
        public BooleanManagedProperty IsCustomizable { get; set; }

        public bool? IsDataSourceSecret { get; set; }
        //For internal use only.
        public bool? IsFilterable { get; set; }

        //For internal use only.
        public BooleanManagedProperty IsGlobalFilterEnabled { get; set; }

        //Whether the attribute is a logical attribute.
        public bool? IsLogical { get; set; }

        //Whether the attribute is part of a managed solution.
        public bool? IsManaged { get; set; }

        //Whether the attribute represents the unique identifier for the record.
        public bool? IsPrimaryId { get; set; }

        //Whether the attribute represents the primary attribute for the entity.
        public bool? IsPrimaryName { get; set; }

        //Whether the attribute display name can be changed.
        public BooleanManagedProperty IsRenameable { get; set; }

        public bool IsRequiredForForm { get; set; }
        //For internal use only.
        public bool IsRetrievable { get; set; }

        //For internal use only.
        public bool IsSearchable { get; set; }

        //Whether the attribute is secured for field-level security.
        public bool IsSecured { get; set; }

        //For internal use only.
        public BooleanManagedProperty IsSortableEnabled { get; set; }

        //Whether the attribute appears in Advanced Find.
        public BooleanManagedProperty IsValidForAdvancedFind { get; set; }

        //Whether the value can be set when a record is created.
        public bool IsValidForCreate { get; set; }

        public bool IsValidForForm { get; set; }
        public bool IsValidForGrid { get; set; }
        //Whether the value can be retrieved.
        public bool IsValidForRead { get; set; }

        //Whether the value can be updated.
        public bool IsValidForUpdate { get; set; }

        //The id of the attribute that is linked between appointments and recurring appointments.
        public Guid? LinkedAttributeId { get; set; }

        //The logical name for the attribute.
        public string LogicalName { get; set; }

        //A unique identifier for the metadata item.
        public Guid MetadataId { get; set; }

        //The property that determines the data entry requirement level enforced for the attribute.
        public AttributeRequiredLevelManagedProperty RequiredLevel	{ get; set; }	

        //The schema name for the attribute.
        public string SchemaName { get; set; }

        //A value that indicates the source type for a calculated or rollup attribute.
        public int? SourceType { get; set; }

        //A bitmask value that describes the sources of data used in a calculated attribute or whether the data sources are invalid.
        public int? SourceTypeMask { get; set; }

        public OptionSetMetadata GlobalOptionSet { get; set; }
        public OptionSetMetadata OptionSet { get; set; }
    }
}