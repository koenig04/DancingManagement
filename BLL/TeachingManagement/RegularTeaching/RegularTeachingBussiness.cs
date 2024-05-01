using BLL.ClassManagement;
using BLL.TraineeManagement;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.TeachingManagement.RegularTeaching
{
    public class RegularTeachingBussiness
    {
        public RegularClassManagement RegularClasses { get; private set; }
        public RegularClassOperationBussiness RegularClassOperation { get; private set; }
        public RegularTraineeBussiness RegularTrainee { get; private set; }
        public TraineeManagementBussiness TraineeManagement { get; private set; }
        public event EventHandler TraineeCountChanged;
        public delegate void OverdueChanged();
        public event OverdueChanged OverdueChangedEvent;

        public RegularTeachingBussiness(RegularClassManagement regularClass, TraineeManagementBussiness traineeManagement)
        {
            RegularClasses = regularClass;
            RegularClassOperation = new RegularClassOperationBussiness(regularClass,traineeManagement.Dal);
            RegularClassOperation.OverdueChangedEvent += () => OverdueChangedEvent?.Invoke();
            RegularClasses.ChangeRegularClassEvent += (operation, model) =>
            {
                if (operation == Common.OperationType.Select)
                {
                    RegularTrainee.GetTrainees(model.ClassID);
                }
                else
                {
                    RegularClassOperation.Enable(operation, model);
                }
            };

            TraineeManagement = traineeManagement;
            RegularTrainee = new RegularTraineeBussiness(TraineeManagement.Dal);
            RegularTrainee.TraineeChangedEvent += (opreation, trainee, newIndex) =>
            {
                if (opreation == Common.OperationType.Add && trainee.IsNew)
                {
                    TraineeManagement.AddTrainee(trainee);
                }
                else if (opreation == Common.OperationType.Update || opreation == Common.OperationType.Delete)
                {
                    TraineeManagement.UpdateTrainee(trainee);
                }
                TraineeCountChanged?.Invoke(null, EventArgs.Empty);
            };
        }
    }
}
