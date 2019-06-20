using System;
using System.Collections.Generic;

namespace app.entities
{
      
    public class ExtendedEntity: Dictionary<string, object>
    {
        protected T GetAttributeValue<T>(string key) 
        {
            if (!ContainsKey(key) || this[key] == null)
                return default(T);

            var typeT = typeof(T); 
            var value = this[key];

            if (typeT == typeof(Guid) || typeT == typeof(Guid?))
                return (T)((object)Guid.Parse((string)this[key]));

            if (typeT == typeof(int)|| typeT == typeof(int?))
                return (T)((object)Convert.ToInt32(this[key]));

            if (typeT == typeof(decimal)|| typeT == typeof(decimal?))
                return (T)((object)Convert.ToDecimal(this[key]));

            if (typeT == typeof(DateTime)|| typeT == typeof(DateTime?))
                return (T)((object)Convert.ToDateTime(this[key]));

            return  default(T);
        } 

        protected void SetAttributeValue<T>(string key, T value) 
        {
            if (!ContainsKey(key))
                Add(key, value);
            else
                this[key] = value;
        }
    }

}


