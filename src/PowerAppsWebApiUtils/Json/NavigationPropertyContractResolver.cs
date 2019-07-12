using System;
using System.Linq;
using System.Reflection;
using PowerAppsWebApiUtils.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PowerAppsWebApiUtils.Json
{

    public class NavigationPropertyContractResolver: DefaultContractResolver
    {
        //public static readonly NavigationPropertyContractResolver Instance = new NavigationPropertyContractResolver();

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
           
                        if (member.Name == "Id" && property.PropertyType == typeof(Guid))
                            return false;

                        if ((member as PropertyInfo).PropertyType == typeof(NavigationProperty))
                        {
                            var attr = member.GetCustomAttribute<NavigationPropertyTargetsAttribute>();
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
            var contract = base.CreateContract(objectType);

            if (objectType == typeof(NavigationProperty))
            {                
                contract.Converter = new NavigationPropertyConverter();
            }
 

            return contract;
        }

    }

}


