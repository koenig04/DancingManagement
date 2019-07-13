using BLL.ClassManagement;
using BLL.NameCallingManagement;
using BLL.PaymentManagement.ClassPayment;
using BLL.PaymentManagement.NormalPayment;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.PaymentManagement
{
    public class PaymentManagementBussiness
    {
        public ClassPaymentBussiness ClassPayment { get; private set; }
        public NormalPaymentBussiness NormalPayment { get; private set; }
        public PaymentInfo PaymentDAL { get; private set; }

        public PaymentManagementBussiness(BlockClassManagement block, RegularClassManagement regular, TraineeInfo trainee, NameCallingBussiness nameCalling)
        {
            PaymentDAL = new PaymentInfo();
            ClassPayment = new ClassPaymentBussiness(block, regular, trainee, PaymentDAL, nameCalling);
            NormalPayment = new NormalPaymentBussiness(PaymentDAL);         
        }
    }
}
