using BLL.NameCallingManagement;
using DancingTrainingManagement.Components.CommonComponent.Message;
using DancingTrainingManagement.Components.NameCallingManagement.EditNameCalling;
using DancingTrainingManagement.Components.NameCallingManagement.NameCalling;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.NameCallingManagement
{
    class NameCallingManagementViewModel : ViewModelBase
    {
        private NameCallingViewModel _calling;

        public NameCallingViewModel Calling
        {
            get { return _calling; }
            set
            {
                _calling = value;
                RaisePropertyChanged("Calling");
            }
        }

        private EditNameCallingViewModel _editCalling;

        public EditNameCallingViewModel EditCalling
        {
            get { return _editCalling; }
            set
            {
                _editCalling = value;
                RaisePropertyChanged("EditCalling");
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

        private int _callingFunc;

        public int CallingFunc
        {
            get { return _callingFunc; }
            set
            {
                _callingFunc = value;
                Calling.OnOperateEnableEvent(true, value == 0 ? true : false);
                EditCalling.OnOperateEnableEvent(true, value == 1 ? true : false);
                RaisePropertyChanged("CallingFunc");
            }
        }

        private DelegateCommand _changeCalling;

        public DelegateCommand ChangeCallingFunc
        {
            get
            {
                _changeCalling = _changeCalling ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        CallingFunc = int.Parse(o.ToString());
                    }));
                return _changeCalling;
            }
            set
            {
                _changeCalling = value;
                RaisePropertyChanged("ChangeCallingFunc");
            }
        }


        public NameCallingManagementViewModel(NameCallingMangementBussiness calling)
        {
            Calling = new NameCallingViewModel(calling.NameCalling, calling.ClassSelecter);
            EditCalling = new EditNameCallingViewModel(calling.EditCalling);
            CallingFunc = 0;
            Msg = new MessageViewModel(true);
            Msg.OnOperateEnableEvent(false, false);

            Calling.ErrOccuredEvent += Msg.Enable;
            Calling.ShowInfoEvent += Msg.Enable;
            EditCalling.ErrOccuredEvent += Msg.Enable;
        }
    }
}
