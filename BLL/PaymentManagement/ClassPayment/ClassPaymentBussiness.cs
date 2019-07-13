using BLL.ClassManagement;
using BLL.NameCallingManagement;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.PaymentManagement.ClassPayment
{
    public class ClassPaymentBussiness
    {
        public ClassFeeBussiness ClassFee { get; private set; }
        public TeacherFeeBussiness TeacherFee { get; private set; }
        public event EventHandler CapitalChanged;

        public ClassPaymentBussiness(BlockClassManagement block, RegularClassManagement regular, TraineeInfo trainee, PaymentInfo dal, NameCallingBussiness nameCalling)
        {
            ClassFee = new ClassFeeBussiness(block, regular, trainee, dal);
            TeacherFee = new TeacherFeeBussiness(nameCalling);

            ClassFee.GeneralCapitalChanged += (sender, args) => CapitalChanged?.Invoke(sender, args);
            TeacherFee.GeneralCapitalChanged += (sender, args) => CapitalChanged?.Invoke(sender, args);
        }
    }
}
