using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Xml;

namespace app.Entities
{
    public class RootObject<T>
    {
        [JsonProperty("@odata.context")]
        public string Context { get; set; }
        [JsonProperty("@Microsoft.Dynamics.CRM.fetchxmlpagingcookie")]
        public string FetchXmlPagingCookie { get; set; }
        [JsonProperty("istracking")]
        public bool IsTracking { get; set; }
        [JsonProperty("value")]
        public List<T> Value { get; set; }

        public void GetPagingInfo(out string page, out string cookie)
        {
            page = null;
            cookie = null;
            if (string.IsNullOrEmpty(FetchXmlPagingCookie))
                return;

            var xml = new XmlDocument();
            xml.LoadXml(FetchXmlPagingCookie);
            var node = xml.FirstChild;
            cookie = node.Attributes["pagingcookie"].Value;
            page = node.Attributes["pagenumber"].Value;
        }
    }

}
