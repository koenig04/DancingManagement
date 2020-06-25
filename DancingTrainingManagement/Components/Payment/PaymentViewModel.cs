using BLL.PaymentManagement;
using DancingTrainingManagement.Components.CommonComponent.Message;
using DancingTrainingManagement.Components.Payment.ClassPayment;
using DancingTrainingManagement.Components.Payment.NormalPayment;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.Payment
{
    class PaymentViewModel : ViewModelBase
    {
        private int _paymentFunc;

        public int PaymentFunc
        {
            get { return _paymentFunc; }
            set
            {
                _paymentFunc = value;
                ClassPayment.OnOperateEnableEvent(true, value == 0 ? true : false);
                NormalPayment.OnOperateEnableEvent(true, value == 1 ? true : false);
                RaisePropertyChanged("PaymentFunc");
            }
        }


        private ClassPaymentViewModel _classPayment;

        public ClassPaymentViewModel ClassPayment
        {
            get { return _classPayment; }
            set
            {
                _classPayment = value;
                RaisePropertyChanged("ClassPayment");
            }

        }

        private NormalPaymentViewModel _normalPayment;

        public NormalPaymentViewModel NormalPayment
        {
            get { return _normalPayment; }
            set
            {
                _normalPayment = value;
                RaisePropertyChanged("NormalPayment");
            }
        }

        private DelegateCommand _changePayment;

        public DelegateCommand ChangePayment
        {
            get
            {
                _changePayment = _changePayment ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        PaymentFunc = int.Parse(o.ToString());
                    }));
                return _changePayment;
            }
            set
            {
                _changePayment = value;
                RaisePropertyChanged("ChangePayment");
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


        private PaymentManagementBussiness _bussiness;
        public PaymentViewModel(PaymentManagementBussiness bussiness)
        {
            ClassPayment = new ClassPaymentViewModel(bussiness.ClassPayment);
            NormalPayment = new NormalPaymentViewModel(bussiness.NormalPayment);            
            Msg = new MessageViewModel(false);
            Msg.OnOperateEnableEvent(false, false);

            ClassPayment.EnableMsgEvent += Msg.Enable;
            NormalPayment.ErrOccuredEvent += Msg.Enable;
            NormalPayment.ShowInfoEvent += Msg.Enable;
            PaymentFunc = 0;
        }
    }
}
