using System;

namespace Microsoft.Dynamics.CRM 
{
    
    ///<chref="https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/optionmetadata?view=dynamics-ce-odata-9" />
    public sealed class OptionMetadata 
    {
        public int? Value { get; set; }
        //The value of the option.

        public Label Label { get; set; }
        //The label containing the text for the option.

        //The label containing the description for the option.
        public Label Description { get; set; }

        //The Hex color assigned to the option
        public string Color { get; set; }

        //Whether the option is part of a managed solution.
        public bool? IsManaged { get; set; }

        public string ExternalValue { get; set; }
        //A unique identifier for the metadata item
        public Guid? MetadataId { get; set; }

        //Gets whether the item of metadata has changed.
        public bool? HasChanged { get; set; }
    }
}