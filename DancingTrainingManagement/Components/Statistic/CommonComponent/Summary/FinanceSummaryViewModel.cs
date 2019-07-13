using BLL.StatisticManagement.FinanceStatistic;
using Common;
using DancingTrainingManagement.Components.Statistic.CommonComponent.Summary.Details;
using DancingTrainingManagement.Components.Statistic.CommonComponent.Summary.ItemGroup;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DancingTrainingManagement.Components.Statistic.CommonComponent.Summary
{
    class FinanceSummaryViewModel : ViewModelBase
    {
        private FinanceRecordDetailsViewModel _details;

        public FinanceRecordDetailsViewModel StatisticDetails
        {
            get { return _details; }
            set
            {
                _details = value;
                RaisePropertyChanged("StatisticDetails");
            }
        }

        private bool _isDetailEnable;

        public bool IsDetailListEnable
        {
            get { return _isDetailEnable; }
            set
            {
                _isDetailEnable = value;
                RaisePropertyChanged("IsDetailListEnable");
            }
        }

        private ObservableCollection<FinanceSummaryGroupViewModel> _summaryCollection;

        public ObservableCollection<FinanceSummaryGroupViewModel> SummaryGroupCollection
        {
            get { return _summaryCollection; }
            set
            {
                _summaryCollection = value;
                RaisePropertyChanged("SummaryGroupCollection");
            }
        }

        private FinanceSummaryGroupViewModel _total;

        public FinanceSummaryGroupViewModel TotalSummary
        {
            get { return _total; }
            set
            {
                _total = value;
                RaisePropertyChanged("TotalSummary");
            }
        }

        private int _popWidth;

        public int PopWidth
        {
            get { return _popWidth; }
            set
            {
                _popWidth = value;
                RaisePropertyChanged("PopWidth");
            }
        }


        protected Color _currentItemColor;

        public delegate void DeleteAccountRecord(StatisticTypeEnum statisticType, string accountID, string accountDate, string accountItem, string accountAmount, string note);
        public event DeleteAccountRecord DeleteRecordEvent;

        private FinanceStatisticBussiness _bussiness;
        public FinanceSummaryViewModel(FinanceStatisticBussiness bussiness)
        {
            SummaryGroupCollection = new ObservableCollection<FinanceSummaryGroupViewModel>();
            TotalSummary = new FinanceSummaryGroupViewModel();
            TotalSummary.SummaryItemClickedEvent += OnSummaryClicked;
            StatisticDetails = new FinanceRecordDetailsViewModel(bussiness);
            StatisticDetails.DeleteRecordEvent += (statisticType, accountID, accountDate, accountItem, accountAmount, note) =>
            {
                IsDetailListEnable = false;
                DeleteRecordEvent?.Invoke(statisticType, accountID, accountDate, accountItem, accountAmount, note);
            };
            IsDetailListEnable = false;
            _bussiness = bussiness;
            _bussiness.ShowDetailsEvent += (normal, teacherFee, classPayment) =>
            {
                StatisticDetails.Enable(normal, teacherFee, classPayment, _currentItemColor);
                IsDetailListEnable = true;
            };
            PopWidth = 400;
        }

        public void ClearSummary()
        {
            SummaryGroupCollection.Clear();
            TotalSummary.ClearSummary();
        }

        public void CreateSummaries(DateTime[] spans, bool isSortedByMonth)
        {
            if (SummaryGroupCollection.Count == 0)
            {
                foreach (DateTime span in spans)
                {
                    SummaryGroupCollection.Add(new FinanceSummaryGroupViewModel(span, isSortedByMonth));
                    SummaryGroupCollection.Last().SummaryItemClickedEvent += OnSummaryClicked;
                }
            }
        }

        public void AddSummary(Dictionary<DateTime, decimal> summaries, Color itemColor, string itemName, StatisticTypeEnum statisticType)
        {
            foreach (DateTime summaryDate in summaries.Keys)
            {
                SummaryGroupCollection.Where(s => s.SummaryDate == summaryDate).First().AddSummaryItem(itemName, summaries[summaryDate], statisticType, itemColor);
            }
            TotalSummary.AddSummaryItem(itemName, summaries.Values.Sum(), statisticType, itemColor);
            if(statisticType== StatisticTypeEnum.Capital)
            {
                TotalSummary.Vis = System.Windows.Visibility.Hidden;
            }
            else
            {
                TotalSummary.Vis = System.Windows.Visibility.Visible;
            }
        }

        private void OnSummaryClicked(StatisticTypeEnum statisticType, DateTime summaryDate, bool isTotal, Color itemColor)
        {
            if (isTotal)
            {
                foreach (FinanceSummaryGroupViewModel item in SummaryGroupCollection)
                {
                    item.ChangeGroupState(false);
                }
            }
            else
            {
                foreach (FinanceSummaryGroupViewModel item in SummaryGroupCollection.Where(s => s.SummaryDate != summaryDate))
                {
                    item.ChangeGroupState(false);
                }
                TotalSummary.ChangeGroupState(false);
            }

            _currentItemColor = itemColor;
            _bussiness.GetDetails(statisticType, summaryDate, isTotal);
        }
    }
}
