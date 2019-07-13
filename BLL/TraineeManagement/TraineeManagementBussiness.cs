using Common;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.TraineeManagement
{
    /// <summary>
    /// 学员管理类
    /// 该类存储了所有的学员列表，用于学员的模糊匹配，不对数据库中的学员信息进行改写。
    /// 当外部发生了学员的添加和修改，都要通知到此类
    /// </summary>
    public class TraineeManagementBussiness
    {
        public List<TraineeModel> Trainees
        {
            get
            {
                return _trinees;
            }
        }

        public delegate void TraineeChanged();
        public event TraineeChanged TraineeChangedEvent;

        private List<TraineeModel> _trinees;
        private List<string> _blockTrainnesCannotBeChosen;

        public TraineeInfo Dal { get; private set; }

        public TraineeManagementBussiness()
        {
            Dal = new TraineeInfo();
            _trinees = Dal.GetList();
            if (_trinees == null)
            {
                _trinees = new List<TraineeModel>();
            }
            _blockTrainnesCannotBeChosen = new List<string>();
        }

        /// <summary>
        /// 在列表中添加学员
        /// </summary>
        /// <param name="trainee"></param>
        public void AddTrainee(TraineeModel trainee)
        {
            _trinees.Add(trainee);
            if (!string.IsNullOrEmpty(trainee.CurrentBlockID))
            {
                _blockTrainnesCannotBeChosen.Add(trainee.TraineeID);
            }
            TraineeChangedEvent?.Invoke();
        }

        /// <summary>
        /// 在列表中修改学员
        /// </summary>
        /// <param name="trainee"></param>
        public void UpdateTrainee(TraineeModel trainee)
        {
            _trinees[_trinees.IndexOf(_trinees.Where(t => t.TraineeID == trainee.TraineeID).First())] = trainee;
            TraineeChangedEvent?.Invoke();
        }

        /// <summary>
        /// 为添加常规班学员进行模糊查找
        /// 只查找没有常规班的学员
        /// </summary>
        /// <param name="fuzzyStr"></param>
        /// <returns></returns>
        public List<TraineeModel> FuzzySearchForRegular(string fuzzyStr)
        {
            if (_trinees == null)
            {
                return null;
            }
            else
            {
                return _trinees.Where(t => t.TraineeName.Contains(fuzzyStr) &&
                string.IsNullOrEmpty(t.RegularClassID) &&
                t.State != 2).ToList();
            }
        }

        /// <summary>
        /// 为添加独舞版学员进行模糊查找
        /// 只查找已经是常规班的学员
        /// </summary>
        /// <param name="fuzzyStr"></param>
        /// <returns></returns>
        public List<TraineeModel> FuzzySearchForBlock(string fuzzyStr)
        {
            if (_trinees == null)
            {
                return null;
            }
            else
            {
                return _trinees.Where(t => t.TraineeName.Contains(fuzzyStr) &&
                !string.IsNullOrEmpty(t.RegularClassID) &&
                t.State != 2 &&
                !_blockTrainnesCannotBeChosen.Contains(t.TraineeID)).ToList();
            }
        }

        public string SearchTraineeName(string traineeID)
        {
            return _trinees.Where(t => t.TraineeID == traineeID).FirstOrDefault().TraineeName;
        }

        public void LoadBlockTraineesCannotBeChosen(string blockID)
        {
            _blockTrainnesCannotBeChosen = Dal.GetBlockTraineeInSameYearAndSeason(blockID);
        }

        public bool CheckRepeateName(string name, string traineeID = null)
        {
            if (string.IsNullOrEmpty(traineeID))//add
            {
                if (_trinees == null || _trinees.Where(t => t.TraineeName == name).FirstOrDefault() == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (_trinees.Where(t => t.TraineeName == name && t.TraineeID != traineeID).FirstOrDefault() == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public TraineeModel GetModel(string traineeID)
        {
            return (TraineeModel)_trinees.Where(t => t.TraineeID == traineeID).First().Clone();
        }
    }
}
