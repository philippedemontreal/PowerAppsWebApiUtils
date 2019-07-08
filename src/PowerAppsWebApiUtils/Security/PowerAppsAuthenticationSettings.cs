using System;

namespace PowerAppsWebApiUtils.Security
{
    public sealed class PowerAppsAuthenticationSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string DirectoryId { get; set; }

        public string ResourceUrl { get; set; }
         public string ApiUrl { get; set; }

    }
}
