using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace app.entities
{

    public class ExtendedEntityContractResolver: DefaultContractResolver
    {
        public static readonly ExtendedEntityContractResolver Instance = new ExtendedEntityContractResolver();

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (property.DeclaringType.IsSubclassOf(typeof(ExtendedEntity)))
            {
                
                property.ShouldSerialize =
                    instance =>
                    {                    
                        var attributes = (instance as ExtendedEntity).Attributes;
                        if (!attributes.ContainsKey(property.PropertyName))
                            return false;

                        if ((member as PropertyInfo).PropertyType == typeof(NavigationProperty))
                        {
                            var attr = member.GetCustomAttributes(typeof(NavigationPropertyTargetsAttribute), false).FirstOrDefault() as NavigationPropertyTargetsAttribute;
                            if (attr != null)
                            {
                                if (attr.Targets.Length > 1)
                                {
                                    var navigationProperty = (member as PropertyInfo).GetGetMethod().Invoke(instance, null) as NavigationProperty;
                                    property.PropertyName +=  $"_{navigationProperty.EntityLogicalName}";
                                }
                            }

                            property.PropertyName +=  "@odata.bind";


                        }

                        

                        return true;
                    };
            }
            
            return property;
        }
        protected override JsonContract CreateContract(Type objectType)
        {
            if (objectType == typeof(NavigationProperty))
            {
                var contract =base.CreateContract(objectType);
                contract.Converter = new NavigationPropertyConverter();
                return contract;
            }
                

            return base.CreateContract(objectType);
        }

    }

}


