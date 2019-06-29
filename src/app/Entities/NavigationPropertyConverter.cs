using System;
using Newtonsoft.Json;

namespace app.entities
{

    public class NavigationPropertyConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(NavigationProperty);
        

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var guid = (string)reader.Value;
            if (reader == null)
                return null;

            return new NavigationProperty { Id = Guid.Parse(guid) };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue((value as NavigationProperty).ToString());
            //throw new NotImplementedException();
        }
    }

    
}

