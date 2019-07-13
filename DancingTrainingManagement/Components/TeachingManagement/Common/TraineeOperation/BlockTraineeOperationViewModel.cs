using BLL.ClassManagement;
using BLL.TeachingManagement;
using BLL.TraineeManagement;
using Common;
using DancingTrainingManagement.UICore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DancingTrainingManagement.Components.TeachingManagement.Common.TraineeOperation
{
    class BlockTraineeOperationViewModel : TraineeOperationViewModel
    {
        private DelegateCommand _confirm;

        public DelegateCommand Confirm
        {
            get
            {
                _confirm = _confirm ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        if (!CheckValidity())
                        {
                            return;
                        }
                        if (CheckTraineeName(IsNewTrainee(), TraineeName, Trainee.TraineeID))
                        {
                            Trainee.IsNew = IsNewTrainee();
                            Trainee.TraineeName = TraineeName;
                            Trainee.CurrentBlockID = _blockClasses.BlockClassCollection.Where(c => c.ClassName == SelectedClass).First().ClassID;

                            _traineeOperation.OperateTrainee(Operation, Trainee);
                            IsPopupOpen = false;
                            Vis = Visibility.Hidden;
                        }
                    }));
                return _confirm;
            }
            set
            {
                _confirm = value;
                RaisePropertyChanged("Confirm");
            }
        }

        private TraineeOperationBussiness _traineeOperation;
        private BlockClassManagement _blockClasses;
        public BlockTraineeOperationViewModel(TraineeManagementBussiness trainees, TraineeOperationBussiness traineeOperation,
            BlockClassManagement blockClasses)
            : base(trainees)
        {
            _traineeOperation = traineeOperation;
            _traineeOperation.OperationEnableEvent += Enable;

            _blockClasses = blockClasses;
            _blockClasses.BlockClassCollection.ForEach(c => ClassCollection.Add(c.ClassName));
            _blockClasses.BlockClassChangedEvent += (operation, regularClass, newIndex) =>
            {
                ClassCollection.Clear();
                _blockClasses.BlockClassCollection.ForEach(c => ClassCollection.Add(c.ClassName));
            };

            ClassComboWidth = 200;
            VisRemain = Visibility.Hidden;
        }

        protected override void UpdateFuzzyResult(string value)
        {
            if (string.IsNullOrEmpty(value))
                return;
            (TraineeCollection.FuzzySearchForBlock(value)??new List<TraineeModel>()).ForEach(s =>
            {

                PopupTraineeCollection.Add(new FuzzyTraineeViewModel(s, ClassType.Block));
                PopupTraineeCollection.Last().TraineeSelectedEvent += OnTraineeSelected;

            });
        }

        public override void Enable(OperationType operation, TraineeModel trainee)
        {
            base.Enable(operation, trainee);
            TraineeCollection.LoadBlockTraineesCannotBeChosen(trainee.CurrentBlockID);
            SelectedClass = _blockClasses.BlockClassCollection.Where(c => c.ClassID == trainee.CurrentBlockID).First().ClassName;
        }
    }
}
