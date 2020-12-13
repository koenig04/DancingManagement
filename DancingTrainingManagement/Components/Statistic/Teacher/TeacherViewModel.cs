using BLL.StatisticManagement.TeachingStatistic;
using Common;
using DancingTrainingManagement.Components.CommonComponent.Calender;
using DancingTrainingManagement.Components.CommonComponent.TeacherSelecter;
using DancingTrainingManagement.Components.CommonComponent.YearMonthSelecter;
using DancingTrainingManagement.Components.Statistic.CommonComponent.StatisticHead;
using DancingTrainingManagement.Components.Statistic.CommonComponent.Summary;
using DancingTrainingManagement.UICore;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DancingTrainingManagement.Components.Statistic.Teacher
{
    class TeacherViewModel : ConcreteStatisticViewModelBase
    {
        private YearMonthSelecterWithArrowViewModel ymdSeleter_;

        public YearMonthSelecterWithArrowViewModel YMDSelecter
        {
            get { return ymdSeleter_; }
            set
            {
                ymdSeleter_ = value;
                RaisePropertyChanged("YMDSelecter");
            }
        }

        private TeacherSelecterWithArrowViewModel _teacherSelecter;

        public TeacherSelecterWithArrowViewModel TeacherSelecter
        {
            get { return _teacherSelecter; }
            set
            {
                _teacherSelecter = value;
                RaisePropertyChanged("TeacherSelecter");
            }
        }

        private CalenderViewModel _calender;

        public CalenderViewModel Calender
        {
            get { return _calender; }
            set
            {
                _calender = value;
                RaisePropertyChanged("Calender");
            }
        }



        public TeacherViewModel(TeachingStatisticBussiness bussiness)
        {
            YMDSelecter = new YearMonthSelecterWithArrowViewModel();
            TeacherSelecter = new TeacherSelecterWithArrowViewModel();
            Calender = new CalenderViewModel();
        }
    }
}
