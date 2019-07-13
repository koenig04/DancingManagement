using Common;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace DancingTrainingManagement.Components.Statistic.CommonComponent.StatisticHead
{
    class DateSelecterViewModel : ViewModelBase
    {
        private YMDSwitcherViewModel _ymd;

        public YMDSwitcherViewModel YMD
        {
            get { return _ymd; }
            set
            {
                _ymd = value;
                RaisePropertyChanged("YMD");
            }
        }


        private ObservableCollection<string> _yearCollection;

        public ObservableCollection<string> YearCollection
        {
            get { return _yearCollection; }
            set
            {
                _yearCollection = value;
                RaisePropertyChanged("YearCollection");
            }
        }

        private ObservableCollection<string> _monthCollection;

        public ObservableCollection<string> MonthCollection
        {
            get { return _monthCollection; }
            set
            {
                _monthCollection = value;
                RaisePropertyChanged("MonthCollection");
            }
        }


        private string _selectStartYear;

        public string SelectStartYear
        {
            get { return _selectStartYear; }
            set
            {
                _selectStartYear = value;
                ChangeDateSpan();
                RaisePropertyChanged("SelectStartYear");
            }
        }

        private string _selectStartMonth;

        public string SelectStartMonth
        {
            get { return _selectStartMonth; }
            set
            {
                _selectStartMonth = value;
                ChangeDateSpan();
                RaisePropertyChanged("SelectStartMonth");
            }
        }

        private string _selectEndYear;

        public string SelectEndYear
        {
            get { return _selectEndYear; }
            set
            {
                _selectEndYear = value;
                ChangeDateSpan();
                RaisePropertyChanged("SelectEndYear");
            }
        }

        private string _selectEndMonth;

        public string SelectEndMonth
        {
            get { return _selectEndMonth; }
            set
            {
                _selectEndMonth = value;
                ChangeDateSpan();
                RaisePropertyChanged("SelectEndMonth");
            }
        }

        private Visibility _monthVis;

        public Visibility MonthVis
        {
            get { return _monthVis; }
            set
            {
                _monthVis = value;
                RaisePropertyChanged("MonthVis");
            }
        }

        public delegate void DateSpanChanged(DateTime startDate, DateTime endDate, bool isSortedByMonth);
        public event DateSpanChanged DateSpanChangedEvent;

        private YMDType _ymdType;
        public DateSelecterViewModel()
        {
            YMD = new YMDSwitcherViewModel();
            YMD.YMDStateChangedEvent += ymd =>
            {
                _ymdType = ymd;
                MonthVis = ymd == YMDType.Month ? Visibility.Visible : Visibility.Hidden;
                ChangeDateSpan();
            };
            _ymdType = YMDType.Month;
            MonthVis = Visibility.Visible;

            YearCollection = new ObservableCollection<string>();
            MonthCollection = new ObservableCollection<string>();
            for (int i = 1; i < 13; i++)
            {
                MonthCollection.Add(i.ToString());
            }
            for (int i = 2019; i < (2019 + 21); i++)
            {
                YearCollection.Add(i.ToString());
            }
        }

        private bool CheckYMDValidity()
        {
            if ((string.IsNullOrEmpty(SelectStartMonth) && _ymdType == YMDType.Month) || string.IsNullOrEmpty(SelectStartYear)
                || (string.IsNullOrEmpty(SelectEndMonth) && _ymdType == YMDType.Month) || string.IsNullOrEmpty(SelectEndYear))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ChangeDateSpan()
        {
            if (CheckYMDValidity())
            {
                DateSpanChangedEvent?.Invoke(
                    _ymdType == YMDType.Month ?
                    DateTime.Parse(string.Format("{0}-{1}-1", SelectStartYear, SelectStartMonth)) :
                    DateTime.Parse(string.Format("{0}-1-1", SelectStartYear)),
                    _ymdType == YMDType.Month ?
                    DateTime.Parse(string.Format("{0}-{1}-1", SelectEndYear, SelectEndMonth)).AddMonths(1).AddDays(-1) :
                    DateTime.Parse(string.Format("{0}-1-1", SelectEndYear)).AddYears(1).AddDays(-1),
                    _ymdType == YMDType.Month ?
                    true : false);
            }
        }
    }
}
