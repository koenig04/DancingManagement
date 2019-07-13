using BLL.StatisticManagement.TraineeStatistic;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.Statistic.Trainee
{
    class PresenceSumViewModel : ViewModelBase
    {
        private string _presence;

        public string PresenceCount
        {
            get { return _presence; }
            set
            {
                _presence = value;
                RaisePropertyChanged("PresenceCount");
            }
        }

        private string _leave;

        public string LeaveCount
        {
            get { return _leave; }
            set
            {
                _leave = value;
                RaisePropertyChanged("LeaveCount");
            }
        }

        private string _absence;

        public string AbsenceCount
        {
            get { return _absence; }
            set
            {
                _absence = value;
                RaisePropertyChanged("AbsenceCount");
            }
        }

        private string _giving;

        public string GivingCount
        {
            get { return _giving; }
            set
            {
                _giving = value;
                RaisePropertyChanged("GivingCount");
            }
        }

        private string _total;

        public string TotleCount
        {
            get { return _total; }
            set
            {
                _total = value;
                RaisePropertyChanged("TotleCount");
            }
        }

        public void Clear()
        {
            PresenceCount = "0";
            AbsenceCount = "0";
            LeaveCount = "0";
            GivingCount = "0";
            TotleCount = "0";
        }

        public void FillSum(PresenceInfo info)
        {
            PresenceCount = info.PresenceCount.ToString();
            LeaveCount = info.LeaveCount.ToString();
            AbsenceCount = info.AbsenceCount.ToString();
            GivingCount = info.GivingCount.ToString();
            TotleCount = info.Details.Count.ToString();
        }
    }
}
