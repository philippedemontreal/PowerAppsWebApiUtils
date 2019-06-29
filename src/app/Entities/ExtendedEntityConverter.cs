using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace app.entities
{

    public class ExtendedEntityConverter : JsonConverter
    {
         public override bool CanRead
    {
        get { return false; }
    }

        public override bool CanConvert(Type objectType)
            => objectType.IsSubclassOf(typeof(ExtendedEntity));
        

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);
        //    if (t.Type != JTokenType.Object)
        // {
        //     t.WriteTo(writer);
        // }
        // else
        // {
        //     JObject o = (JObject)t;
        //     IList<string> propertyNames = o.Properties().Select(p => p.Name).ToList();

        //     o.AddFirst(new JProperty("Keys", new JArray(propertyNames)));

        //     o.WriteTo(writer);
        // }
            //throw new NotImplementedException();
        }
    }

    
}

