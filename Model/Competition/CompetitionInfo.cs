using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Competition
{
    public class CompetitionInfoModel
    {
        public string CompetitionID { get; set; }
        public string CompetitionName { get; set; }
        public int CompetitionYear { get; set; }
        public int CompetitionTraineeCount { get; set; }
        public decimal CompetitionFee { get; set; }
        public decimal CompetitionActualFee { get; set; }
        public bool IsRecorded { get; set; }
    }
}
