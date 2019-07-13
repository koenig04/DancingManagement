using BLL.TraineeManagement;
using Common;
using DancingTrainingManagement.Components.CommonComponent.Message;
using DancingTrainingManagement.UICore;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace DancingTrainingManagement.Components.TeachingManagement.Common.TraineeOperation
{
    class TraineeOperationViewModel : ViewModelBase
    {
        private MessageViewModel _msg;

        public MessageViewModel Msg
        {
            get { return _msg; }
            set
            {
                _msg = value;
                RaisePropertyChanged("Msg");
            }
        }

        private ObservableCollection<FuzzyTraineeViewModel> _popupTrainees;

        public ObservableCollection<FuzzyTraineeViewModel> PopupTraineeCollection
        {
            get { return _popupTrainees; }
            set
            {
                _popupTrainees = value;
                RaisePropertyChanged("PopupTraineeCollection");
            }
        }

        private int _popupHeight;

        public int PopupHeight
        {
            get { return _popupHeight; }
            set
            {
                _popupHeight = value;
                RaisePropertyChanged("PopupHeight");
            }
        }

        private string _traineeName;

        public string TraineeName
        {
            get { return _traineeName; }
            set
            {
                OnTraineeNameChanged(value);
                _traineeName = value;
                RaisePropertyChanged("TraineeName");
            }
        }

        private string _initalRemain;

        public string InitialRemain
        {
            get { return _initalRemain; }
            set
            {
                _initalRemain = value;
                RaisePropertyChanged("InitialRemain");
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

        private Visibility _errVis;

        public Visibility ErrVis
        {
            get { return _errVis; }
            set
            {
                _errVis = value;
                RaisePropertyChanged("ErrVis");
            }
        }

        private Visibility _errName;

        public Visibility ErrNameVis
        {
            get { return _errName; }
            set
            {
                _errName = value;
                RaisePropertyChanged("ErrNameVis");
            }
        }

        private Visibility _errClass;

        public Visibility ErrClassVis
        {
            get { return _errClass; }
            set
            {
                _errClass = value;
                RaisePropertyChanged("ErrClassVis");
            }
        }


        private bool _isClassEditable;

        public bool IsClassEditable
        {
            get { return _isClassEditable; }
            set
            {
                _isClassEditable = value;
                RaisePropertyChanged("IsClassEditable");
            }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        private bool _isPopupOpen;

        public bool IsPopupOpen
        {
            get { return _isPopupOpen; }
            set
            {
                _isPopupOpen = value;
                RaisePropertyChanged("IsPopupOpen");
            }
        }

        private DelegateCommand _traineeLostFocus;

        public DelegateCommand TraineeLostFocus
        {
            get
            {
                _traineeLostFocus = _traineeLostFocus ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        IsPopupOpen = false;
                    }));
                return _traineeLostFocus;
            }
            set
            {
                _traineeLostFocus = value;
                RaisePropertyChanged("TraineeLostFocus");
            }
        }

        private DelegateCommand _cancel;

        public DelegateCommand Cancel
        {
            get
            {
                _cancel = _cancel ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        IsPopupOpen = false;
                        Vis = Visibility.Hidden;
                    }));
                return _cancel;
            }
            set
            {
                _cancel = value;
                RaisePropertyChanged("Cancel");
            }
        }

        private string _selectedClass;

        public string SelectedClass
        {
            get { return _selectedClass; }
            set
            {
                _selectedClass = value;
                RaisePropertyChanged("SelectedClass");
            }
        }

        private ObservableCollection<string> _classCollection;

        public ObservableCollection<string> ClassCollection
        {
            get { return _classCollection; }
            set
            {
                _classCollection = value;
                RaisePropertyChanged("ClassCollection");
            }
        }

        private int _classComboWidth;

        public int ClassComboWidth
        {
            get { return _classComboWidth; }
            set
            {
                _classComboWidth = value;
                RaisePropertyChanged("ClassComboWidth");
            }
        }


        protected TraineeManagementBussiness TraineeCollection;

        protected TraineeModel Trainee;
        protected OperationType Operation;

        private const int _itemUnitHeight = 40;
        private const int _maxItemCount = 4;

        public TraineeOperationViewModel(TraineeManagementBussiness trainees)
        {
            TraineeCollection = trainees;
            ClassCollection = new ObservableCollection<string>();
            PopupTraineeCollection = new ObservableCollection<FuzzyTraineeViewModel>();
            Vis = Visibility.Hidden;
            Msg = new MessageViewModel(false);
            Msg.OnOperateEnableEvent(false, false);
        }

        protected bool CheckTraineeName(bool isNew, string traineeName, string traineeID)
        {
            if (!isNew || TraineeCollection.CheckRepeateName(traineeName, Operation == OperationType.Add ? null : traineeID))
            {
                return true;
            }
            else
            {
                Msg.Enable(MessageType.RepeateNameErr, MessageLevel.Warning);
                return false;
            }
        }

        public virtual void Enable(OperationType operation, TraineeModel trainee)
        {
            Trainee = trainee;
            Operation = operation;

            Title = (operation == OperationType.Add ? "增加" : "修改") + "学员";
            IsClassEditable = operation == OperationType.Add ? false : true;
            TraineeName = trainee.TraineeName;
            IsPopupOpen = false;
            InitialRemain = operation == OperationType.Add ? "0" : trainee.InitialRemainRegularCount.ToString();

            HideErr();
            Vis = Visibility.Visible;
        }

        protected virtual void UpdateFuzzyResult(string value) { }

        private void OnTraineeNameChanged(string value)
        {
            if (value == null || string.IsNullOrEmpty(value.Trim()))
            {
                IsPopupOpen = false;
                return;
            }

            PopupTraineeCollection.Clear();

            if (Operation == OperationType.Add)
            {
                UpdateFuzzyResult(value);

                PopupHeight = (PopupTraineeCollection.Count > _maxItemCount ? _maxItemCount : PopupTraineeCollection.Count) * _itemUnitHeight;
                IsPopupOpen = true;
            }
        }

        protected virtual void OnTraineeSelected(TraineeModel trainee)
        {
            TraineeName = trainee.TraineeName;
            Trainee.TraineeName = TraineeName;
            Trainee.TraineeID = trainee.TraineeID;
            Trainee.InitialRemainRegularCount = trainee.InitialRemainRegularCount;
        }

        private void HideErr()
        {
            ErrVis = Visibility.Hidden;
            ErrClassVis = Visibility.Hidden;
            ErrNameVis = Visibility.Hidden;
        }

        protected virtual bool CheckValidity()
        {
            bool res = true;
            HideErr();
            if (string.IsNullOrEmpty(TraineeName))
            {
                ErrNameVis = Visibility.Visible;
                res = res && false;
            }
            if (string.IsNullOrEmpty(SelectedClass))
            {
                ErrClassVis = Visibility.Visible;
                res = res && false;
            }
            return res;
        }

        protected bool IsNewTrainee()
        {
            if (Trainee.TraineeName == TraineeName)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
