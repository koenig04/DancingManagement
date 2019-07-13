using Common;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.CommonComponent.Message
{
    class MessageViewModel : ViewModelBase
    {
        private string _content;

        public string MsgContent
        {
            get { return _content; }
            set
            {
                _content = value;
                RaisePropertyChanged("MsgContent");
            }
        }

        private DelegateCommand _confirm;

        public DelegateCommand Confirm
        {
            get
            {
                _confirm = _confirm ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        Vis = System.Windows.Visibility.Hidden;
                    }));
                return _confirm;
            }
            set { _confirm = value; }
        }

        private MessageLevel _msgLevel;

        public MessageLevel MsgLevel
        {
            get { return _msgLevel; }
            set
            {
                _msgLevel = value;
                RaisePropertyChanged("MsgLevel");
            }
        }

        private int _totalHeight;

        public int MsgTotalHeight
        {
            get { return _totalHeight; }
            set
            {
                _totalHeight = value;
                RaisePropertyChanged("MsgTotalHeight");
            }
        }

        public MessageViewModel(bool isWholePage)
        {
            MsgTotalHeight = isWholePage ? 700 : 600;
        }

        public void Enable(MessageType msg, MessageLevel level)
        {
            MsgContent = Common.Message.Instance.MessageDic[msg];
            MsgLevel = level;
            Vis = System.Windows.Visibility.Visible;
        }
    }
}
