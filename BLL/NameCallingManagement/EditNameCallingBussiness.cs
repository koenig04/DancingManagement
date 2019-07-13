using BLL.ClassManagement;
using BLL.CommonBussiness;
using BLL.TeacherManagement;
using BLL.TeachingManagement.BlockTeaching;
using BLL.TeachingManagement.RegularTeaching;
using BLL.TraineeManagement;
using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.NameCallingManagement
{
    public class EditNameCallingBussiness
    {
        public delegate void CallingsChanged(List<NameCallingModel> callings);
        public event CallingsChanged CallingsChangedEvent;

        public NameCallingBussiness Calling { get; private set; }
        public ClassSelecterBussiness ClassSelecter { get; private set; }
        private BlockClassManagement _block;
        private RegularClassManagement _regular;
        private TraineeManagementBussiness _trainees;

        public EditNameCallingBussiness(NameCallingBussiness calling, BlockClassManagement block, RegularClassManagement regular, ClassSelecterBussiness classSelecter,
            TraineeManagementBussiness trainees)
        {
            Calling = calling;
            ClassSelecter = classSelecter;
            _block = block;
            _regular = regular;
            _trainees = trainees;
        }

        public void ChangeEditDate(DateTime date)
        {
            List<NameCallingModel> callings = Calling.GetListByDate(date);
            FillCallingDetails(callings);
            CallingsChangedEvent?.Invoke(callings);
        }

        private void FillCallingDetails(List<NameCallingModel> callings)
        {
            if (callings != null)
            {
                foreach (NameCallingModel item in callings)
                {
                    if (item.ClassType == (int)ClassType.Regular)
                    {
                        item.ClassName = _regular.RegularClassCollection.Where(c => c.ClassID == item.ClassID).First().ClassName;
                    }
                    else
                    {
                        item.ClassName = _block.BlockClassCollection.Where(c => c.ClassID == item.ClassID).First().ClassName;
                    }
                    item.TeacherName = TeacherManagementBussiness.Instance.Teachers.Where(t => t.TeacherID == item.TeacherID).First().TeacherName;
                    item.TraineesWithCallingState = new List<TraineeModel>();
                    if (!string.IsNullOrEmpty(item.Presence))
                    {
                        foreach (string t in item.PresenceTrainees)
                        {
                            FillTraineeCallings(item, t, 0);
                        }
                    }
                    if (!string.IsNullOrEmpty(item.Leave))
                    {
                        foreach (string t in item.LeaveTrainees)
                        {
                            FillTraineeCallings(item, t, 1);
                        }
                    }
                    if (!string.IsNullOrEmpty(item.Absence))
                    {
                        foreach (string t in item.AbsenceTrainees)
                        {
                            FillTraineeCallings(item, t, 2);
                        }
                    }
                    if (!string.IsNullOrEmpty(item.Giving))
                    {
                        foreach (string t in item.GivingTrainees)
                        {
                            FillTraineeCallings(item, t, 3);
                        }
                    }
                    item.TraineesWithCallingState.Sort();
                }
            }
        }

        private void FillTraineeCallings(NameCallingModel calling, string traineeID, int callingState)
        {
            calling.TraineesWithCallingState.Add(_trainees.GetModel(traineeID));
            calling.TraineesWithCallingState.Last().CallingState = callingState;
        }

        public void OperateCalling(OperationType op, NameCallingModel calling)
        {
            if(op== OperationType.Update)
            {
                Calling.Update(calling);
            }
            else if(op== OperationType.Delete)
            {
                Calling.Del(calling.NameCallingID);
            }
        }
    }
}
