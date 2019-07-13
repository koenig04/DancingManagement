using BLL.ClassManagement;
using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.TeachingManagement.RegularTeaching
{
    public class RegularClassOperationBussiness
    {
        public delegate void OperationEnable(OperationType operation, RegularClassModel model);
        public event OperationEnable OperationEnableEnvet;


        private RegularClassManagement _bussiness;

        public RegularClassOperationBussiness(RegularClassManagement regularClass)
        {
            _bussiness = regularClass;
        }

        public void Enable(OperationType operation, RegularClassModel model)
        {
            OperationEnableEnvet?.Invoke(operation, model);
        }

        public void OperateRegularClass(OperationType operation, RegularClassModel model)
        {
            _bussiness.OperateClassInfo(operation, model);
        }
    }
}
