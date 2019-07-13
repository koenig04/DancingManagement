using Common;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.Statistic.CommonComponent.StatisticHead.FilterItems
{
    class TeacherFilterItemViewModel : FilterItemViewModel
    {
        private DelegateCommand _itemClicked;

        public DelegateCommand ItemClicked
        {
            get
            {
                _itemClicked = _itemClicked ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        IsSelected = !IsSelected;
                        ChangeSelectionState(IsSelected);
                        FilterItemClickedEvent?.Invoke(_type, IsSelected);
                    }));
                return _itemClicked;
            }
            set
            {
                _itemClicked = value;
                RaisePropertyChanged("ItemClicked");
            }
        }

        public delegate void FilterItemClicked(TeachingStatisticTypeEnum statisticType, bool isSelected);
        public event FilterItemClicked FilterItemClickedEvent;

        private TeachingStatisticTypeEnum _type;

        public TeacherFilterItemViewModel(TeachingStatisticTypeEnum statisticType) : base()
        {
            _type = statisticType;
            ItemName = StatisticTypeManagement.Instance.TeachingStatisticTypeCollection.Where(s => s.StatisticType == statisticType).First().ShownText;
            BorderColor = GlobalVariables.MainColor;
            Init();
        }

        public override void ChangeSelectionState(bool isSelected)
        {
            ItemForeColor = isSelected ? GlobalVariables.MainBackColor : GlobalVariables.MainColor;
            ItemColor = isSelected ? GlobalVariables.MainColor : GlobalVariables.MainBackColor;
            IsSelected = isSelected;
        }
    }
}
