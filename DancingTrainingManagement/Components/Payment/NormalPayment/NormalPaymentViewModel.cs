using BLL.PaymentManagement.NormalPayment;
using Common;
using DancingTrainingManagement.UICore;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DancingTrainingManagement.Components.Payment.NormalPayment
{
    class NormalPaymentViewModel : ViewModelBase
    {
        private int _inOutState = -1;

        public int InOutState
        {
            get { return _inOutState; }
            set
            {
                _inOutState = value;
                _bussiness.ChangeInOutState(value == 0 ? true : false);
                RaisePropertyChanged("InOutState");
            }
        }

        private DelegateCommand _changeInOut;

        public DelegateCommand ChangeInOutState
        {
            get
            {
                _changeInOut = _changeInOut ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        InOutState = int.Parse(o.ToString());
                    }));
                return _changeInOut;
            }
            set
            {
                _changeInOut = value;
                RaisePropertyChanged("ChangeInOutState");
            }
        }

        private ObservableCollection<AccountItemViewModel> _itemCollection;

        public ObservableCollection<AccountItemViewModel> ItemCollection
        {
            get { return _itemCollection; }
            set
            {
                _itemCollection = value;
                RaisePropertyChanged("ItemCollection");
            }
        }

        private DateTime _accountDate;

        public DateTime AccountDate
        {
            get { return _accountDate; }
            set
            {
                _accountDate = value;
                RaisePropertyChanged("AccountDate");
            }
        }

        private string _accountAmount;

        public string AccountAmount
        {
            get { return _accountAmount; }
            set
            {
                _accountAmount = value;
                RaisePropertyChanged("AccountAmount");
            }
        }

        private string _notice;

        public string Notice
        {
            get { return _notice; }
            set
            {
                _notice = value;
                RaisePropertyChanged("Notice");
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
                        if (CheckValidity())
                        {
                            AccountInfoModel account = new AccountInfoModel();
                            account.AccountDate = AccountDate;
                            account.AccountAmount = decimal.Parse(AccountAmount);
                            account.ItemID = _currentItem.ItemID;
                            account.Notice = Notice;
                            _bussiness.AddNormalPayment(account);
                            ShowInfoEvent?.Invoke(MessageType.NormalPaymentSuccess);
                        }
                    }));
                return _confirm;
            }
            set
            {
                _confirm = value;
                RaisePropertyChanged("RaisePropertyChanged");
            }
        }

        private DelegateCommand _clearAll;

        public DelegateCommand ClearAll
        {
            get
            {
                _clearAll = _clearAll ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        AccountAmount = string.Empty;
                        Notice = string.Empty;
                        _currentItem = null;
                        foreach (AccountItemViewModel item in ItemCollection)
                        {
                            item.ChangeSelecState(false);
                        }
                    }));
                return _clearAll;
            }
            set
            {
                _clearAll = value;
                RaisePropertyChanged("ClearAll");
            }
        }


        public delegate void ErrOccured(MessageType msg, MessageLevel level = MessageLevel.Warning);
        public event ErrOccured ErrOccuredEvent;
        public delegate void ShowInfo(MessageType msg, MessageLevel level = MessageLevel.Info);
        public event ShowInfo ShowInfoEvent;

        private NormalPaymentBussiness _bussiness;
        private ItemInfoModel _currentItem;
        public NormalPaymentViewModel(NormalPaymentBussiness bussiness)
        {
            _bussiness = bussiness;
            ItemCollection = new ObservableCollection<AccountItemViewModel>();
            AccountDate = DateTime.Now;

            _bussiness.ItemsChangedEvent += items =>
            {
                ItemCollection.Clear();
                _currentItem = null;
                items.ForEach(i =>
                {
                    ItemCollection.Add(new AccountItemViewModel(i));
                    ItemCollection.Last().ItemSelectedEvent += item =>
                    {
                        _currentItem = item;
                        foreach (AccountItemViewModel accountItem in ItemCollection.Where(t => t.ItemID != item.ItemID))
                        {
                            accountItem.ChangeSelecState(false);
                        }
                    };
                });
            };
        }

        private bool CheckValidity()
        {
            if (string.IsNullOrEmpty(AccountAmount) || !Regex.IsMatch(AccountAmount, @"^[+-]?\d*[.]?\d*$"))
            {
                return RaiseErrOccuredEvent(MessageType.NormalPaymentAmountErr);
            }
            if (_currentItem == null)
            {
                return RaiseErrOccuredEvent(MessageType.NormalPaymentItemErr);
            }
            if (string.IsNullOrEmpty(Notice) && _currentItem.ItemName == "其他")
            {
                return RaiseErrOccuredEvent(MessageType.NormalPaymentNoticeErr);
            }
            return true;
        }

        private bool RaiseErrOccuredEvent(MessageType msg)
        {
            ErrOccuredEvent?.Invoke(msg, MessageLevel.Warning);
            return false;
        }
    }
}
