using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ItemInfoModel
    {
        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public bool IsIncome { get; set; }
        public string IconName { get; set; }
        public string IconNamePressed { get; set; }
    }
}
