using Common;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DancingTrainingManagement.Components.Statistic.CommonComponent.Summary.Item
{
    class FinanceSummaryItemViewModel : SummaryItemViewModel
    {
        private DelegateCommand _clicked;

        public DelegateCommand ItemClicked
        {
            get
            {
                _clicked = _clicked ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        if (_statisticType != StatisticTypeEnum.Diff && _statisticType != StatisticTypeEnum.Capital)
                        {
                            SummaryItemClickedEvent?.Invoke(_statisticType, SummaryItemColor);
                            ChangeSelectState(true);
                        }
                    }));
                return _clicked;
            }
            set
            {
                _clicked = value;
                RaisePropertyChanged("ItemClicked");
            }
        }

        public StatisticTypeEnum Statistic
        {
            get
            {
                return _statisticType;
            }
        }

        public delegate void SummaryItemClicked(StatisticTypeEnum statisticType, Color itemColor);
        public event SummaryItemClicked SummaryItemClickedEvent;

        private StatisticTypeEnum _statisticType;

        public FinanceSummaryItemViewModel(string itemName, decimal itemAmount, StatisticTypeEnum statisticType, Color color)
            : base(itemName, color)
        {

            ItemAmount = itemAmount.ToString();
            _statisticType = statisticType;
        }
    }
}
