using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.CommonComponent.Message
{
    class OperationMessageViewModel : ViewModelBase
    {
        private string _contentOne;

        public string MsgContentOne
        {
            get { return _contentOne; }
            set
            {
                _contentOne = value;
                RaisePropertyChanged("MsgContentOne");
            }
        }

        private string _two;

        public string MsgContentTwo
        {
            get { return _two; }
            set
            {
                _two = value;
                RaisePropertyChanged("MsgContentTwo");
            }
        }

        private string _three;

        public string MsgContentThree
        {
            get { return _three; }
            set
            {
                _three = value;
                RaisePropertyChanged("MsgContentThree");
            }
        }

        private string _four;

        public string MsgContentFour
        {
            get { return _four; }
            set
            {
                _four = value;
                RaisePropertyChanged("MsgContentFour");
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
                        ConfirmOperationEvent?.Invoke();
                        Vis = System.Windows.Visibility.Hidden;
                    }));
                return _confirm;
            }
            set { _confirm = value; }
        }

        private string _confirmText;

        public string ConfirmText
        {
            get { return _confirmText; }
            set
            {
                _confirmText = value;
                RaisePropertyChanged("ConfirmText");
            }
        }

        private DelegateCommand _cancel;

        public DelegateCommand Cancel
        {
            get
            {
                _cancel = _cancel ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        Vis = System.Windows.Visibility.Hidden;
                    }));
                return _cancel;
            }
            set
            {
                _cancel = value;
                RaisePropertyChanged("Cancel");
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

        public delegate void ConfirmOperation();
        public event ConfirmOperation ConfirmOperationEvent;

        public OperationMessageViewModel(bool isWholePage, string confirmText)
        {
            ConfirmText = confirmText;
            MsgTotalHeight = isWholePage ? 700 : 600;
        }

        public void Enable(string messageOne, string messageTwo = null, string messageThree = null, string messageFour = null)
        {
            MsgContentOne = messageOne;
            MsgContentTwo = messageTwo;
            MsgContentThree = messageThree;
            MsgContentFour = messageFour;
            Vis = System.Windows.Visibility.Visible;
        }
    }
}
