using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.Statistic.CommonComponent.Summary.Details
{
    class TeachingCountViewModel : ViewModelBase,IComparable<TeachingCountViewModel>
    {
        private string _className;

        public string ClassName
        {
            get { return _className; }
            set
            {
                _className = value;
                RaisePropertyChanged("ClassName");
            }
        }

        private string _classDate;

        public string ClassDate
        {
            get { return _classDate; }
            set
            {
                _classDate = value;
                RaisePropertyChanged("ClassDate");
            }
        }

        public DateTime Date
        {
            get
            {
                return _date;
            }
        }

        private DateTime _date;
        public TeachingCountViewModel(string className, DateTime classDate)
        {
            ClassName = className;
            ClassDate = classDate.ToString("yyyy年MM月dd日");
            _date = classDate;
        }

        public int CompareTo(TeachingCountViewModel other)
        {
            if (Date > other.Date)
            {
                return 1;
            }
            else if (Date < other.Date)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
