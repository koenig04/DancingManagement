using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.Statistic.Teacher
{
    class CLassCountViewModel : ViewModelBase
    {
        private string date_;

        public string ClassDate
        {
            get { return date_; }
            set { date_ = value; RaisePropertyChanged("ClassDate"); }
        }

        private string count_;

        public string ClassCount
        {
            get { return count_; }
            set { count_ = value; RaisePropertyChanged("ClassCount"); }
        }

        public CLassCountViewModel(string classDate, string classCount)
        {
            ClassDate = classDate;
            ClassCount = "x" + classCount;
        }
    }
}
