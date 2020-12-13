using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.CommonComponent.YearMonthSelecter
{
    class YearMonthSelecterWithArrowViewModel : ViewModelBase
    {
        public delegate void YearMonthChanged(int year, int month);
        public event YearMonthChanged YearMonthChangedEvent;

        private int year_;

        public int Year
        {
            get { return year_; }
            set
            {
                year_ = value;
                RaisePropertyChanged("Year");
            }
        }

        private int month_;

        public int Month
        {
            get { return month_; }
            set
            {
                month_ = value;
                RaisePropertyChanged("Month");
            }
        }

        private DelegateCommand _changeMonth;

        public DelegateCommand ChangeMonth
        {
            get
            {
                _changeMonth = _changeMonth ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        checkMonthValidation(int.Parse(o.ToString()));
                    }));
                return _changeMonth;
            }
            set
            {
                _changeMonth = value;
                RaisePropertyChanged("ChangeMonth");
            }
        }

        private void checkMonthValidation(int changing)
        {
            int changedMonth = Month + changing;
            if(changedMonth < 1)
            {
                changedMonth = 12;
                Year -= 1;
            } else if(changedMonth > 12)
            {
                changedMonth = 1;
                Year += 1;
            }

            Month = changedMonth;
            YearMonthChangedEvent?.Invoke(Year, Month);
        }

        public YearMonthSelecterWithArrowViewModel()
        {
            Month = DateTime.Now.Month;
            Year = DateTime.Now.Year;
        }
    }
}
