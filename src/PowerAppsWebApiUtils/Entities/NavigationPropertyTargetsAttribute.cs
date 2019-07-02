using System;

namespace PowerAppsWebApiUtils.Entities
{

    public class NavigationPropertyTargetsAttribute : Attribute
    {
        public string[] Targets { get; private set; }
        public NavigationPropertyTargetsAttribute(params string[] targets)
        {
            Targets = targets;
        }
    }

}

