using DancingTrainingManagement.UICore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.Overdue
{
    class OverdueItemViewModel : ViewModelBase
    {
        private string _name;

        public string OverdueName
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("OverdueName");
            }
        }

        private string _class;

        public string OverdueClass
        {
            get { return _class; }
            set
            {
                _class = value;
                RaisePropertyChanged("OverdueClass");
            }
        }

        private string _date;

        public string OverdueDate
        {
            get { return _date; }
            set
            {
                _date = value;
                RaisePropertyChanged("OverdueDate");
            }
        }

        private string _amount;

        public string OverdueAmount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                RaisePropertyChanged("OverdueAmount");
            }
        }

        private OverdueModel _overdue;
        public OverdueItemViewModel(OverdueModel overdue)
        {
            _overdue = overdue;
            OverdueName = overdue.TraineeName;
            OverdueClass = overdue.ClassName;
            OverdueDate = overdue.OverdueDate.ToString("yyyy年MM月dd日");
            OverdueAmount = overdue.RenewAmount.ToString();
        }
    }
}
