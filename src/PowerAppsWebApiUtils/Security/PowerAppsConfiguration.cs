using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PowerAppsWebApiUtils.Security
{


    public sealed class PowerAppsConfiguration
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string DirectoryId { get; set; }

        public string ResourceUrl { get; set; }
 
        public string ApiUrl { get; set; }
    }
}
