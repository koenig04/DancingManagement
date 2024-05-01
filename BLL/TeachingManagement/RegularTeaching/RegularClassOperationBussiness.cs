using BLL.ClassManagement;
using Common;
using DAL;
using Model;
using Model.DancingClass;
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
        public delegate void OverdueChanged();
        public event OverdueChanged OverdueChangedEvent;


        private RegularClassManagement _bussiness;
        private ClassGivingInfoDAL _givingDAL = new ClassGivingInfoDAL();
        private ClassGivingDetailDAL _givingDetailDAL = new ClassGivingDetailDAL();
        private TraineeInfo _traineeDAL;

        public RegularClassOperationBussiness(RegularClassManagement regularClass, TraineeInfo traineeDAL)
        {
            _bussiness = regularClass;
            _traineeDAL = traineeDAL;
        }

        public void Enable(OperationType operation, RegularClassModel model)
        {
            OperationEnableEnvet?.Invoke(operation, model);
        }

        public void OperateRegularClass(OperationType operation, RegularClassModel model)
        {
            _bussiness.OperateClassInfo(operation, model);
        }

        public void AddNewGiving(ClassGivingInfoModel model)
        {
            string id;
            _givingDAL.Add(model, out id);
            model.ClassGivingID = id;
            AddGivingDetail(model);
            OverdueChangedEvent?.Invoke();
        }

        private void AddGivingDetail(ClassGivingInfoModel model)
        {
            //get all the trainees
            List<TraineeModel> trainees = _traineeDAL.GetTraineesForRegular(model.ClassID);

            string detailID;
            int givingCount = 0;

            foreach (TraineeModel t in trainees)
            {
                givingCount = 0;
                //get namecalling info, then add giving detail
                List<DateTime> namecallings = _givingDetailDAL.GetListAsGivingInfo(t.TraineeID, model);
                if (namecallings != null)
                {
                    for (int i = 0; i < namecallings.Count; i++)
                    {
                        if ((i + 1) % model.GivingInterval == 0)
                        {
                            _givingDetailDAL.Add(new ClassGivingDetailModel()
                            {
                                ClassGivingID = model.ClassGivingID,
                                GivingDate = namecallings[i],
                                TraineeID = t.TraineeID
                            }, out detailID);
                            givingCount++;
                        }
                    }
                }
                //update remain class count
                int currentRemain = t.RemainRegularCount + givingCount;
                if (currentRemain > 0 && t.RemainRegularCount <= 0)
                {
                    _traineeDAL.IncreaseRemainCountAndOverdue(t.TraineeID, currentRemain);
                }
                else
                {
                    _traineeDAL.UpdateRemainCount(t.TraineeID, currentRemain);
                }
            }
        }

        public List<ClassGivingInfoModel> GetGiving(string classID)
        {
            return _givingDAL.GetList(classID);
        }

        public void UpdateGiving(ClassGivingInfoModel model)
        {
            _givingDAL.Update(model);
        }

        public void DelGiving(ClassGivingInfoModel info)
        {
            _givingDAL.Del(info.ClassGivingID);
            DelGivingDetail(info);
            OverdueChangedEvent?.Invoke();
        }

        private void DelGivingDetail(ClassGivingInfoModel info)
        {
            //get giving detail
            List<ClassGivingDetailModel> details = _givingDetailDAL.GetList(info.ClassGivingID);

            if (details != null)
            {
                List<string> trainees = (from d in details
                                         select d.TraineeID).Distinct().ToList();

                foreach (string traineeID in trainees)
                {
                    int givingCount = (from d in details
                                       where d.TraineeID == traineeID
                                       select d).Count();
                    TraineeModel trainee = _traineeDAL.GetModel(traineeID);
                    int currentRemain = trainee.RemainRegularCount - givingCount;
                    if (trainee.RemainRegularCount > 0 && currentRemain <= 0)
                    {
                        //get namecalling
                        List<DateTime> namecallings = _givingDetailDAL.GetListAsGivingInfo(traineeID, info);
                        DateTime overdueDate = namecallings[namecallings.Count + currentRemain - 1];
                        _traineeDAL.ReduceRemaiCountAndOverdue(traineeID, currentRemain, info.ClassID, overdueDate);
                    }
                    else
                    {
                        _traineeDAL.UpdateRemainCount(traineeID, currentRemain);
                    }
                }

                _givingDetailDAL.Del(info.ClassGivingID);
            }
        }
    }
}
