using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common
{
    public class BlockClassType
    {
        public static BlockClassType Instance = new BlockClassType();
        public List<BlockType> BlockClassTypeCollection { get; private set; }
        public List<BlockType> BlockClassSeasonColletion { get; private set; }

        public BlockClassType()
        {
            if (File.Exists(GlobalVariables.BlockTypePath))
            {
                using (StreamReader sr = new StreamReader(GlobalVariables.BlockTypePath))
                {
                    string blockTypeStr = sr.ReadToEnd();
                    BlockClassTypeCollection = JsonConvert.DeserializeObject<List<BlockType>>(blockTypeStr);
                }
            }

            if (File.Exists(GlobalVariables.BlockSeasonPath))
            {
                using (StreamReader sr = new StreamReader(GlobalVariables.BlockSeasonPath))
                {
                    string blockSeasonStr = sr.ReadToEnd();
                    BlockClassSeasonColletion = JsonConvert.DeserializeObject<List<BlockType>>(blockSeasonStr);
                }
            }

        }
    }

    public class BlockType
    {
        public int Type { get; set; }
        public string Name { get; set; }
    }
}
