using DAL;
using Model;
using Model.DancingClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.NameCallingManagement
{
    public class NameCallingBussiness
    {
        public delegate void OverdueChanged();
        public event OverdueChanged OverdueChangedEvent;

        private NameCallingInfo _dal = new NameCallingInfo();

        public NameCallingBussiness()
        {

        }

        public void Add(NameCallingModel calling)
        {
            _dal.Add(calling);

            //check giving
            List<ClassGivingInfoModel> givings = _dal.GetGivingList(calling.ClassID);
            if (givings != null)
            {
                //do giving
                foreach (string traineeID in calling.PresenceTrainees)
                {
                    List<DateTime> namecallings = _dal.GetListAsGivingInfo(traineeID, givings[0]);
                    if (namecallings.Count % givings[0].GivingInterval == 0)
                    {
                        //add giving
                        TraineeModel trainee = _dal.GetTraineeModel(traineeID);
                        int currentRemain = trainee.RemainRegularCount + 1;

                        string detailID;
                        _dal.AddGivingDetail(new ClassGivingDetailModel()
                        {
                            ClassGivingID = givings[0].ClassGivingID,
                            GivingDate = calling.ClassDate,
                            TraineeID = traineeID
                        }, out detailID);

                        if (currentRemain > 0 && trainee.RemainRegularCount <= 0)
                        {
                            _dal.IncreaseRemainCountAndOverdue(traineeID, currentRemain);
                        }
                        else
                        {
                            _dal.UpdateRemainCount(traineeID, currentRemain);
                        }
                    }
                }
            }


            if (calling.ClassType == 0)
            {
                OverdueChangedEvent?.Invoke();
            }
        }

        public int GetClassCountForTeacher(string teacherID, DateTime startDate, DateTime endDate)
        {
            return _dal.GetClassCountForTeacher(teacherID, startDate, endDate);
        }

        public List<ClassInfoForTeacherModel> GetClassInfoForTeacher(string teacherID, DateTime startDate, DateTime endDate)
        {
            return _dal.GetClassInfoForTeacher(teacherID, startDate, endDate);
        }

        public List<NameCallingModel> GetListByTrainee(string traineeID, DateTime startDate, DateTime endDate)
        {
            return _dal.GetListByTrainee(traineeID, startDate, endDate);
        }

        public List<NameCallingModel> GetListForCurrentTerm(string traineeID)
        {
            return _dal.GetListForCurrentTerm(traineeID);
        }

        public List<NameCallingModel> GetListForPreviousTerm(string traineeID)
        {
            return _dal.GetListForPreviousTerm(traineeID);
        }

        public List<NameCallingModel> GetListByDate(DateTime callingDate)
        {
            return _dal.GetListByDate(callingDate);
        }

        public void Del(string callingID)
        {
            _dal.Del(callingID);
            OverdueChangedEvent?.Invoke();
        }

        public void Update(NameCallingModel model)
        {
            _dal.Update(model);
            OverdueChangedEvent?.Invoke();
        }

        public List<NameCallingModel> GetListByClass(string classID, DateTime startDate, DateTime endDate, bool isGeneral)
        {
            return _dal.GetListByClass(classID, startDate, endDate, isGeneral);
        }

        public List<ClassGivingDetailModel> GetGivingDetail(string traineeID)
        {
            return _dal.GetGivingDetail(traineeID);
        }

        public TraineeModel GetTrainee(string traineeID)
        {
            return _dal.GetTraineeModel(traineeID);
        }

        public List<NameCallingModel> GetListByPresentCount(string traineeID, int presentCount)
        {
            return _dal.GetListByPresentCount(traineeID, presentCount);
        }

        public List<ClassGivingDetailModel> GetGivingDetailByTraineeAndCount(string traineeID, int givingCount)
        {
            return _dal.GetGivingDetailByTraineeAndCount(traineeID, givingCount);
        }
    }
}
