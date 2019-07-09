using Newtonsoft.Json;
using System.Collections.Generic;
using System;


namespace PowerAppsWebApiUtils.Entities
{
    public class RootObject<T>
    {
        [JsonProperty("@odata.context")]
        public string Context { get; set; }
        [JsonProperty("@odata.nextLink")]
        public string NextLink { get; set; }
        [JsonProperty("value")]
        public List<T> Value { get; set; }

        public void GetPagingInfo(out string page, out string cookie)
        {            
            page = null;
            cookie = null;
            throw new NotImplementedException();
        }
    }

}
