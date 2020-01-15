using System;

namespace PowerAppsWebApiUtils.Entities
{

    public class NavigationPropertyAttribute : Attribute
    {
        public string SchemaName { get; set; }
        public NavigationPropertyAttribute()
        {
        }
    }

}

