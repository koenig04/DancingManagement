using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Payment
{
    public class ClassPaymentModel
    {
        public string PaymentID { get; set; }
        /// <summary>
        /// 次数还是时间 0次数 1时间
        /// </summary>
        public int PaymentType { get; set; }
        public DateTime PaymentDate { get; set; }
        public string TraineeID { get; set; }
        public decimal TotalCost { get; set; }
        public int ClassType { get; set; }
        public string ClassID { get; set; }
        public int TotalCount { get; set; }
        public int PaymentTerms { get; set; }
    }
}
