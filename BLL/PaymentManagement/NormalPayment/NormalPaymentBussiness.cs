using BLL.ItemManagement;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.PaymentManagement.NormalPayment
{
    public class NormalPaymentBussiness
    {
        public delegate void ItemsChanged(List<ItemInfoModel> items);
        public event ItemsChanged ItemsChangedEvent;
        public event EventHandler CapitalChanged;

        private PaymentInfo _dal;
        public NormalPaymentBussiness(PaymentInfo dal)
        {
            _dal = dal;
        }

        public void ChangeInOutState(bool isIncome)
        {
            ItemsChangedEvent?.Invoke(ItemManagementBussiness.Instance.Items.Where(i => i.IsIncome == isIncome).ToList());
        }

        public void AddNormalPayment(AccountInfoModel model)
        {
            _dal.AddNormalPayment(model);
            CapitalChanged?.Invoke(null, EventArgs.Empty);
        }

    }
}
