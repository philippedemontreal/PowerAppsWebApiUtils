using System;
using PowerAppsWebApiUtils.Configuration;
using PowerAppsWebApiUtils.Processes;

namespace PowerappsWebApiUtils
{
    class Program   
    {

     static void Main(string[] args)
        {
            var config =  PowerAppsConfigurationReader.GetConfiguration();
            new CodeGenProcess().Execute(config);
                        
         }  
    }
}
