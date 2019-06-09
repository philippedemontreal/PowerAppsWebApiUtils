using System;
using System.Collections.Generic;

namespace app.Metadata
{

    public enum AttributeRequiredLevel
    {
      ApplicationRequired,
      None,
      Recommended,
      SystemRequired,

    }
    
   
    public class RequiredLevelProperty  
    {
		public AttributeRequiredLevel Value { get; set; }
		public bool CanBeChanged { get; set; }
		public string ManagedPropertyLogicalName { get; set; }
    }
}