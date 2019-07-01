using System;
using System.Linq;
using System.Reflection;
using Microsoft.Dynamics.CRM;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace app.entities
{

    public class ExtendedEntityContractResolver: NavigationPropertyContractResolver
    {
        public static new readonly ExtendedEntityContractResolver Instance = new ExtendedEntityContractResolver();


        protected override JsonContract CreateContract(Type objectType)
        {
            var contract = base.CreateContract(objectType);

            if (objectType.IsSubclassOf(typeof(ExtendedEntity)))
            {
                contract.Converter = new ExtendedEntityConverter();
            }

            if (objectType == typeof(AttributeMetadata))
            {
                contract.Converter = new AttributeMetadataConverter();
            }
            
                

            return contract;
        }

    }

}


