using System;
using System.Collections.Generic;

namespace Microsoft.Dynamics.CRM 
{
    ///<chref="https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/optionsettype?view=dynamics-ce-odata-9"/>
    public enum OptionSetType
    {
        //The option set provides a list of options.
        Picklist =	0,
        //	The option set represents state options for a StateAttributeMetadata attribute.
        State =	1,
        //	The option set represents status options for a StatusAttributeMetadata attribute.
        Status = 2,
        //	The option set provides two options for a BooleanAttributeMetadata attribute.
        Boolean = 3,
    }

    ///<chref="https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/optionsetmetadata?view=dynamics-ce-odata-9" />
    public sealed class OptionSetMetadata 
    {
        //A description for the option set.
        public Label Description { get; set; }

        //A label containing the display name for the global option set.
        public Label DisplayName { get; set; }

        public string ExternalTypeName { get; set; }
        //Indicates whether the item of metadata has changed.
        public bool? HasChanged { get; set; }

        //A string identifying the solution version that the solution component was added in.
        public string IntroducedVersion { get; set; }

        //Whether the option set is customizable.
        public BooleanManagedProperty IsCustomizable { get; set; }

        //Whether the option set is a custom option set.
        public bool IsCustomOptionSet { get; set; }

        //Whether the option set is a global option set.
        public bool IsGlobal { get; set; }

        //Whether the option set is part of a managed solution.
        public bool IsManaged { get; set; }

        //A unique identifier for the metadata item.
        public Guid MetadataId	{ get; set; }

        //The name of a global option set.
        public string Name { get; set; }

        public List<OptionMetadata> Options { get; set; }
        //The type of option set.
        public OptionSetType? OptionSetType { get; set; }
    }
}