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

        private CallingState _state;

        public CallingState PresenceState
        {
            get { return _state; }
            set
            {
                _state = value;
                RaisePropertyChanged("PresenceState");
            }
        }

        public PresenceItemViewModel(PresenceDetail detail)
        {
            ClassName = detail.ClassName;
            ClassDate = detail.ClassDate.ToString("yyyy年MM月dd日");
            PresenceState = detail.State;
        }
    }
}
