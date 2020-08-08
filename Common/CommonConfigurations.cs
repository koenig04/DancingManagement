using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common
{
    public class CommonConfigurations
    {
        public static CommonConfigurations Instance = new CommonConfigurations();
        public CommonConfigurationItems Items { get; set; }

        public CommonConfigurations()
        {
            if (File.Exists(GlobalVariables.CommonConfigurationsPath))
            {
                using (StreamReader sr = new StreamReader(GlobalVariables.CommonConfigurationsPath))
                {
                    string commonStr = sr.ReadToEnd();
                    Items = JsonConvert.DeserializeObject<CommonConfigurationItems>(commonStr);
                }
            }
        }
    }

    public class CommonConfigurationItems
    {
        public string ConnectionString { get; set; }
    }
}
