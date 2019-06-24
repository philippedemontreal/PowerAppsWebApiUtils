using System;

namespace Microsoft.Dynamics.CRM
{
    ///<chref="https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/attributerequiredlevel?view=dynamics-ce-odata-9" />

    public enum AttributeRequiredLevel
    {
      None =	0,	//No requirements are specified.
      SystemRequired	= 1, //The attribute is required to have a value.
      ApplicationRequired	= 2,	//The attribute is required to have a value.
      Recommended	= 3,	//It is recommended that the attribute has a value.

    }
    
    ///<chref="https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/attributerequiredlevelmanagedproperty?view=dynamics-ce-odata-9" />

    public sealed class AttributeRequiredLevelManagedProperty  
    {
		public AttributeRequiredLevel Value { get; set; }
		public bool CanBeChanged { get; set; }
		public string ManagedPropertyLogicalName { get; set; }
    }
}