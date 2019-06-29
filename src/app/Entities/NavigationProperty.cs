using System;
using Newtonsoft.Json;

namespace app.entities
{

    public class NavigationProperty
    {
        public string  EntityLogicalName { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
            => $"/{EntityLogicalName}({Id.ToString().ToUpper()})";
    }
}

