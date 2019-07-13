using BLL.ClassManagement;
using BLL.TraineeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.TeachingManagement.BlockTeaching
{
    public class BlockTeachingBussiness
    {
        public BlockClassManagement BlockClasses { get; private set; }
        public BlockClassOperationBussiness BlockClassOperation { get; private set; }
        public TraineeManagementBussiness TraineeManagement { get; private set; }
        public BlockTraineeBussiness BlockTrainee { get; private set; }
        public event EventHandler TraineeCountChanged;

        public BlockTeachingBussiness(BlockClassManagement blockClasses, TraineeManagementBussiness traineeManagement)
        {
            BlockClasses = blockClasses;
            BlockClassOperation = new BlockClassOperationBussiness(blockClasses);
            BlockClasses.ChangeBlockClassEvent += (operation, model) =>
            {
                if (operation == Common.OperationType.Select)
                {
                    BlockTrainee.GetTrainees(model.ClassID);
                }
                else
                {
                    BlockClassOperation.Enable(operation, model);
                }
            };

            TraineeManagement = traineeManagement;
            BlockTrainee = new BlockTraineeBussiness(TraineeManagement.Dal);

            BlockTrainee.TraineeChangedEvent += (opreation, trainee, newIndex) =>
            {
                if (opreation == Common.OperationType.Add && trainee.IsNew)
                {
                    TraineeManagement.AddTrainee(trainee);
                }
                else if (opreation == Common.OperationType.Update)
                {
                    TraineeManagement.UpdateTrainee(trainee);
                }
                TraineeCountChanged?.Invoke(null, EventArgs.Empty);
            };
        }
    }
}
