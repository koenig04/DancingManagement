using BLL.ClassManagement;
using BLL.StatisticManagement.TraineeStatistic;
using BLL.TraineeManagement;
using Common;
using DancingTrainingManagement.UICore;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.Statistic.Trainee
{
    class PresenceStatisticViewModel : ViewModelBase
    {
        private ObservableCollection<string> _classes;

        public ObservableCollection<string> ClassCollection
        {
            get { return _classes; }
            set
            {
                _classes = value;
                RaisePropertyChanged("ClassCollection");
            }
        }

        private string _selectedClass;

        public string SelectedClass
        {
            get { return _selectedClass; }
            set
            {
                _selectedClass = value;
                if(!string.IsNullOrEmpty(value))
                _bussiness.GetTrainees(value);
                RaisePropertyChanged("SelectedClass");
            }
        }

        private string _selectedTrainee;

        public string SelectTrainee
        {
            get { return _selectedTrainee; }
            set
            {
                _selectedTrainee = value;
                RaisePropertyChanged("SelectTrainee");
            }
        }

        private ObservableCollection<string> _trainees;

        public ObservableCollection<string> TraineeCollection
        {
            get { return _trainees; }
            set
            {
                _trainees = value;
                RaisePropertyChanged("TraineeCollection");
            }
        }


        private int _termState;

        public int TermState
        {
            get { return _termState; }
            set
            {
                _termState = value;
                RaisePropertyChanged("TermState");
            }
        }

        private DelegateCommand _changeTermState;

        public DelegateCommand ChangeTermState
        {
            get
            {
                _changeTermState = _changeTermState ?? new DelegateCommand(new Action<object>(
                   o =>
                   {
                       TermState = int.Parse(o.ToString());
                   }));
                return _changeTermState;
            }
            set
            {
                _changeTermState = value;
                RaisePropertyChanged("ChangeTermState");
            }
        }

        private ObservableCollection<PresenceItemViewModel> _presences;

        public ObservableCollection<PresenceItemViewModel> PresenceCollection
        {
            get { return _presences; }
            set
            {
                _presences = value;
                RaisePropertyChanged("PresenceCollection");
            }
        }


        private DelegateCommand _search;

        public DelegateCommand Search
        {
            get
            {
                _search = _search ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        if (CheckValidity())
                        {
                            ClearChartEvent?.Invoke();
                            PresenceCollection.Clear();
                            _bussiness.GetRegularPresence(SelectTrainee, TermState == 0 ? true : false);
                        }
                    }));
                return _search;
            }
            set
            {
                _search = value;
                RaisePropertyChanged("Search");
            }
        }

        public delegate void ErrOccured(MessageType msg, MessageLevel level = MessageLevel.Warning);
        public event ErrOccured ErrOccuredEvent;
        public delegate void ClearChart();
        public event ClearChart ClearChartEvent;
        public delegate void PresenceInfoChanged(PresenceInfo info);
        public event PresenceInfoChanged PresenceInfoChangedEvent;

        private PresenceBussiness _bussiness;
        public PresenceStatisticViewModel(PresenceBussiness bussiness)
        {
            _bussiness = bussiness;
            TermState = 0;
            PresenceCollection = new ObservableCollection<PresenceItemViewModel>();

            ClassCollection = new ObservableCollection<string>();
            foreach (RegularClassModel item in _bussiness.GetRegularClasses())
            {
                ClassCollection.Add(item.ClassName);
            }
            TraineeCollection = new ObservableCollection<string>();

            _bussiness.RegularClassChangedEvent += () =>
            {
                ClassCollection.Clear();
                if (_bussiness.GetRegularClasses() != null)
                    foreach (RegularClassModel item in _bussiness.GetRegularClasses())
                    {
                        ClassCollection.Add(item.ClassName);
                    }
                SelectedClass = string.Empty;
                TraineeCollection.Clear();
                SelectTrainee = string.Empty;
            };

            _bussiness.TraineeChangedEvent += trainees =>
            {
                SelectTrainee = string.Empty;
                TraineeCollection.Clear();
                if (trainees != null)
                    foreach (TraineeModel item in trainees)
                    {
                        TraineeCollection.Add(item.TraineeName);
                    }
            };

            _bussiness.PresenceInfoChangedEvent += OnPresenceInfoChanged;
        }

        private void OnPresenceInfoChanged(PresenceInfo info)
        {
            if (info.Details != null)
            {
                foreach (PresenceDetail item in info.Details)
                {
                    PresenceCollection.Add(new PresenceItemViewModel(item));
                }
                PresenceInfoChangedEvent?.Invoke(info);
            }
        }

        private bool CheckValidity()
        {
            if (string.IsNullOrEmpty(SelectedClass))
            {
                return EnableErrMsg(MessageType.TraineeStatisticClassErr);
            }
            if(string.IsNullOrEmpty(SelectTrainee))
            {
                return EnableErrMsg(MessageType.TraineeStatisticTraineeErr);
            }
            return true;
        }

        private bool EnableErrMsg(MessageType msg)
        {
            ErrOccuredEvent?.Invoke(msg);
            return false;
        }
    }
}
