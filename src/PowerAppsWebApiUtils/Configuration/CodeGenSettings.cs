using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using PowerAppsWebApiUtils.Security;

namespace PowerAppsWebApiUtils.Configuration
{


    public sealed class CodeGenSettings
    {
         public string[] Entities { get; set; }
        public string Namespace { get; set; }
        public string FileName { get; set; }

        public PowerAppsAuthenticationSettings AuthenticationSettings { get; set; }

    }
}
