using Common;
using DancingTrainingManagement.Components.Statistic.CommonComponent.StatisticHead.FilterItems;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.Statistic.CommonComponent.FilterItems
{
    class TeachingItemsViewModel : ViewModelBase
    {
        private ObservableCollection<TeacherFilterItemViewModel> _teachingItems;

        public ObservableCollection<TeacherFilterItemViewModel> TeachingItems
        {
            get { return _teachingItems; }
            set
            {
                _teachingItems = value;
                RaisePropertyChanged("TeachingItems");
            }
        }

        public delegate void FilterItemsChanged(List<TeachingStatisticTypeEnum> statisticType);
        public event FilterItemsChanged FilterItemsChangedEvent;

        private List<TeachingStatisticTypeEnum> _statistictItems;
        public TeachingItemsViewModel()
        {
            TeachingItems = new ObservableCollection<TeacherFilterItemViewModel>() { new TeacherFilterItemViewModel(TeachingStatisticTypeEnum.ClassCount) };
            foreach (TeacherFilterItemViewModel item in TeachingItems)
            {
                item.FilterItemClickedEvent += OnFilterItemClicked;
            }
            _statistictItems = new List<TeachingStatisticTypeEnum>();
        }

        private void OnFilterItemClicked(TeachingStatisticTypeEnum statisticType, bool isSelected)
        {
            if (isSelected)
            {
                _statistictItems.Add(statisticType);
            }
            else
            {
                _statistictItems.Remove(statisticType);
            }
            FilterItemsChangedEvent?.Invoke(_statistictItems);
        }
    }
}
