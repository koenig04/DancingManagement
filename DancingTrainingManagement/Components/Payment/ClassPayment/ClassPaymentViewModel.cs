using BLL.PaymentManagement.ClassPayment;
using Common;
using DancingTrainingManagement.Components.CommonComponent.ClassSelecter;
using DancingTrainingManagement.Components.Payment.ClassPayment.ClassFee;
using DancingTrainingManagement.Components.Payment.ClassPayment.TeacherFee;
using DancingTrainingManagement.UICore;
using Model.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.Payment.ClassPayment
{
    class ClassPaymentViewModel : ViewModelBase
    {
        private ClassFeeViewModel _classFee;

        public ClassFeeViewModel ClassFee
        {
            get { return _classFee; }
            set
            {
                _classFee = value;
                RaisePropertyChanged("ClassFeeViewModel");
            }
        }

        private TeacherFeeViewModel _teacherFee;

        public TeacherFeeViewModel TeacherFee
        {
            get { return _teacherFee; }
            set
            {
                _teacherFee = value;
                RaisePropertyChanged("TeacherFee");
            }
        }


        public delegate void EnableMsg(MessageType msg, MessageLevel level);
        public event EnableMsg EnableMsgEvent;

        private ClassPaymentBussiness bussiness;
        public ClassPaymentViewModel(ClassPaymentBussiness bussiness)
        {
            ClassFee = new ClassFeeViewModel(bussiness.ClassFee);
            ClassFee.ErrOccuredEvent += Enable;
            ClassFee.ShowInfoEvent += Enable;

            TeacherFee = new TeacherFeeViewModel(bussiness.TeacherFee);
            TeacherFee.ErrOccuredEvent += Enable;
            TeacherFee.ShowInfoEvent += Enable;
        }

        private void Enable(MessageType msg, MessageLevel level)
        {
            EnableMsgEvent?.Invoke(msg, level);
        }
    }
}
