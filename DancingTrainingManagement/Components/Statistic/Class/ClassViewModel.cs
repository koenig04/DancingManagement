using BLL.StatisticManagement.ClassStatistic;
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


        ClassStatisticBussiness bussiness_;

        public ClassViewModel(ClassStatisticBussiness bussiness) : base()
        {
            bussiness_ = bussiness;

            DateSelecter = new DateSelecterViewModel();
            DateSelecter.DateSpanChangedEvent += OnDateChanged;
        }

        private void OnDateChanged(DateTime startDate, DateTime endDate, bool isSortedByMonth)
        {
            throw new NotImplementedException();
        }
    }
}
