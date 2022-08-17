using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.Statistic.Class.Attendence
{
    class GeneralClassSwitcherViewModel : ViewModelBase
    {
        private int state_;

        public int AttendeceState
        {
            get { return state_; }
            set
            {
                state_ = value;
                RaisePropertyChanged("AttendeceState");
            }
        }

        private DelegateCommand changeAttendence;

        public DelegateCommand ChangeAttendenceState
        {
            get
            {
                changeAttendence = changeAttendence ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        AttendeceState = int.Parse(o.ToString());
                        AttendenceTypeChangedEvent?.Invoke(AttendeceState);
                    }));
                return changeAttendence;
            }
            set
            {
                changeAttendence = value;
                RaisePropertyChanged("ChangeAttendenceState");
            }
        }

        public delegate void AttendenceTypeChanged(int state);
        public event AttendenceTypeChanged AttendenceTypeChangedEvent;

        public GeneralClassSwitcherViewModel()
        {
            AttendeceState = 1;
        }
    }
}
