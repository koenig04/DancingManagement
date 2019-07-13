using BLL.NameCallingManagement;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.PaymentManagement.ClassPayment
{
    public class TeacherFeeBussiness
    {
        public delegate void ClassCountChanged(int count);
        public event ClassCountChanged ClassCountChangedEvent;
        public event EventHandler GeneralCapitalChanged;

        private NameCallingBussiness _nameCalling;
        private TeacherFeeInfo _dal;

        public TeacherFeeBussiness(NameCallingBussiness nameCalling)
        {
            _nameCalling = nameCalling;
            _dal = new TeacherFeeInfo();
        }

        public void QuerryClassCount(string teacherID, DateTime startDate, DateTime endDate)
        {
            ClassCountChangedEvent?.Invoke(_nameCalling.GetClassCountForTeacher(teacherID, startDate, endDate));
        }

        public bool AddTeacherFee(string teacherID, int feeYear, int feeMonth, decimal amount)
        {
            bool ret =  _dal.Add(teacherID, feeYear, feeMonth, amount);
            GeneralCapitalChanged?.Invoke(null, EventArgs.Empty);
            return ret;
        }
    }
}
