using System;
using System.Collections.Generic;
using Microsoft.Dynamics.CRM;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace app.Json
{

    public class AttributeMetadataConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(AttributeMetadata);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {    
            var attr = JObject.Load(reader);
            AttributeTypeCode code;

            if (attr.ContainsKey("AttributeType") && Enum.TryParse<AttributeTypeCode>((string)attr["AttributeType"], out code))
            {
                switch (code)
                {
                    case AttributeTypeCode.Lookup:
                    case AttributeTypeCode.Owner:
                    case AttributeTypeCode.Customer:
                        return attr.ToObject<LookupAttributeMetadata>();
                    
                    default:
                        return attr.ToObject<AttributeMetadata>();
                }
            }

            return attr.ToObject<AttributeMetadata>();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => throw new NotImplementedException();      
        
    }

    
}

