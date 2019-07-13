using Common;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.TeachingManagement.BlockTeaching
{
    /// <summary>
    /// 常规班学员管理类
    /// 该类维护当前常规班的学员列表，不对数据库进行修改
    /// </summary>
    public class BlockTraineeBussiness
    {
        public delegate void LoadTrainees(List<TraineeModel> trainees);
        /// <summary>
        /// 重新获取了常规班学员，通知界面刷新
        /// </summary>
        public event LoadTrainees LoadTraineesEvnet;
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

        public BlockTraineeBussiness(TraineeInfo traineeDAL)
        {
            _dal = traineeDAL;
            TraineeOperation = new TraineeOperationBussiness();
            TraineeOperation.TraineeChangedEvent += OnTraineeChanged;
        }

        private void OnTraineeChanged(OperationType operation, TraineeModel trainee)
        {
            switch (operation)
            {
                case OperationType.Add:
                    if (trainee.IsNew)//新建学员
                    {
                        string traineeID;
                        _dal.Add(trainee, out traineeID);
                        trainee.TraineeID = traineeID;
                    }
                    _dal.AddTraineeForBlock(trainee.CurrentBlockID, trainee.TraineeID);
                    OverdueChangedEvent?.Invoke();
                    break;
                case OperationType.Update:
                    _dal.UpdateForBlock(trainee, _currentClassID);
                    if (trainee.CurrentBlockID != _currentClassID)
                    {
                        operation = OperationType.Delete;
                    }
                    break;
            }
            TraineeChangedEvent?.Invoke(operation, trainee);
        }

        /// <summary>
        /// 重新获取独舞学员列表
        /// </summary>
        /// <param name="classID">独舞班级ID</param>
        public void GetTrainees(string classID)
        {
            _currentClassID = classID;
            _trainees = _dal.GetTraineesForBlock(classID);
            if (_trainees != null)
                _trainees.Sort();
            LoadTraineesEvnet?.Invoke(_trainees);
        }

        public List<TraineeModel> GetTraineesSync(string classID)
        {
            _currentClassID = classID;
            _trainees = _dal.GetTraineesForBlock(classID);
            if (_trainees != null)
                _trainees.Sort();
            return _trainees;
        }

        public void GetTraineesNotPay(string classID)
        {
            _currentClassID = classID;
            _trainees = _dal.GetTraineesForBlock(classID);
            List<string> traineesPaid = _dal.GetListInPayment(classID);
            if (_trainees != null)
            {
                if (traineesPaid != null)
                    _trainees = _trainees.Where(t => !traineesPaid.Contains(t.TraineeID)).ToList();
                _trainees.Sort();
            }

            LoadTraineesEvnet?.Invoke(_trainees);
        }

        /// <summary>
        /// 界面请求对学员进行编辑
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="trainee"></param>
        public void OperateTrainee(OperationType operation, TraineeModel trainee)
        {
            trainee = trainee ?? new TraineeModel();
            trainee.CurrentBlockID = _currentClassID;
            if (operation == OperationType.Delete)
            {
                _dal.DeleteTraineeForBlock(trainee.CurrentBlockID, trainee.TraineeID);
                TraineeChangedEvent?.Invoke(OperationType.Delete, trainee);
                return;
            }
            if (operation == OperationType.Add)
            {
                if (string.IsNullOrEmpty(_currentClassID))
                    return;
            }
            TraineeOperation.Enable(operation, trainee);
        }
    }
}
