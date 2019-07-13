using Common;
using DancingTrainingManagement.Components.Statistic.CommonComponent.Summary.Item;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DancingTrainingManagement.Components.Statistic.CommonComponent.Summary.ItemGroup
{
    class FinanceSummaryGroupViewModel : SummaryItemGroupViewModel
    {
        private ObservableCollection<FinanceSummaryItemViewModel> _items;

        public ObservableCollection<FinanceSummaryItemViewModel> SummaryItems
        {
            get { return _items; }
            set
            {
                _items = value;
                RaisePropertyChanged("SummaryItems");
            }
        }

        public delegate void SummaryItemClicked(StatisticTypeEnum statisticType, DateTime summaryDate, bool isTotal, Color itemColor);
        public event SummaryItemClicked SummaryItemClickedEvent;

        public FinanceSummaryGroupViewModel(DateTime summaryDate, bool isSortedByMonth) : base(summaryDate, isSortedByMonth)
        {
            SummaryItems = new ObservableCollection<FinanceSummaryItemViewModel>();
        }

        public FinanceSummaryGroupViewModel() : base()
        {
            SummaryItems = new ObservableCollection<FinanceSummaryItemViewModel>();
        }

        public void AddSummaryItem(string itemName, decimal itemAmount, StatisticTypeEnum statisticType, Color color)
        {
            SummaryItems.Add(new FinanceSummaryItemViewModel(itemName, itemAmount, statisticType, color));
            SummaryItems.Last().SummaryItemClickedEvent += (statistic, itemColor) =>
            {
                foreach (FinanceSummaryItemViewModel item in SummaryItems.Where(i => i.Statistic != statistic))
                {
                    item.ChangeSelectState(false);
                }
                SummaryItemClickedEvent?.Invoke(statistic, SummaryDate, IsTotal, itemColor);
            };
            ChangeGroupWidth(SummaryItems.Count);
        }

        public void ChangeGroupState(bool state)
        {
            foreach (FinanceSummaryItemViewModel item in SummaryItems)
            {
                item.ChangeSelectState(state);
            }
        }

        public override void ClearSummary()
        {
            SummaryItems.Clear();
        }
    }
}
