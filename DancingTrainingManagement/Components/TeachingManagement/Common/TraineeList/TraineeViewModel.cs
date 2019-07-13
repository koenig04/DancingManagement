using Common;
using DancingTrainingManagement.UICore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace DancingTrainingManagement.Components.TeachingManagement.Common.TraineeList
{
    class TraineeViewModel : ViewModelBase
    {
        private string _traineeID;

        public string TraineeID
        {
            get { return _traineeID; }
            set
            {
                _traineeID = value;
                RaisePropertyChanged("TraineeID");
            }
        }

        private string _traineeName;

        public string TraineeName
        {
            get { return _traineeName; }
            set
            {
                _traineeName = value;
                RaisePropertyChanged("TraineeName");
            }
        }

        private Visibility _reverseVis;

        public Visibility ReverseVis
        {
            get { return _reverseVis; }
            set
            {
                _reverseVis = value;
                RaisePropertyChanged("ReverseVis");
            }
        }

        private Visibility _modifyVis;

        public Visibility ModifyVis
        {
            get { return _modifyVis; }
            set
            {
                _modifyVis = value;
                RaisePropertyChanged("ModifyVis");
            }
        }

        private Visibility _deleteVis;

        public Visibility DeleteVis
        {
            get { return _deleteVis; }
            set
            {
                _deleteVis = value;
                RaisePropertyChanged("DeleteVis");
            }
        }


        private DelegateCommand _reverseTrainee;

        public DelegateCommand ReverseTrainee
        {
            get
            {
                _reverseTrainee = _reverseTrainee ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        OperateTraineeEvent?.Invoke(OperationType.Reverse, _trainee);
                    }));
                return _reverseTrainee;
            }
            set
            {
                _reverseTrainee = value;
                RaisePropertyChanged("ReverseTrainee");
            }
        }

        private DelegateCommand _modifyTrainee;

        public DelegateCommand ModifyTrainee
        {
            get
            {
                _modifyTrainee = _modifyTrainee ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        OperateTraineeEvent?.Invoke(OperationType.Update, _trainee);
                    }));
                return _modifyTrainee;
            }
            set
            {
                _modifyTrainee = value;
                RaisePropertyChanged("ModifyTrainee");
            }
        }

        private DelegateCommand _deleteTrainee;

        public DelegateCommand DeleteTrainee
        {
            get
            {
                _deleteTrainee = _deleteTrainee ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        OperateTraineeEvent?.Invoke(OperationType.Delete, _trainee);
                    }));
                return _deleteTrainee;
            }
            set
            {
                _deleteTrainee = value;
                RaisePropertyChanged("DeleteTrainee");
            }
        }

        private DelegateCommand _traineeClicked;

        public DelegateCommand TraineeClicked
        {
            get
            {
                _traineeClicked = _traineeClicked ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        OperateTraineeEvent?.Invoke(OperationType.Select, _trainee);
                        VisOperation = Visibility.Visible;
                    }));
                return _traineeClicked;
            }
            set
            {
                _traineeClicked = value;
                RaisePropertyChanged("TraineeClicked");
            }
        }


        private Visibility _visOperation;

        public Visibility VisOperation
        {
            get { return _visOperation; }
            set
            {
                _visOperation = value;
                RaisePropertyChanged("VisOperation");
            }
        }

        private Visibility _visRemain;

        public Visibility VisRemain
        {
            get { return _visRemain; }
            set
            {
                _visRemain = value;
                RaisePropertyChanged("VisRemain");
            }
        }


        private Color _stateColor;

        public Color StateColor
        {
            get { return _stateColor; }
            set
            {
                _stateColor = value;
                RaisePropertyChanged("StateColor");
            }
        }

        private string _remainClass;

        public string RemainClass
        {
            get { return _remainClass; }
            set
            {
                _remainClass = value;
                RaisePropertyChanged("RemainClass");
            }
        }

        private Color _remainColor;

        public Color RemainColor
        {
            get { return _remainColor; }
            set
            {
                _remainColor = value;
                RaisePropertyChanged("RemainColor");
            }
        }


        public delegate void OperateTrainee(OperationType operation, TraineeModel trainee);
        public event OperateTrainee OperateTraineeEvent;

        private TraineeModel _trainee;
        public TraineeViewModel(TraineeModel trainee, bool isRegular)
        {
            Render(trainee, isRegular);
            Vis = Visibility.Visible;
            VisOperation = Visibility.Collapsed;
        }

        //public void Change(TraineeModel trainee)
        //{
        //    Render(trainee, isRegular);
        //}

        private void Render(TraineeModel trainee, bool isRegular)
        {
            _trainee = trainee;
            TraineeID = trainee.TraineeIDForShown;
            TraineeName = trainee.TraineeName;
            VisRemain = isRegular ? Visibility.Visible : Visibility.Hidden;
            RemainClass = trainee.RemainRegularCount.ToString();
            RemainColor = trainee.RemainRegularCount > 0 ? GlobalVariables.IncomeColor : GlobalVariables.ExpenseColor;
            ChangeButtons(trainee.State);
        }

        private void ChangeButtons(int state)
        {
            if (state == (int)TraineeState.Normal)
            {
                ModifyVis = Visibility.Visible;
                DeleteVis = Visibility.Visible;
                ReverseVis = Visibility.Collapsed;
                StateColor = GlobalVariables.MainColor;
            }
            else if (state == (int)TraineeState.Suspend)
            {
                ModifyVis = Visibility.Collapsed;
                DeleteVis = Visibility.Visible;
                ReverseVis = Visibility.Visible;
                StateColor = GlobalVariables.DarkMainBackColor;
            }
            else
            {
                Vis = Visibility.Collapsed;
            }
        }

        public void ConvertToUnselected()
        {
            VisOperation = Visibility.Collapsed;
        }
    }
}
