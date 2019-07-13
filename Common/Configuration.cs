using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common
{
    public class Configuration
    {
        public static Configuration Instance = new Configuration();
        public ConfigurationItems Configurations { get; private set; }

        public Configuration()
        {
            if (File.Exists(GlobalVariables.ConfigurationPath))
            {
                using (StreamReader sr = new StreamReader(GlobalVariables.ConfigurationPath))
                {
                    string regularTypeStr = sr.ReadToEnd();
                    Configurations = JsonConvert.DeserializeObject<ConfigurationItems>(regularTypeStr);
                }
            }
        }
    }

    public class ConfigurationItems
    {
        public int CountPerTerm { get; set; }
    }
}
