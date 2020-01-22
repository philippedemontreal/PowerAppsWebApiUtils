using System;

namespace PowerAppsWebApiUtils.Entities
{
    public class NavigationPropertyAttribute : Attribute
    {
        public string DataMemberForWrite { get; set; }
        public string RelationSchemaName { get; set; } 
        public bool MultipleTargets { get; set; }       

        public NavigationPropertyAttribute()
        {
        }
    }
}

