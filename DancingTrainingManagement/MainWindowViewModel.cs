using BLL;
using Common;
using DancingTrainingManagement.Components.NameCallingManagement;
using DancingTrainingManagement.Components.Overdue;
using DancingTrainingManagement.Components.Payment;
using DancingTrainingManagement.Components.Statistic;
using DancingTrainingManagement.Components.TeachingManagement;
using DancingTrainingManagement.Components.UserSetting;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DancingTrainingManagement
{
    class MainWindowViewModel : ViewModelBase
    {
        private int _mainFunc;

        public int MainFunc
        {
            get { return _mainFunc; }
            set
            {
                _mainFunc = value;
                Calling.OnOperateEnableEvent(true, value == 0 ? true : false);
                Statistic.OnOperateEnableEvent(true, value == 2 ? true : false);
                Payment.OnOperateEnableEvent(true, value == 1 ? true : false);
                Teaching.OnOperateEnableEvent(true, value == 3 ? true : false);
                Overdue.OnOperateEnableEvent(true, value == 4 ? true : false);
                Setting.OnOperateEnableEvent(true, value == 5 ? true : false);
                RaisePropertyChanged("MainFunc");
            }
        }

        private DelegateCommand _changeFunc;

        public DelegateCommand ChangeMainFunc
        {
            get
            {
                _changeFunc = _changeFunc ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        MainFunc = int.Parse(o.ToString());
                    }));
                return _changeFunc;
            }
            set
            {
                _changeFunc = value;
                RaisePropertyChanged("ChangeMainFunc");
            }
        }

        private TeachingManagementViewModel _teaching;

        public TeachingManagementViewModel Teaching
        {
            get { return _teaching; }
            set
            {
                _teaching = value;
                RaisePropertyChanged("Teaching");
            }
        }

        private NameCallingManagementViewModel _calling;

        public NameCallingManagementViewModel Calling
        {
            get { return _calling; }
            set
            {
                _calling = value;
                RaisePropertyChanged("NameCallingManagementViewModel");
            }
        }


        private PaymentViewModel _pay;

        public PaymentViewModel Payment
        {
            get { return _pay; }
            set
            {
                _pay = value;
                RaisePropertyChanged("Payment");
            }
        }

        private OverdueViewModel _overdue;

        public OverdueViewModel Overdue
        {
            get { return _overdue; }
            set
            {
                _overdue = value;
                RaisePropertyChanged("Overdue");
            }
        }

        private StatisticViewModel _statistic;

        public StatisticViewModel Statistic
        {
            get { return _statistic; }
            set
            {
                _statistic = value;
                RaisePropertyChanged("Statistic");
            }
        }

        private UserSettingViewModel _setting;

        public UserSettingViewModel Setting
        {
            get { return _setting; }
            set
            {
                _setting = value;
                RaisePropertyChanged("Setting");
            }
        }


        private DelegateCommand _exitApp;

        public DelegateCommand ExitApp
        {
            get
            {
                _exitApp = _exitApp ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        System.Windows.Application.Current.Shutdown();
                    }));
                return _exitApp;
            }
            set
            {
                _exitApp = value;
                RaisePropertyChanged("ExitApp");
            }
        }

        private MainWindowBussiness _bussiness;
        public MainWindowViewModel(string userID)
        {
            _bussiness = new MainWindowBussiness(userID);
            Teaching = new TeachingManagementViewModel(_bussiness.Teaching);
            Calling = new NameCallingManagementViewModel(_bussiness.Calling);
            Payment = new PaymentViewModel(_bussiness.Payment);
            Overdue = new OverdueViewModel(_bussiness.Overdue);
            Statistic = new StatisticViewModel(_bussiness.Statistic);
            Setting = new UserSettingViewModel(_bussiness.UserSetting);
            MainFunc = 0;
        }
    }
}
