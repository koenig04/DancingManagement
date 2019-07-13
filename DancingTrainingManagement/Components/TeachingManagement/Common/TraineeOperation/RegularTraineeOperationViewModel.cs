using BLL.TraineeManagement;
using System;
using System.Linq;
using Model;
using Common;
using DancingTrainingManagement.UICore;
using System.Text.RegularExpressions;
using System.Windows;
using BLL.ClassManagement;
using BLL.TeachingManagement;
using System.Collections.Generic;

namespace DancingTrainingManagement.Components.TeachingManagement.Common.TraineeOperation
{
    class RegularTraineeOperationViewModel : TraineeOperationViewModel
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
                            Trainee.RegularClassID = _regularClasses.RegularClassCollection.Where(c => c.ClassName == SelectedClass).First().ClassID;
                            Trainee.RegularClassName = SelectedClass;
                            Trainee.InitialRemainRegularCount = int.Parse(InitialRemain);

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
        private RegularClassManagement _regularClasses;
        public RegularTraineeOperationViewModel(TraineeManagementBussiness trainees, TraineeOperationBussiness traineeOperation,
            RegularClassManagement regularClasses)
            : base(trainees)
        {
            _traineeOperation = traineeOperation;
            _traineeOperation.OperationEnableEvent += Enable;

            _regularClasses = regularClasses;
            _regularClasses.RegularClassCollection.ForEach(c => ClassCollection.Add(c.ClassName));
            _regularClasses.RegularClassChangedEvent += (operation, regularClass, newIndex) =>
            {
                ClassCollection.Clear();
                _regularClasses.RegularClassCollection.ForEach(c => ClassCollection.Add(c.ClassName));
            };

            ClassComboWidth = 100;
            VisRemain = Visibility.Visible;
        }

        protected override void UpdateFuzzyResult(string value)
        {
            if (string.IsNullOrEmpty(value))
                return;
            (TraineeCollection.FuzzySearchForRegular(value) ??new List<TraineeModel>()).ForEach(s =>
            {

                PopupTraineeCollection.Add(new FuzzyTraineeViewModel(s, ClassType.Regular));
                PopupTraineeCollection.Last().TraineeSelectedEvent += OnTraineeSelected;

            });
        }

        public override void Enable(OperationType operation, TraineeModel trainee)
        {
            base.Enable(operation, trainee);
            SelectedClass = _regularClasses.RegularClassCollection.Where(c => c.ClassID == trainee.RegularClassID).First().ClassName;
        }

        protected override bool CheckValidity()
        {
            bool res = base.CheckValidity();
            if (string.IsNullOrEmpty(InitialRemain) || !Regex.IsMatch(InitialRemain, @"\d*"))
            {
                ErrVis = Visibility.Visible;
                res = res && false;
            }
            return res;
        }
    }
}
