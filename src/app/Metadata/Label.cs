using System;
using System.Collections.Generic;

namespace Microsoft.Dynamics.CRM
{
        
    ///<chref="https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/label?view=dynamics-ce-odata-9" />
    public class Label  
    {
        public List<LocalizedLabel> LocalizedLabels { get; set; }
        public LocalizedLabel UserLocalizedLabel { get; set; }
    
    }
}