using System;
using System.Collections.Generic;

namespace app.Entities
{

    public enum RequiredLevel
    {
      None,
      ApplicationRequired,
      SystemRequired,

    }
    
   
    public class RequiredLevelProperty  
    {
		public RequiredLevel Value { get; set; }
		public bool CanBeChanged { get; set; }
		public string ManagedPropertyLogicalName { get; set; }
    }
}