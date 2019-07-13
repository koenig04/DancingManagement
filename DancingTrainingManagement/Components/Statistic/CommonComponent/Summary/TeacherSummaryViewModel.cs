using BLL.StatisticManagement.TeachingStatistic;
using DancingTrainingManagement.Components.Statistic.CommonComponent.Summary.ItemGroup;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Common;
using System.Windows.Media;
using DancingTrainingManagement.Components.Statistic.CommonComponent.Summary.Details;

namespace DancingTrainingManagement.Components.Statistic.CommonComponent.Summary
{
    class TeacherSummaryViewModel : ViewModelBase
    {
        private TeachingCountDetailsViewModel _details;

        public TeachingCountDetailsViewModel StatisticDetails
        {
            get { return _details; }
            set
            {
                _details = value;
                RaisePropertyChanged("StatisticDetails");
            }
        }

        private bool _isTeacherDetailEnable;

        public bool IsDetailListEnable
        {
            get { return _isTeacherDetailEnable; }
            set
            {
                _isTeacherDetailEnable = value;
                RaisePropertyChanged("IsDetailListEnable");
            }
        }

        private ObservableCollection<TeachingClassCountGroupViewModel> _summaryCollection;

        public ObservableCollection<TeachingClassCountGroupViewModel> SummaryGroupCollection
        {
            get { return _summaryCollection; }
            set
            {
                _summaryCollection = value;
                RaisePropertyChanged("SummaryGroupCollection");
            }
        }

        private TeachingClassCountGroupViewModel _total;

        public TeachingClassCountGroupViewModel TotalSummary
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

        private TeachingStatisticBussiness _bussiness;

        public TeacherSummaryViewModel(TeachingStatisticBussiness bussiness) : base()
        {
            _bussiness = bussiness;
            SummaryGroupCollection = new ObservableCollection<TeachingClassCountGroupViewModel>();
            TotalSummary = new TeachingClassCountGroupViewModel();
            StatisticDetails = new TeachingCountDetailsViewModel();
            _bussiness.ShowDetailsEvent += details =>
            {
                StatisticDetails.Enable(details);
                IsDetailListEnable = true;
            };
            PopWidth = 200;
        }

        public void CreateSummaries(DateTime[] spans, bool isSortedByMonth)
        {
            if (SummaryGroupCollection.Count == 0)
            {
                foreach (DateTime span in spans)
                {
                    SummaryGroupCollection.Add(new TeachingClassCountGroupViewModel(span, isSortedByMonth));
                    SummaryGroupCollection.Last().SummaryItemClickedEvent += OnSummaryClicked;
                }
            }
        }

        public void AddSummary(Dictionary<DateTime, int> summaries, Color itemColor, string itemName, string teacherID)
        {
            foreach (DateTime summaryDate in summaries.Keys)
            {
                SummaryGroupCollection.Where(s => s.SummaryDate == summaryDate).First().AddSummaryItem(itemName, summaries[summaryDate], teacherID, itemColor);
            }
            TotalSummary.AddSummaryItem(itemName, summaries.Values.Sum(), teacherID, itemColor);
        }

        private void OnSummaryClicked(string teacherID, Color itemColor, DateTime summaryDate, bool isTotal)
        {
            if (isTotal)
            {
                foreach (TeachingClassCountGroupViewModel item in SummaryGroupCollection)
                {
                    item.ChangeGroupState(false);
                }
            }
            else
            {
                foreach (TeachingClassCountGroupViewModel item in SummaryGroupCollection.Where(s => s.SummaryDate != summaryDate))
                {
                    item.ChangeGroupState(false);
                }
                TotalSummary.ChangeGroupState(false);
            }
            _bussiness.GetDetails(teacherID, summaryDate, isTotal);
        }

        public void ClearSummary()
        {
            SummaryGroupCollection.Clear();
            TotalSummary.ClearSummary();
        }
    }
}
