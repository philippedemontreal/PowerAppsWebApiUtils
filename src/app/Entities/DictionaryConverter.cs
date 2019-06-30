using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace app.entities
{

    public class DictionaryConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(Dictionary<string, object>);
        

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            
            var result = new Dictionary<string, object>();
            foreach (JProperty token in JObject.Load(reader).Children())
            {
                var value = token.Value.ToObject<object>();
                if (token.Value.Type == JTokenType.String)
                {
                    Guid identifier;
                    if (Guid.TryParse((string)value, out identifier))
                        value = identifier;
                }

                if (!token.Name.StartsWith("@odata"))
                {
                    var match = Regex.Match(token.Name, @"_(.+)_value(.*)");
                    if (match.Success)
                        result.Add(match.Groups[1].Value + (match.Groups.Count > 2 ? match.Groups[2].Value : string.Empty) , value);
                    else
                        result.Add(token.Name, value);

                }
            }
            return result;
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => throw new NotImplementedException();
        
        
    }

    
}

