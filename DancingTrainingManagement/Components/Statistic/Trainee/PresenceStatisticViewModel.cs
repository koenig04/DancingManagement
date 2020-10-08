using BLL.ClassManagement;
using BLL.StatisticManagement.TraineeStatistic;
using BLL.TraineeManagement;
using Common;
using DancingTrainingManagement.UICore;
using LiveCharts;
using LiveCharts.Wpf;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

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
                if (!string.IsNullOrEmpty(value))
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

        private SeriesCollection _seriesCollection;

        public SeriesCollection PieColletcion
        {
            get { return _seriesCollection; }
            set
            {
                _seriesCollection = value;
                RaisePropertyChanged("PieColletcion");
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
            PieColletcion = new SeriesCollection();
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
            PieColletcion.Clear();

            if (info.Details.Count > 0)
            {
                if (info.PresenceCount > 0)
                    PieColletcion.Add(new PieSeries()
                    {
                        Title = "出勤",
                        Fill = new SolidColorBrush(GlobalVariables.IncomeColor),
                        DataLabels = true,
                        Values = new ChartValues<int> { info.PresenceCount },
                        LabelPoint = new Func<ChartPoint, string>(chartPoint => string.Format("{0} ({1:P})", "出勤", chartPoint.Participation)),
                        LabelPosition = PieLabelPosition.OutsideSlice,
                        Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)),
                        FontWeight = FontWeight.FromOpenTypeWeight(800),
                        FontSize = 14
                    });
                if (info.LeaveCount > 0)
                    PieColletcion.Add(new PieSeries()
                    {
                        Title = "请假",
                        Fill = new SolidColorBrush(GlobalVariables.SecondaryColor),
                        DataLabels = true,
                        Values = new ChartValues<int> { info.LeaveCount },
                        LabelPoint = new Func<ChartPoint, string>(chartPoint => string.Format("{0} ({1:P})", "请假", chartPoint.Participation)),
                        LabelPosition = PieLabelPosition.OutsideSlice,
                        Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)),
                        FontWeight = FontWeight.FromOpenTypeWeight(800),
                        FontSize = 14
                    });
                if (info.AbsenceCount > 0)
                    PieColletcion.Add(new PieSeries()
                    {
                        Title = "旷课",
                        Fill = new SolidColorBrush(GlobalVariables.ExpenseColor),
                        DataLabels = true,
                        Values = new ChartValues<int> { info.AbsenceCount },
                        LabelPoint = new Func<ChartPoint, string>(chartPoint => string.Format("{0} ({1:P})", "旷课", chartPoint.Participation)),
                        LabelPosition = PieLabelPosition.OutsideSlice,
                        Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)),
                        FontWeight = FontWeight.FromOpenTypeWeight(800),
                        FontSize = 14
                    });
                if (info.GivingCount > 0)
                    PieColletcion.Add(new PieSeries()
                    {
                        Title = "送课",
                        Fill = new SolidColorBrush(GlobalVariables.LightMainColor),
                        DataLabels = true,
                        Values = new ChartValues<int> { info.GivingCount },
                        LabelPoint = new Func<ChartPoint, string>(chartPoint => string.Format("{0} ({1:P})", "送课", chartPoint.Participation)),
                        LabelPosition = PieLabelPosition.OutsideSlice,
                        Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)),
                        FontWeight = FontWeight.FromOpenTypeWeight(800),
                        FontSize = 14
                    });
            }
        }

        private bool CheckValidity()
        {
            if (string.IsNullOrEmpty(SelectedClass))
            {
                return EnableErrMsg(MessageType.TraineeStatisticClassErr);
            }
            if (string.IsNullOrEmpty(SelectTrainee))
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
