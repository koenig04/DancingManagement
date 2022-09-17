using BLL.StatisticManagement.TeachingStatistic;
using BLL.TeacherManagement;
using Common;
using DancingTrainingManagement.Components.CommonComponent.Calender;
using DancingTrainingManagement.Components.CommonComponent.TeacherSelecter;
using DancingTrainingManagement.Components.CommonComponent.YearMonthSelecter;
using DancingTrainingManagement.Components.Statistic.CommonComponent.StatisticHead;
using DancingTrainingManagement.Components.Statistic.CommonComponent.Summary;
using DancingTrainingManagement.UICore;
using LiveCharts;
using LiveCharts.Wpf;
using Model;
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

        //private CalenderViewModel _calender;

        //public CalenderViewModel Calender
        //{
        //    get { return _calender; }
        //    set
        //    {
        //        _calender = value;
        //        RaisePropertyChanged("Calender");
        //    }
        //}

        private string count_;

        public string TotalClassCount
        {
            get { return count_; }
            set { count_ = value; RaisePropertyChanged("TotalClassCount"); }
        }

        private ClassStatisticViewModel countDetail_;

        public ClassStatisticViewModel ClassCount
        {
            get { return countDetail_; }
            set { countDetail_ = value; RaisePropertyChanged("ClassCount"); }
        }


        private TeacherModel currentTeacher_ = null;
        private int currentYear_ = 0, currentMonth_ = 0;
        private TeachingStatisticBussiness bussiness_;

        public TeacherViewModel(TeachingStatisticBussiness bussiness)
        {
            bussiness_ = bussiness;
            bussiness_.IndividualTeacherCountInfoChangedEvent += OnIndividualTeacherCountChanged;
            bussiness_.IndividualTeacherCountByClassChangedEvent += OnIndividualTeacherCountByClassChanged;

            YMDSelecter = new YearMonthSelecterWithArrowViewModel();
            YMDSelecter.YearMonthChangedEvent += (year, month) =>
              {
                  currentYear_ = year;
                  currentMonth_ = month;
                  SearchTeacherInfo();
              };
            TeacherSelecter = new TeacherSelecterWithArrowViewModel();
            TeacherSelecter.SelectedTeacherChangedEvent += teacher =>
              {
                  currentTeacher_ = teacher;
                  SearchTeacherInfo();
              };
            ClassCount = new ClassStatisticViewModel();
            //Calender = new CalenderViewModel();

            currentYear_ = DateTime.Now.Year;
            currentMonth_ = DateTime.Now.Month;
            currentTeacher_ = TeacherManagementBussiness.Instance.Teachers[0];
            SearchTeacherInfo();
        }

        private void OnIndividualTeacherCountByClassChanged(List<TeachingCountByClass> info)
        {
            ClassCount.UpdateClassList(info);
            int totalCount = 0;
            foreach(var i in info)
            {
                totalCount += (from c in i.Info
                               select c.Value).Sum();
            }
            TotalClassCount = totalCount.ToString();
        }

        private void OnIndividualTeacherCountChanged(TeachingCountGroup info)
        {
            int totalCount = 0;
            List<CalenderItemInfoModel> infos = new List<CalenderItemInfoModel>();
            foreach (var i in info.DetailsGroup)
            {
                if (i.Value != null)
                {
                    List<string> content = new List<string>();
                    List<string> classNames = (from c in i.Value
                                               select c.ClassName).Distinct().ToList();
                    foreach (var name in classNames)
                    {
                        int classCount = i.Value.Count(c => c.ClassName == name);
                        content.Add(name + " x" + classCount.ToString());
                        totalCount += classCount;
                    }

                    infos.Add(new CalenderItemInfoModel(i.Key.Day.ToString(), content));
                }
            }
            //Calender.UpdateInfo(infos);
            TotalClassCount = totalCount.ToString();
        }

        private void SearchTeacherInfo()
        {
            if (currentMonth_ != 0 && currentYear_ != 0 && currentTeacher_ != null)
            {
                //Calender.UpdateDateInfo(currentYear_, currentMonth_);

                DateTime startDate = Convert.ToDateTime(currentYear_.ToString() + "-" + currentMonth_.ToString() + "-1");
                DateTime endDate = startDate.AddMonths(1);

                //bussiness_.SearchTeachingCountForIndividualTeacher(startDate, endDate, currentTeacher_.TeacherID);
                bussiness_.SearchTeachingCountByClassAndTeacher(startDate, endDate, currentTeacher_.TeacherID);
            }
        }
    }
}
