using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class OverdueModel
    {
        public string OverdueID { get; set; }
        public string TraineeID { get; set; }
        public string TraineeName { get; set; }
        public int ClassType { get; set; }
        public string ClassID { get; set; }
        public string ClassName { get; set; }
        public DateTime OverdueDate { get; set; }
        public decimal RenewAmount { get; set; }
    }
}
