using System;
using app.entities;
using Newtonsoft.Json;

namespace app.Json
{

    public class NavigationPropertyConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(NavigationProperty);
        

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            => throw new NotImplementedException();


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => writer.WriteValue((value as NavigationProperty).ToString());
        
        
    }

    
}

