using BLL.UserSettingManagement;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.UserSetting
{
    class UserSettingViewModel : ViewModelBase
    {
        private int _settingFunc;

        public int SettingFunc
        {
            get { return _settingFunc; }
            set
            {
                _settingFunc = value;
                RaisePropertyChanged("SettingFunc");
            }
        }

        private DelegateCommand _changeSetting;

        public DelegateCommand ChangeSettingFunc
        {
            get
            {
                _changeSetting = _changeSetting ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        SettingFunc = int.Parse(o.ToString());
                    }));
                return _changeSetting;
            }
            set
            {
                _changeSetting = value;
                RaisePropertyChanged("ChangeSettingFunc");
            }
        }


        private ChangingPasswordViewModel _changingpw;

        public ChangingPasswordViewModel ChangingPassword
        {
            get { return _changingpw; }
            set
            {
                _changingpw = value;
                RaisePropertyChanged("ChangingPassword");
            }
        }

        public UserSettingViewModel(UserSettingManagementBussiness bussiness)
        {
            ChangingPassword = new ChangingPasswordViewModel(bussiness.ChangePassword);
        }
    }
}
