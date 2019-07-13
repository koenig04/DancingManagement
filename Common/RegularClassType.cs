using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common
{
    public class RegularClassType
    {
        public static RegularClassType Instance = new RegularClassType();
        public List<RegularType> RegularClassTypeCollection { get; private set; }

        public RegularClassType()
        {
            if (File.Exists(GlobalVariables.RegularTypePath))
            {
                using (StreamReader sr = new StreamReader(GlobalVariables.RegularTypePath))
                {
                    string regularTypeStr = sr.ReadToEnd();
                    RegularClassTypeCollection = JsonConvert.DeserializeObject<List<RegularType>>(regularTypeStr);
                }
            }

        }
    }

    public class RegularType
    {
        public int Level { get; set; }
        public string Name { get; set; }
    }
}
