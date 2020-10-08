using BLL.StatisticManagement.TraineeStatistic;
using Common;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.Statistic.Trainee
{
    class PresenceItemViewModel : ViewModelBase
    {
        private string _className;

        public string ClassName
        {
            get { return _className; }
            set
            {
                _className = value;
                RaisePropertyChanged("ClassName");
            }
        }

        private string _date;

        public string ClassDate
        {
            get { return _date; }
            set
            {
                _date = value;
                RaisePropertyChanged("ClassDate");
            }
        }

        private CallingStatisticState _state;

        public CallingStatisticState PresenceState
        {
            get { return _state; }
            set
            {
                _state = value;
                RaisePropertyChanged("PresenceState");
            }
        }

        public void ChangeToOverdue()
        {
            PresenceState = CallingStatisticState.Overdue;
        }

        public PresenceItemViewModel(PresenceDetail detail)
        {
            ClassName = detail.ClassName;
            ClassDate = detail.ClassDate.ToString("yyyy/MM/dd");
            PresenceState = (CallingStatisticState)detail.State;
        }
    }
}
