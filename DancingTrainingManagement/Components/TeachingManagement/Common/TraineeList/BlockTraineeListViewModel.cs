using BLL.TeachingManagement.BlockTeaching;
using Common;
using DancingTrainingManagement.UICore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.TeachingManagement.Common.TraineeList
{
    class BlockTraineeListViewModel : TraineeListViewModel
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

        private BlockTraineeBussiness _bussiness;
        public BlockTraineeListViewModel(BlockTraineeBussiness bussiness)
        {
            _bussiness = bussiness;
            _bussiness.LoadTraineesEvnet += trainees =>
            {
                Trainees.Clear();
                if (trainees != null)
                {
                    trainees.ForEach(t =>
                    {
                        Trainees.Add(new TraineeViewModel(t,false));
                        Trainees.Last().OperateTraineeEvent += OnOperateTrainee;
                    });
                }
            };

            _bussiness.TraineeChangedEvent += (operation, trainee, newIndex) =>
            {
                switch (operation)
                {
                    case OperationType.Add:
                        Trainees.Add(new TraineeViewModel(trainee,false));
                        Trainees.Last().OperateTraineeEvent += OnOperateTrainee;
                        break;
                    case OperationType.Update:
                        Trainees.Where(t => t.TraineeID == trainee.TraineeIDForShown).First().TraineeName = trainee.TraineeName;
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
