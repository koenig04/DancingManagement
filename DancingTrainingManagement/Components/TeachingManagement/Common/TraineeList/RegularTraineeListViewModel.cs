using BLL.TeachingManagement.RegularTeaching;
using Common;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace DancingTrainingManagement.Components.TeachingManagement.Common.TraineeList
{
    class RegularTraineeListViewModel : TraineeListViewModel
    {
        private DelegateCommand _addTrainee;

        public DelegateCommand AddTrainee
        {
            get
            {
                _addTrainee = _addTrainee ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        _bussiness.OperateTrainee(OperationType.Add, null);
                    }));
                return _addTrainee;
            }
            set
            {
                _addTrainee = value;
                RaisePropertyChanged("AddTrainee");
            }
        }

        private RegularTraineeBussiness _bussiness;
        public RegularTraineeListViewModel(RegularTraineeBussiness bussiness) : base()
        {
            _bussiness = bussiness;
            _bussiness.LoadTraineesEvent += trainees =>
            {
                Trainees.Clear();
                if (trainees != null)
                {
                    trainees.ForEach(t =>
                    {
                        Trainees.Add(new TraineeViewModel(t, true));
                        Trainees.Last().OperateTraineeEvent += OnOperateTrainee;
                    });
                }
            };
            _bussiness.TraineeChangedEvent += (operation, trainee, newIndex) =>
            {
                switch (operation)
                {
                    case OperationType.Add:
                        Trainees.Add(new TraineeViewModel(trainee, true));
                        Trainees.Last().OperateTraineeEvent += OnOperateTrainee;
                        break;
                    case OperationType.Update:
                        if (newIndex == -1)
                        {//只是更新信息
                            Trainees.Where(t => t.TraineeID == trainee.TraineeIDForShown).First().TraineeName = trainee.TraineeName;
                        }
                        else
                        {//删除或者恢复
                            Trainees.Remove(Trainees.Where(t => t.TraineeID == trainee.TraineeIDForShown).First());
                            Trainees.Insert(newIndex, new TraineeViewModel(trainee, true));
                            Trainees[newIndex].OperateTraineeEvent += OnOperateTrainee;
                        }
                        break;
                    case OperationType.Delete:
                        Trainees.Remove(Trainees.Where(t => t.TraineeID == trainee.TraineeIDForShown).First());
                        break;
                }
            };
        }

        private void OnOperateTrainee(OperationType operation, TraineeModel trainee)
        {
            if (operation == OperationType.Select)
            {
                foreach (TraineeViewModel item in Trainees.Where(t => t.TraineeID != trainee.TraineeIDForShown))
                {
                    item.ConvertToUnselected();
                }
                return;
            }
            _bussiness.OperateTrainee(operation, trainee);
        }
    }
}
