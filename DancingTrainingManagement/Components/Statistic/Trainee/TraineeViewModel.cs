using BLL.StatisticManagement.TraineeStatistic;
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
    class TraineeViewModel : ConcreteStatisticViewModelBase
    {
        private PresenceStatisticViewModel _presence;

        public PresenceStatisticViewModel Presence
        {
            get { return _presence; }
            set
            {
                _presence = value;
                RaisePropertyChanged("Presence");
            }
        }

        private PresenceSumViewModel _sum;

        public PresenceSumViewModel Sum
        {
            get { return _sum; }
            set
            {
                _sum = value;
                RaisePropertyChanged("Sum");
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

        private TraineeStatisticBussiness _bussiness;
        public TraineeViewModel(TraineeStatisticBussiness bussiness):base()
        {
            _bussiness = bussiness;
            Presence = new PresenceStatisticViewModel(bussiness.Presence);
            Sum = new PresenceSumViewModel();
            PieColletcion = new SeriesCollection();
            Presence.ClearChartEvent += () =>
            {
                PieColletcion.Clear();
                Sum.Clear();
            };
            Presence.PresenceInfoChangedEvent += OnPresenceInfoChanged;
            Presence.PresenceInfoChangedEvent += Sum.FillSum;
            Presence.ErrOccuredEvent += (msg,level) => EnableErrMsg(msg);
        }

        private void OnPresenceInfoChanged(PresenceInfo info)
        {
            if (info.Details.Count > 0)
            {
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
    }
}
