using System;

namespace app.entities
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

