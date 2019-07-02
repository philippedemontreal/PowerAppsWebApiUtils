using System;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;

namespace PowerAppsWebApiUtils.Json
{

    public class DictionaryContractResolver: DefaultContractResolver
    {
        public static readonly DictionaryContractResolver Instance = new DictionaryContractResolver();

        protected override JsonContract CreateContract(Type objectType)
        {
            if (objectType == typeof(Dictionary<string, object>))
            {
                var contract = base.CreateContract(objectType);
                contract.Converter = new DictionaryConverter();
                return contract;
            }
                

            return base.CreateContract(objectType);
        }

    }

}


