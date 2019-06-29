using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace app.entities
{

    public class DictionaryContractResolver: DefaultContractResolver
    {
        public static readonly DictionaryContractResolver Instance = new DictionaryContractResolver();

        protected override JsonContract CreateContract(Type objectType)
        {
            if (objectType == typeof(Dictionary<string, object>))
            {
                var contract =base.CreateContract(objectType);
                contract.Converter = new DictionaryConverter();
                return contract;
            }
                

            return base.CreateContract(objectType);
        }

    }

}


