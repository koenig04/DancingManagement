using BLL.StatisticManagement.ClassStatistic;
using Common;
using DancingTrainingManagement.UICore;
using LiveCharts;
using LiveCharts.Wpf;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace DancingTrainingManagement.Components.Statistic.Class.Attendence
{
    class AttendenceViewModel : ViewModelBase
    {
        private GeneralClassSwitcherViewModel switcher_;

        public GeneralClassSwitcherViewModel GeneralClassSwitcher
        {
            get { return switcher_; }
            set { switcher_ = value; RaisePropertyChanged("GeneralClassSwitcher"); }
        }

        private ObservableCollection<string> _classes;

        public ObservableCollection<string> ClassCollection
        {
            get { return _classes; }
            set
            {
                _classes = value;
                RaisePropertyChanged("ClassCollection");
            }
        }

        private string _selectedClass;

        public string SelectedClass
        {
            get { return _selectedClass; }
            set
            {
                _selectedClass = value;
                GetAttendenceRatio();
                RaisePropertyChanged("SelectedClass");
            }
        }

        private SeriesCollection _seriesCollection;

        public SeriesCollection PieColletcion
        {
            get { return _seriesCollection; }
            set
            {
                _seriesCollection = value;
                RaisePropertyChanged("PieColletcion");
            }
        }

        private string ratio_;

        public string AttendenceRatio
        {
            get { return ratio_; }
            set { ratio_ = value; RaisePropertyChanged("AttendenceRatio"); }
        }

        private Visibility vis_;

        public Visibility VisClass
        {
            get { return vis_; }
            set { vis_ = value; RaisePropertyChanged("VisClass"); }
        }


        private AttendenceBussiness bussiness_;
        private List<RegularClassModel> classInfos_;
        private DateTime? startDate_, endDate_;
        private bool isGeneral_;

        public AttendenceViewModel(AttendenceBussiness bussiness)
        {
            bussiness_ = bussiness;
            bussiness_.ClassInfoChangedEvent += infos =>
            {
                ClassCollection.Clear();
                foreach (var c in infos)
                {
                    ClassCollection.Add(c.ClassName);
                }
            };
            classInfos_ = bussiness_.GetClassInfo();

            ClassCollection = new ObservableCollection<string>();
            foreach (var c in classInfos_)
            {
                ClassCollection.Add(c.ClassName);
            }

            GeneralClassSwitcher = new GeneralClassSwitcherViewModel();
            GeneralClassSwitcher.AttendenceTypeChangedEvent += type =>
            {
                isGeneral_ = type == 1 ? true : false;
                VisClass = type == 1 ? Visibility.Hidden : Visibility.Visible;
                GetAttendenceRatio();
            };

            PieColletcion = new SeriesCollection();
            VisClass = Visibility.Hidden;
            isGeneral_ = true;
        }

        public void ChangeDate(DateTime startDate, DateTime endDate)
        {
            startDate_ = startDate;
            endDate_ = endDate;
            GetAttendenceRatio();
        }

        private void GetAttendenceRatio()
        {
            if (startDate_ != null && endDate_ != null && ((!isGeneral_ && !string.IsNullOrWhiteSpace(SelectedClass)) || isGeneral_))
            {
                double ratio;
                if (isGeneral_)
                {
                    ratio = bussiness_.GetAttendenceRatio(string.Empty, (DateTime)startDate_, (DateTime)endDate_, isGeneral_);
                }
                else
                {
                    ratio = bussiness_.GetAttendenceRatio(classInfos_.Where(c => c.ClassName == SelectedClass).First().ClassID, (DateTime)startDate_, (DateTime)endDate_, isGeneral_);
                }
                AttendenceRatio = (ratio * 100).ToString("f2") + "%";
                PieColletcion.Clear();
                PieColletcion.Add(new PieSeries()
                {
                    Fill = new SolidColorBrush(Colors.Transparent),
                    Values = new ChartValues<double> { (1 - ratio) * 100 },
                });
                PieColletcion.Add(new PieSeries()
                {
                    Fill = new SolidColorBrush(GlobalVariables.IncomeColor),
                    Values = new ChartValues<double> { ratio * 100 },
                });
            }
        }
    }
}
