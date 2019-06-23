using System;
using System.Collections.Generic;

namespace Microsoft.Dynamics.CRM
{
    ///<chref="https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/localizedlabel?view=dynamics-ce-odata-9" />
    public sealed class LocalizedLabel  
    {
       		
        public string Label { get; set; }
        public int LanguageCode { get; set; }
        public bool IsManaged{ get; set; }
        public Guid MetadataId{ get; set; }
        public bool? HasChanged{ get; set; }
    }
}