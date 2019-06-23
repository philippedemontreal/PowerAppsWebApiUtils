using System;
using System.Collections.Generic;

namespace Microsoft.Dynamics.CRM
{

    public enum AttributeRequiredLevel
    {
      ApplicationRequired,
      None,
      Recommended,
      SystemRequired,

    }
    
   
    public sealed class RequiredLevelProperty  
    {
		public AttributeRequiredLevel Value { get; set; }
		public bool CanBeChanged { get; set; }
		public string ManagedPropertyLogicalName { get; set; }
    }
}