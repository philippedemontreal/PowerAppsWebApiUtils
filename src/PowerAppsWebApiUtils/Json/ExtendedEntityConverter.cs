using System;
using System.Collections.Generic;
using PowerAppsWebApiUtils.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PowerAppsWebApiUtils.Json
{

    public class ExtendedEntityConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => objectType.IsSubclassOf(typeof(ExtendedEntity));
        

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {            
            var result = Activator.CreateInstance(objectType) as ExtendedEntity;
            result.Attributes = JObject.Load(reader).ToObject<Dictionary<string, object>>(new JsonSerializer { ContractResolver = DictionaryContractResolver.Instance });           
            return result;
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => throw new NotImplementedException();
        
        
    }

    
}

