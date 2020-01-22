using System;

namespace PowerAppsWebApiUtils.Entities
{

    [AttributeUsage(AttributeTargets.Property,  AllowMultiple = true)]
    public class NavigationPropertyTargetAttribute : Attribute
    {
        public string Target { get;  set; }
        public string CollectionName { get;  set; }
        public NavigationPropertyTargetAttribute()
        {            
        }
        
    }

}

