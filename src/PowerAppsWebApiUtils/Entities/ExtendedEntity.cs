using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Dynamics.CRM;

namespace PowerAppsWebApiUtils.Entities
{
    public class ExtendedEntity: crmbaseentity 
    {
        public ExtendedEntity()
            => Attributes = new Dictionary<string, object>();

        public ExtendedEntity(Guid id)
        : this()
            =>  Id = id;
    
        [IgnoreDataMember]
        public Dictionary<string, object> Attributes { get; set; }
   
        public NavigationProperty ToNavigationProperty()
            => string.IsNullOrEmpty(EntityCollectionName) ? 
                throw new InvalidOperationException("EntityCollectionName is not set") : 
                new NavigationProperty { Id = Id, EntityLogicalName = EntityLogicalName, LogicalCollectionName = EntityCollectionName };

        protected NavigationProperty GetNavigationAttribute(string key) 
        {
                if (!Attributes.ContainsKey(key))
                    return null;
                
                var id = Attributes[key]; 
                if (id == null)
                    return null;

                return new NavigationProperty 
                { 
                    Id = (Guid)Attributes[key],
                    Name = Attributes.ContainsKey($"{key}@OData.Community.Display.V1.FormattedValue") ? (string)Attributes[$"{key}@OData.Community.Display.V1.FormattedValue"] : null,
                    EntityLogicalName = Attributes.ContainsKey($"{key}@Microsoft.Dynamics.CRM.lookuplogicalname") ? (string)Attributes[$"{key}@Microsoft.Dynamics.CRM.lookuplogicalname"] : null,
                    LogicalCollectionName= Attributes.ContainsKey($"{key}/LogicalCollectionName") ? (string)Attributes[$"{key}/LogicalCollectionName"] : null,
                };
        }

        protected void SetNavigationAttribute(string key, NavigationProperty result) 
        {
                        
            var navigationProperty = result as NavigationProperty;
            
            SetAttributeValue<Guid?>($"{key}", navigationProperty?.Id);
            SetAttributeValue<string>($"{key}@OData.Community.Display.V1.FormattedValue", navigationProperty?.Name);
            SetAttributeValue<string>($"{key}@Microsoft.Dynamics.CRM.lookuplogicalname", navigationProperty?.EntityLogicalName);
            SetAttributeValue<string>($"{key}/LogicalCollectionName", navigationProperty?.LogicalCollectionName);
            return;
        }

        protected T GetAttributeValue<T>(string key) 
        {
            if (!Attributes.ContainsKey(key) || Attributes[key] == null)
                return default(T);
                        
            var typeOfKey = typeof(T);
            var result = Attributes[key];

            if (result.GetType() == typeOfKey || (typeOfKey.IsGenericType &&  typeOfKey.GenericTypeArguments[0] == result.GetType()))
                return (T)result;

            if (typeOfKey == typeof(string))
                return (T)((object)Convert.ToString(result));

            if (typeOfKey == typeof(int)|| typeOfKey == typeof(int?))
                return (T)((object)Convert.ToInt32(result));

            if (typeOfKey == typeof(long)|| typeOfKey == typeof(long?))
                return (T)((object)Convert.ToInt64(result));

            if (typeOfKey == typeof(bool)|| typeOfKey == typeof(bool?))
                return (T)((object)Convert.ToBoolean(result));

            if (typeOfKey == typeof(decimal)|| typeOfKey == typeof(decimal?))
                return (T)((object)Convert.ToDecimal(result));

            if (typeOfKey == typeof(double)|| typeOfKey == typeof(double?))
                return (T)((object)Convert.ToDouble(result));

            if (typeOfKey == typeof(DateTime)|| typeOfKey == typeof(DateTime?))
                return (T)((object)Convert.ToDateTime(result));

            if (typeOfKey.BaseType != null && typeOfKey.BaseType == typeof(ValueType) && typeOfKey.GenericTypeArguments.Length == 1)
                return (T)(Enum.ToObject(typeOfKey.GenericTypeArguments[0], result));

            return  default(T);
        } 

        protected void SetAttributeValue<T>(string key, T value) 
        {                       
            if (!Attributes.ContainsKey(key))
                Attributes.Add(key, value);
            else
                Attributes[key] = value;
        }
    }

}


