using System;
using System.Collections.Generic;

namespace Microsoft.Dynamics.CRM
{


    ///<chref="https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/booleanmanagedproperty?view=dynamics-ce-odata-9" />
        public sealed class BooleanManagedProperty  
    {
        //The value of the managed property.
        public bool Value { get; set; }
        //Whether the managed property value can be changed.
        public bool CanBeChanged { get; set; }
        //The logical name for the managed property.
        public string ManagedPropertyLogicalName { get; set; }
    }
}