using BLL.TeachingManagement.Others;
using Common;
using DancingTrainingManagement.Components.CommonComponent.Message;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.TeachingManagement.Others
{
    class OthersViewModel : ViewModelBase
    {
        private DelegateCommand _exportCallingList;

        public DelegateCommand ExportCallingList
        {
            get
            {
                _exportCallingList = _exportCallingList ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        if (_bussiness.ExportCallingList())
                        {
                            Msg.Enable(MessageType.ExportSuccess, MessageLevel.Info);
                        }
                        else
                        {
                            Msg.Enable(MessageType.ExportFailed, MessageLevel.Warning);
                        }
                    }));
                return _exportCallingList;
            }
            set
            {
                _exportCallingList = value;
                RaisePropertyChanged("ExportCallingList");
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

        private OthersBussiness _bussiness;
        public OthersViewModel(OthersBussiness bussiness)
        {
            _bussiness = bussiness;
            Msg = new MessageViewModel(false);
            Msg.OnOperateEnableEvent(false, false);
        }
    }
}
