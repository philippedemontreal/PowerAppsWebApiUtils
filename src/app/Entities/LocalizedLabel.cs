using System;
using System.Collections.Generic;

namespace app.Entities
{
        
    public class LocalizedLabelsDefinition  
    {
        public List<LocalizedLabel> LocalizedLabels { get; set; }
        public LocalizedLabel UserLocalizedLabel { get; set; }
    
    }

    public class LocalizedLabel  
    {
       		
        public string Label { get; set; }
        public int LanguageCode { get; set; }
        public bool IsManaged{ get; set; }
        public Guid MetadataId{ get; set; }
        public bool? HasChanged{ get; set; }
    }
}