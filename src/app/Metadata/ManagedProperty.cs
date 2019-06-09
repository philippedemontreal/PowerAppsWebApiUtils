using System;
using System.Collections.Generic;

namespace app.Metadata
{
    public class ManagedProperty  
    {
		public bool Value { get; set; }
		public bool CanBeChanged { get; set; }
		public string ManagedPropertyLogicalName { get; set; }
    }
}