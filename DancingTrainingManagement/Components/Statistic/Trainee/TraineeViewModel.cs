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

        private ObservableCollection<PresenceItemViewModel> _itemCollection;

        public ObservableCollection<PresenceItemViewModel> ItemCollection
        {
            get { return _itemCollection; }
            set
            {
                _itemCollection = value;
                RaisePropertyChanged("ItemCollection");
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
        public TraineeViewModel(TraineeStatisticBussiness bussiness) : base()
        {
            _bussiness = bussiness;
            ItemCollection = new ObservableCollection<PresenceItemViewModel>();
            Presence = new PresenceStatisticViewModel(bussiness.Presence);
            Sum = new PresenceSumViewModel();
            PieColletcion = new SeriesCollection();
            Presence.ClearChartEvent += () =>
            {
                Sum.Clear();
            };

            Presence.ErrOccuredEvent += (msg, level) => EnableErrMsg(msg);

            _bussiness.Presence.PresenceInfoChangedEvent += OnPresenceInfoChanged;
        }

        private void OnPresenceInfoChanged(PresenceInfo info)
        {
            ItemCollection.Clear();

            int presenceCount = 0;
            if (info.Details != null)
            {
                foreach (PresenceDetail item in info.Details)
                {
                    if (item.State == CallingState.Presence) presenceCount++;
                    ItemCollection.Add(new PresenceItemViewModel(item));
                    if(item.State == CallingState.Presence && presenceCount > Configuration.Instance.Configurations.CountPerTerm)
                    {
                        ItemCollection.Last().ChangeToOverdue();
                    }
                }
            }

            Sum.FillSum(info);
        }
    }
}
