using BLL.ClassManagement;
using BLL.CommonBussiness;
using BLL.PaymentManagement.ClassPayment.PaymentSelecter;
using BLL.TeachingManagement.BlockTeaching;
using BLL.TeachingManagement.RegularTeaching;
using DAL;
using Model.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.PaymentManagement.ClassPayment
{
    public class ClassFeeBussiness
    {
        public delegate void OverdueChanged();
        public event OverdueChanged OverdueChangedEvent;
        public event EventHandler GeneralCapitalChanged;

        public ClassSelecterBussiness ClassSelecter { get; private set; }
        public PaymentInCountBussiness CountPayment { get; private set; }

        private PaymentInfo _dal;
        public ClassFeeBussiness(BlockClassManagement block, RegularClassManagement regular, TraineeInfo trainee, PaymentInfo dal)
        {
            ClassSelecter = new ClassSelecterBussiness(regular, block, new RegularTraineeBussiness(trainee), new BlockTraineeBussiness(trainee), true);
            CountPayment = new PaymentInCountBussiness();
            _dal = dal;

            ClassSelecter.ClassTypeChangedEvent += CountPayment.OnClassTypeChanged;
            ClassSelecter.SelectedClassChangedEvent += CountPayment.OnClassModelChanged;
        }

        public void AddPayment(ClassPaymentModel model)
        {
            _dal.AddCountPayment(model);
            OverdueChangedEvent?.Invoke();
            GeneralCapitalChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}
