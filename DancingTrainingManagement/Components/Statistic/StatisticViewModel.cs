using BLL.StatisticManagement;
using DancingTrainingManagement.Components.Statistic.Finance;
using DancingTrainingManagement.Components.Statistic.General;
using DancingTrainingManagement.Components.Statistic.Teacher;
using DancingTrainingManagement.Components.Statistic.Trainee;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.Statistic
{
    class StatisticViewModel : ViewModelBase
    {
        private int _statisticFunc;

        public int StatisticFunc
        {
            get { return _statisticFunc; }
            set
            {
                _statisticFunc = value;
                Finance.OnOperateEnableEvent(true, value == 0 ? true : false);
                Teacher.OnOperateEnableEvent(true, value == 1 ? true : false);
                Trainee.OnOperateEnableEvent(true, value == 2 ? true : false);
                General.OnOperateEnableEvent(true, value == 3 ? true : false);
                RaisePropertyChanged("StatisticFunc");
            }
        }

        private DelegateCommand _changeFunc;

        public DelegateCommand ChangeStatisticFunc
        {
            get
            {
                _changeFunc = _changeFunc ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        StatisticFunc = int.Parse(o.ToString());
                    }));
                return _changeFunc;
            }
            set
            {
                _changeFunc = value;
                RaisePropertyChanged("ChangeStatisticFunc");
            }
        }

        private FinanceViewModel _finance;

        public FinanceViewModel Finance
        {
            get { return _finance; }
            set
            {
                _finance = value;
                RaisePropertyChanged("Finance");
            }
        }

        private TeacherViewModel _teacher;

        public TeacherViewModel Teacher
        {
            get { return _teacher; }
            set
            {
                _teacher = value;
                RaisePropertyChanged("TeacherViewModel");
            }
        }

        private TraineeViewModel _trainee;

        public TraineeViewModel Trainee
        {
            get { return _trainee; }
            set
            {
                _trainee = value;
                RaisePropertyChanged("Trainee");
            }
        }

        private GeneralViewModel _general;

        public GeneralViewModel General
        {
            get { return _general; }
            set
            {
                _general = value;
                RaisePropertyChanged("General");
            }
        }


        public StatisticViewModel(StatisticManagementBussiness bussiness)
        {
            Finance = new FinanceViewModel(bussiness.Finance);
            Teacher = new TeacherViewModel(bussiness.Teacher);
            Trainee = new TraineeViewModel(bussiness.Trainee);
            General = new GeneralViewModel(bussiness.General);
            StatisticFunc = 3;
        }
    }
}
