using DancingTrainingManagement.UICore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.NameCallingManagement.CommonComponent
{
    class TraineeForCallingViewModel : ViewModelBase
    {
        private string _traineeID;

        public string TraineeID
        {
            get { return _traineeID; }
            set
            {
                _traineeID = value;
                RaisePropertyChanged("TraineeID");
            }
        }

        private string _traineeName;

        public string TraineeName
        {
            get { return _traineeName; }
            set
            {
                _traineeName = value;
                RaisePropertyChanged("TraineeName");
            }
        }

        private int _callingState;

        public int CallingState
        {
            get { return _callingState; }
            set
            {
                CallingStateChangedEvent?.Invoke(value, _callingState, _trainee.TraineeID);
                _callingState = value;
                RaisePropertyChanged("CallingState");
            }
        }

        private DelegateCommand _present;

        public DelegateCommand Present
        {
            get
            {
                _present = _present ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        CallingState = 0;
                    }));
                return _present;
            }
            set
            {
                _present = value;
                RaisePropertyChanged("Present");
            }
        }

        private DelegateCommand _leave;

        public DelegateCommand Leave
        {
            get
            {
                _leave = _leave ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        CallingState = 1;
                    }));
                return _leave;
            }
            set
            {
                _leave = value;
                RaisePropertyChanged("Leave");
            }
        }

        private DelegateCommand _absent;

        public DelegateCommand Absent
        {
            get
            {
                _absent = _absent ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        CallingState = 2;
                    }));
                return _absent;
            }
            set
            {
                _absent = value;
                RaisePropertyChanged("Absent");
            }
        }

        private DelegateCommand _changeCallingState;

        public DelegateCommand ChangeCallingState
        {
            get
            {
                _changeCallingState = _changeCallingState ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        CallingState = int.Parse(o.ToString());
                    }));
                return _changeCallingState;
            }
            set
            {
                _changeCallingState = value;
                RaisePropertyChanged("ChangeCallingState");
            }
        }


        public delegate void CallingStateChanged(int currentCallingState, int previousCallingState, string traineeID);
        public event CallingStateChanged CallingStateChangedEvent;

        private TraineeModel _trainee;
        public TraineeForCallingViewModel(TraineeModel trainee, int callingState = -1)
        {
            TraineeID = trainee.TraineeIDForShown;
            TraineeName = trainee.TraineeName;
            CallingState = callingState;
            _trainee = trainee;
        }
    }
}
