using BLL.UserSettingManagement;
using Common;
using DancingTrainingManagement.Components.CommonComponent.Message;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.UserSetting
{
    class ChangingPasswordViewModel : ViewModelBase
    {
        private string _oldPW;

        public string OldPassword
        {
            get { return _oldPW; }
            set
            {
                _oldPW = value;
                RaisePropertyChanged("OldPassword");
            }
        }

        private string _newPW;

        public string NewPassword
        {
            get { return _newPW; }
            set
            {
                _newPW = value;
                RaisePropertyChanged("NewPassword");
            }
        }

        private string _newAgain;

        public string NewPasswordAgain
        {
            get { return _newAgain; }
            set
            {
                _newAgain = value;
                RaisePropertyChanged("NewPasswordAgain");
            }
        }

        private MessageViewModel _msg;

        public MessageViewModel Msg
        {
            get { return _msg; }
            set
            {
                _msg = value;
                RaisePropertyChanged("Msg");
            }
        }

        private DelegateCommand _change;

        public DelegateCommand Change
        {
            get
            {
                _change = _change ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        MessageType msg;
                        if (_bussiness.ChangePassword(OldPassword, NewPassword, NewPasswordAgain, out msg))
                        {
                            Msg.Enable(msg, MessageLevel.Info);
                        }
                        else
                        {
                            Msg.Enable(msg, MessageLevel.Warning);
                        }
                    }));
                return _change;
            }
            set
            {
                _change = value;
                RaisePropertyChanged("Change");
            }
        }


        private ChangPasswordBussiness _bussiness;
        public ChangingPasswordViewModel(ChangPasswordBussiness bussiness)
        {
            _bussiness = bussiness;
            Msg = new MessageViewModel(false);
            Msg.OnOperateEnableEvent(false, false);
        }
    }
}
