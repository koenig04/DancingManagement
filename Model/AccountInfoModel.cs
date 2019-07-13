using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class AccountInfoModel
    {
        public string AccountID { get; set; }
        public DateTime AccountDate { get; set; }
        public string ItemID { get; set; }
        public decimal AccountAmount { get; set; }
        public string Notice { get; set; }
        public bool IsIncome { get; set; }
    }
}
