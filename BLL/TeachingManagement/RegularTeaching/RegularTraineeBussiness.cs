using Common;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.TeachingManagement.RegularTeaching
{
    /// <summary>
    /// 常规班学员管理类
    /// 该类维护当前常规班的学员列表，不对数据库进行修改
    /// </summary>
    public class RegularTraineeBussiness
    {
        public delegate void LoadTrainees(List<TraineeModel> trainees);
        /// <summary>
        /// 重新获取了常规班学员，通知界面刷新
        /// </summary>
        public event LoadTrainees LoadTraineesEvent;
        public delegate void TraineeChanged(OperationType operation, TraineeModel trainee, int newIndex = -1);
        /// <summary>
        /// 学员信息已经改变，通知界面刷新
        /// </summary>
        public event TraineeChanged TraineeChangedEvent;

        public delegate void OverdueChanged();
        public event OverdueChanged OverdueChangedEvent;

        public TraineeOperationBussiness TraineeOperation { get; private set; }

        private TraineeInfo _dal;
        private List<TraineeModel> _trainees;
        private string _currentClassID;
        public RegularTraineeBussiness(TraineeInfo traineeDAL)
        {
            _dal = traineeDAL;
            TraineeOperation = new TraineeOperationBussiness();
            TraineeOperation.TraineeChangedEvent += OnTraineeChanged;
        }

        /// <summary>
        /// 重新获取常规学员列表
        /// </summary>
        /// <param name="classID">常规班级ID</param>
        public void GetTrainees(string classID)
        {
            _currentClassID = classID;
            _trainees = _dal.GetTraineesForRegular(classID);
            if (_trainees != null)
                _trainees.Sort();
            LoadTraineesEvent?.Invoke(_trainees);
        }

        public List<TraineeModel> GetTraineesSync(string classID)
        {
            _currentClassID = classID;
            _trainees = _dal.GetTraineesForRegularCallingList(classID);
            if (_trainees != null)
                _trainees.Sort();
            return _trainees;
        }

        //public void GetTraineeNames()

        /// <summary>
        /// 界面请求对学员进行编辑
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="trainee"></param>
        public void OperateTrainee(OperationType operation, TraineeModel trainee)
        {
            if(operation== OperationType.Delete||operation== OperationType.Reverse)
            {
                DeleteOrReverseTrainee(operation, trainee);
                OverdueChangedEvent?.Invoke();
                return;
            }
            if (operation == OperationType.Add)
            {
                if (string.IsNullOrEmpty(_currentClassID))
                    return;
                trainee = new TraineeModel();
                trainee.RegularClassID = _currentClassID;
            }

            TraineeOperation.Enable(operation, trainee);
        }

        /// <summary>
        /// 删除学员
        /// </summary>
        /// <param name="trainee"></param>
        private void DeleteOrReverseTrainee(OperationType operation, TraineeModel trainee)
        {
            if (operation == OperationType.Delete)
                trainee.State++;
            else if (operation == OperationType.Reverse)
                trainee.State--;
            _dal.Update(trainee);
            if (trainee.State == 2)
            {//彻底删除
                TraineeChangedEvent?.Invoke(OperationType.Delete, trainee);
            }
            else
            {//暂时删除或者恢复正常
                _trainees[_trainees.IndexOf(_trainees.Where(t => t.TraineeID == trainee.TraineeID).First())] = trainee;
                _trainees.Sort();
                int newIndex = _trainees.IndexOf(trainee);
                TraineeChangedEvent?.Invoke(OperationType.Update, trainee, newIndex);
            }
        }

        /// <summary>
        /// 学员信息已更改，更新界面，更新数据库
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="trainee"></param>
        public void OnTraineeChanged(OperationType operation, TraineeModel trainee)
        {
            if (operation == OperationType.Update)
            {
                _dal.Update(trainee);
                if (trainee.RegularClassID != _currentClassID)
                {
                    operation = OperationType.Delete;
                }
                OverdueChangedEvent?.Invoke();
            }
            else if (operation == OperationType.Add)
            {
                if(trainee.IsNew)
                {
                    string traineeID;
                    _dal.Add(trainee, out traineeID);
                    trainee.TraineeID = traineeID;
                }
                else
                {
                    _dal.AddTraineeForRegular(trainee.RegularClassID, trainee.TraineeID);
                }
                _trainees.Add(trainee);
                OverdueChangedEvent?.Invoke();
            }
            TraineeChangedEvent?.Invoke(operation, trainee);
        }
    }
}
