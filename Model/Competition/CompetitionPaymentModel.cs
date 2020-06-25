using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Competition
{
    public class CompetitionPaymentModel
    {
        public string CompetitionPaymentID { get; set; }
        public string CompetitionID { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal CompetitionTotalFee { get; set; }
        public decimal CompetitionIncome { get; set; }
    }
}
