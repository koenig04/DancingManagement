using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.PaymentManagement.ClassPayment.PaymentSelecter
{
    public class PaymentInCountBussiness
    {
        public delegate void ClassTypeChanged(int classType);
        public event ClassTypeChanged ClassTypeChangedEvent;
        public delegate void SelectedClassChanged(ClassModel model);
        public event SelectedClassChanged SelectedClassChangedEvent;

        public void OnClassTypeChanged(int classType)
        {
            ClassTypeChangedEvent?.Invoke(classType);
        }

        public void OnClassModelChanged(ClassModel model)
        {
            SelectedClassChangedEvent?.Invoke(model);
        }
    }
}
