using Common;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace DancingTrainingManagement.Components.Statistic.CommonComponent.Summary.ItemGroup
{
    class SummaryItemGroupViewModel : ViewModelBase
    {
        private int _yearMonthType;

        public int YearMonthType
        {
            get { return _yearMonthType; }
            set
            {
                _yearMonthType = value;
                RaisePropertyChanged("YearMonthType");
            }
        }

        private string _month;

        public string SummaryMonth
        {
            get { return _month; }
            set
            {
                _month = value;
                RaisePropertyChanged("SummaryMonth");
            }
        }

        private string _year;

        public string SummaryYear
        {
            get { return _year; }
            set
            {
                _year = value;
                RaisePropertyChanged("SummaryYear");
            }
        }

        private int _totalState;

        public int TotalState
        {
            get { return _totalState; }
            set
            {
                _totalState = value;
                RaisePropertyChanged("TotalState");
            }
        }

        private int _width;

        public int GroupWidth
        {
            get { return _width; }
            set
            {
                _width = value;
                RaisePropertyChanged("GroupWidth");
            }
        }

        public DateTime SummaryDate
        {
            get
            {
                return _summaryDate;
            }
        }

        private Visibility _vis;

        public Visibility Vis
        {
            get { return _vis; }
            set
            {
                _vis = value;
                RaisePropertyChanged("Vis");
            }
        }



        private DateTime _summaryDate;
        protected bool IsTotal;
        public SummaryItemGroupViewModel(DateTime summaryDate, bool isSortedByMonth)
        {
            if (isSortedByMonth)
            {
                YearMonthType = 0;
            }
            else
            {
                YearMonthType = 1;
            }
            TotalState = 0;

            SummaryYear = summaryDate.Year.ToString() + "年";
            SummaryMonth = summaryDate.Month.ToString() + "月";
            _summaryDate = summaryDate;
            IsTotal = false;
        }

        public SummaryItemGroupViewModel()
        {
            TotalState = 1;
            IsTotal = true;
            Vis = System.Windows.Visibility.Visible;
        }

        protected void ChangeGroupWidth(int itemCount)
        {
            GroupWidth = 95 * itemCount + 60;
        }

        public virtual void ClearSummary() { }
    }
}
