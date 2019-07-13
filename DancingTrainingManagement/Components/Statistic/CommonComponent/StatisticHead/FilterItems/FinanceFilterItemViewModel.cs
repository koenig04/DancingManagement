using Common;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.Statistic.CommonComponent.StatisticHead.FilterItems
{
    class FinanceFilterItemViewModel : FilterItemViewModel
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
                        FilterItemClickedEvent?.Invoke(_model, IsSelected);
                    }));
                return _itemClicked;
            }
            set
            {
                _itemClicked = value;
                RaisePropertyChanged("ItemClicked");
            }
        }

        public delegate void FilterItemClicked(StatisticTypeModel statisticType, bool isSelected);
        public event FilterItemClicked FilterItemClickedEvent;

        private StatisticTypeModel _model;
        public FinanceFilterItemViewModel(StatisticTypeEnum statisticType)
        {
            _model = StatisticTypeManagement.Instance.StatisticTypeCollection.Where(s => s.StatisticType == statisticType).First();
            ItemName = _model.ShownText;
            switch (_model.Catogery)
            {
                case StatisticCategoryEnum.Income:
                    BorderColor = GlobalVariables.IncomeColor;
                    break;
                case StatisticCategoryEnum.Outcome:
                    BorderColor = GlobalVariables.ExpenseColor;
                    break;
            }
            if(statisticType== StatisticTypeEnum.Diff)
            {
                BorderColor = GlobalVariables.SecondaryColor;
            }
            if (statisticType == StatisticTypeEnum.Capital)
            {
                BorderColor = GlobalVariables.MainColor;
            }
            Init();
        }

        public override void ChangeSelectionState(bool isSelected)
        {
            IsSelected = isSelected;
            switch (_model.Catogery)
            {
                case StatisticCategoryEnum.Income:
                    ItemForeColor = isSelected ? GlobalVariables.MainBackColor : GlobalVariables.IncomeColor;
                    ItemColor = isSelected ? GlobalVariables.IncomeColor : GlobalVariables.MainBackColor;
                    break;
                case StatisticCategoryEnum.Outcome:
                    ItemForeColor = isSelected ? GlobalVariables.MainBackColor : GlobalVariables.ExpenseColor;
                    ItemColor = isSelected ? GlobalVariables.ExpenseColor : GlobalVariables.MainBackColor;
                    break;
            }
            if (_model.StatisticType == StatisticTypeEnum.Diff)
            {
                ItemForeColor = isSelected ? GlobalVariables.MainBackColor : GlobalVariables.SecondaryColor;
                ItemColor = isSelected ? GlobalVariables.SecondaryColor : GlobalVariables.MainBackColor;
            }
            if (_model.StatisticType == StatisticTypeEnum.Capital)
            {
                ItemForeColor = isSelected ? GlobalVariables.MainBackColor : GlobalVariables.MainColor;
                ItemColor = isSelected ? GlobalVariables.MainColor : GlobalVariables.MainBackColor;
            }
        }
    }
}
