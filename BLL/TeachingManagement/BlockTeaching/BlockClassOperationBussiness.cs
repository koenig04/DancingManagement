using BLL.ClassManagement;
using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.TeachingManagement.BlockTeaching
{
    public class BlockClassOperationBussiness
    {
        public delegate void OperationEnable(OperationType operation, BlockClassModel model);
        public event OperationEnable OperationEnableEvent;


        private BlockClassManagement _bussiness;

        public BlockClassOperationBussiness(BlockClassManagement blockClass)
        {
            _bussiness = blockClass;
        }

        public void Enable(OperationType operation, BlockClassModel model)
        {
            OperationEnableEvent?.Invoke(operation, model);
        }

        public void OperateBlockClass(OperationType operation, BlockClassModel model)
        {
            _bussiness.OperateClassInfo(operation, model);
        }
    }
}
