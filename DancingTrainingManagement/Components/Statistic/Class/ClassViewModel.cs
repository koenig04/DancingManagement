using BLL.StatisticManagement.ClassStatistic;
using DancingTrainingManagement.Components.Statistic.Class.Attendence;
using DancingTrainingManagement.Components.Statistic.CommonComponent.StatisticHead;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.Statistic.Class
{
    class ClassViewModel : ConcreteStatisticViewModelBase
    {

        private DateSelecterViewModel selecter_;

        public DateSelecterViewModel DateSelecter
        {
            get { return selecter_; }
            set { selecter_ = value; RaisePropertyChanged("DateSelecter"); }
        }

        private AttendenceViewModel attendence_;

        public AttendenceViewModel Attendence
        {
            get { return attendence_; }
            set { attendence_ = value; RaisePropertyChanged("Attendence"); }
        }



        ClassStatisticBussiness bussiness_;

        public ClassViewModel(ClassStatisticBussiness bussiness) : base()
        {
            bussiness_ = bussiness;

            DateSelecter = new DateSelecterViewModel();
            DateSelecter.DateSpanChangedEvent += OnDateChanged;

            Attendence = new AttendenceViewModel(bussiness.Attendence);
        }

        private void OnDateChanged(DateTime startDate, DateTime endDate, bool isSortedByMonth)
        {
            Attendence.ChangeDate(startDate, endDate);
        }
    }
}
