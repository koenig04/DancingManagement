using Common;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.TeachingManagement
{
    public class TraineeOperationBussiness
    {
        public delegate void OperationEnable(OperationType operation, TraineeModel model);
        public event OperationEnable OperationEnableEvent;
        public delegate void TraineeChanged(OperationType operation, TraineeModel model);
        public event TraineeChanged TraineeChangedEvent;      

        public void Enable(OperationType operation, TraineeModel model)
        {
            OperationEnableEvent?.Invoke(operation, model);
        }

        public void OperateTrainee(OperationType operation, TraineeModel model)
        {
            TraineeChangedEvent?.Invoke(operation, model);
        }
    }
}
