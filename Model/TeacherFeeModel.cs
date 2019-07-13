using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class TeacherFeeModel
    {
        public string TeacherFeeID { get; set; }
        public DateTime PaymentDate { get; set; }
        public string TeacherID { get; set; }
        public string GeneralID { get; set; }
        public decimal Amount { get; set; }
        public int FeeYear { get; set; }
        public int FeeMonth { get; set; }
    }
}
