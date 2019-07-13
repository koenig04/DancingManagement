using BLL.PaymentManagement.ClassPayment;
using Common;
using DancingTrainingManagement.Components.CommonComponent.ClassSelecter;
using DancingTrainingManagement.Components.Payment.ClassPayment.ClassFee.PaymentSelecter;
using DancingTrainingManagement.UICore;
using Model.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.Payment.ClassPayment.ClassFee
{
    class ClassFeeViewModel : ViewModelBase
    {
        private ClassSelecterViewModel _classSlecter;

        public ClassSelecterViewModel ClassSelecter
        {
            get { return _classSlecter; }
            set
            {
                _classSlecter = value;
                RaisePropertyChanged("ClassSelecter");
            }
        }

        private int _paymentType;

        public int PaymentType
        {
            get { return _paymentType; }
            set
            {
                _paymentType = value;
                CountPayment.OnOperateEnableEvent(true, value == 0 ? true : false);
                RaisePropertyChanged("PaymentType");
            }
        }

        private DelegateCommand _changePayment;

        public DelegateCommand ChangePaymentType
        {
            get
            {
                _changePayment = _changePayment ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        PaymentType = int.Parse(o.ToString());
                    }));
                return _changePayment;
            }
            set
            {
                _changePayment = value;
                RaisePropertyChanged("ChangePaymentType");
            }
        }

        private PaymentInCountViewModel _countPayment;

        public PaymentInCountViewModel CountPayment
        {
            get
            {
                return _countPayment;
            }
            set
            {
                _countPayment = value;
                RaisePropertyChanged("CountPayment");
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
                            if (PaymentType == 0)
                            {
                                ClassPaymentModel model = new ClassPaymentModel();
                                model.PaymentDate = DateTime.Now;
                                model.TraineeID = ClassSelecter.GetCurrentTraineeID();
                                model.TotalCost = decimal.Parse(CountPayment.TotalCost);
                                model.ClassType = ClassSelecter.ClassType;
                                model.ClassID = ClassSelecter.GetCurrentClassID();
                                model.PaymentTerms = CountPayment.TermCount;
                                model.TotalCount = Configuration.Instance.Configurations.CountPerTerm * CountPayment.TermCount;
                                _bussiness.AddPayment(model);
                                ShowInfoEvent?.Invoke(MessageType.TraineePaymentSuccess);
                            }
                        }
                    }));
                return _confirm;
            }
            set
            {
                _confirm = value;
                RaisePropertyChanged("Confirm");
            }
        }

        public delegate void ErrOccured(MessageType msg, MessageLevel level = MessageLevel.Warning);
        public event ErrOccured ErrOccuredEvent;
        public delegate void ShowInfo(MessageType msg, MessageLevel level = MessageLevel.Info);
        public event ShowInfo ShowInfoEvent;

        private ClassFeeBussiness _bussiness;
        public ClassFeeViewModel(ClassFeeBussiness bussiness)
        {
            _bussiness = bussiness;

            ClassSelecter = new ClassSelecterViewModel(bussiness.ClassSelecter, false, true);
            CountPayment = new PaymentInCountViewModel(bussiness.CountPayment);

            PaymentType = -1;
        }

        private bool CheckValidity()
        {
            if (ClassSelecter.ClassType < 0)
            {
                return RaiseErrOccuredEvent(MessageType.ClassTypeErr);
            }
            if (string.IsNullOrEmpty(ClassSelecter.SelectedClass))
            {
                return RaiseErrOccuredEvent(MessageType.ClassNameErr);
            }
            if (string.IsNullOrEmpty(ClassSelecter.SelectedTrainee))
            {
                return RaiseErrOccuredEvent(MessageType.TraineeErr);
            }
            if (PaymentType != 0)
            {
                return RaiseErrOccuredEvent(MessageType.TimePaymentNotSupport);
            }
            if (CountPayment.TermCount <= 0 && ClassSelecter.ClassType == 0)
            {
                return RaiseErrOccuredEvent(MessageType.TermCountErr);
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
