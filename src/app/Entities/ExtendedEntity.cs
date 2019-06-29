using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Microsoft.Dynamics.CRM;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace app.entities
{

    public interface IExtendedEntity
    {
        Dictionary<string, object> Attributes { get; set; }
    }  
 
    public class ExtendedEntity: IExtendedEntity, IExtensibleDataObject
    {
            
 
        [JsonIgnore]
        public Dictionary<string, object> Attributes { get; set; }
        public ExtensionDataObject ExtensionData { get ; set; }

        public ExtendedEntity()
        {
            Attributes = new Dictionary<string, object>();
        }

        protected T GetAttributeValue<T>(string key) 
        {
                        
            var typeT = typeof(T);
                        
            if (typeT == typeof(NavigationProperty))
            {
                key = $"{key}";
                if (!Attributes.ContainsKey(key))
                    return default(T);

                return (T)((object)new NavigationProperty 
                { 
                    Id = ((Guid)Attributes[key]),
                    Name = (string)Attributes[$"{key}@OData.Community.Display.V1.FormattedValue"],
                    EntityLogicalName = (string)Attributes[$"{key}@Microsoft.Dynamics.CRM.lookuplogicalname"],
                });
            }

            if (!Attributes.ContainsKey(key) || Attributes[key] == null)
                return default(T);

            var value = Attributes[key];
            if (typeT == typeof(string))
                return (T)((object)Attributes[key]);

            if (typeT == typeof(Guid) || typeT == typeof(Guid?))
                return (T)((object)Guid.Parse((string)Attributes[key]));

            if (typeT == typeof(int)|| typeT == typeof(int?))
                return (T)((object)Convert.ToInt32(Attributes[key]));

            if (typeT == typeof(long)|| typeT == typeof(long?))
                return (T)((object)Convert.ToInt64(Attributes[key]));

            if (typeT == typeof(bool)|| typeT == typeof(bool?))
                return (T)((object)Convert.ToBoolean(Attributes[key]));

            if (typeT == typeof(decimal)|| typeT == typeof(decimal?))
                return (T)((object)Convert.ToDecimal(Attributes[key]));

            if (typeT == typeof(double)|| typeT == typeof(double?))
                return (T)((object)Convert.ToDouble(Attributes[key]));

            if (typeT == typeof(DateTime)|| typeT == typeof(DateTime?))
                return (T)((object)Convert.ToDateTime(Attributes[key]));

            if (typeT.BaseType == typeof(ValueType) && typeT.GenericTypeArguments.Length == 1)
                return (T)(Enum.ToObject(typeT.GenericTypeArguments[0], (Int64)Attributes[key]));



            return  default(T);
        } 

        protected void SetAttributeValue<T>(string key, T value) 
        {
                        
            if (typeof(T) == typeof(NavigationProperty))
            {
                var navigationProperty = value as NavigationProperty;
                SetAttributeValue<Guid>($"{key}", navigationProperty.Id);
                SetAttributeValue<string>($"{key}@OData.Community.Display.V1.FormattedValue", navigationProperty.Name);
                SetAttributeValue<string>($"{key}@Microsoft.Dynamics.CRM.lookuplogicalname", navigationProperty.EntityLogicalName);
                return;
            } 


            if (!Attributes.ContainsKey(key))
                Attributes.Add(key, value);
            else
                Attributes[key] = value;
        }
    }

}


