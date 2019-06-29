using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Microsoft.Dynamics.CRM;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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

