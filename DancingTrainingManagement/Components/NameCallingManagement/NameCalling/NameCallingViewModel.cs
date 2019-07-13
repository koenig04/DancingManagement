using BLL.ClassManagement;
using BLL.CommonBussiness;
using BLL.NameCallingManagement;
using BLL.TeacherManagement;
using BLL.TeachingManagement.BlockTeaching;
using BLL.TeachingManagement.RegularTeaching;
using Common;
using DancingTrainingManagement.Components.CommonComponent.ClassSelecter;
using DancingTrainingManagement.Components.NameCallingManagement.CommonComponent;
using DancingTrainingManagement.UICore;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace DancingTrainingManagement.Components.NameCallingManagement.NameCalling
{
    class NameCallingViewModel : ViewModelBase
    {
        private ClassSelecterViewModel _classSeleter;

        public ClassSelecterViewModel ClassSeleter
        {
            get { return _classSeleter; }
            set
            {
                _classSeleter = value;
                RaisePropertyChanged("ClassSeleter");
            }
        }

        private int _classCount;

        public int ClassCount
        {
            get { return _classCount; }
            set
            {
                _classCount = value;
                RaisePropertyChanged("ClassCount");
            }
        }

        private DelegateCommand _changeClassCount;

        public DelegateCommand ChangeClassCount
        {
            get
            {
                _changeClassCount = _changeClassCount ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        if (_isInput)
                            ClassCount = int.Parse(o.ToString());
                        else
                            ClassCount = 1;
                    }));
                return _changeClassCount;
            }
            set
            {
                _changeClassCount = value;
                RaisePropertyChanged("ChangeClassCount");
            }
        }

        private DateTime _classDate = DateTime.Now;

        public DateTime ClassDate
        {
            get { return _classDate; }
            set
            {
                _classDate = value;
                RaisePropertyChanged("ClassDate");
            }
        }

        private NameCallingListViewModel _callingNames;

        public NameCallingListViewModel CallingNames
        {
            get { return _callingNames; }
            set
            {
                _callingNames = value;
                RaisePropertyChanged("CallingNames");
            }
        }

        private DelegateCommand _setAllPresence;

        public DelegateCommand SetAllPresence
        {
            get
            {
                _setAllPresence = _setAllPresence ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        CallingNames.SetAllPresence();
                    }));
                return _setAllPresence;
            }
            set
            {
                _setAllPresence = value;
                RaisePropertyChanged("SetAllPresence");
            }
        }

        private Visibility _visInput;

        public Visibility VisInput
        {
            get { return _visInput; }
            set
            {
                _visInput = value;
                RaisePropertyChanged("VisInput");
            }
        }

        private Visibility _visEdit;

        public Visibility VisEdit
        {
            get { return _visEdit; }
            set
            {
                _visEdit = value;
                RaisePropertyChanged("VisEdit");
            }
        }


        private DelegateCommand _confirm;

        public DelegateCommand Confirm
        {
            get
            {
                _confirm = _confirm ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        if (CheckValidity())
                        {
                            FillCalling();
                            for (int i = 0; i < ClassCount; i++)
                                _nameCalling.Add(_calling);
                            ShowInfoEvent?.Invoke(MessageType.NameCallingSussses);
                            ClearAll();
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

        private DelegateCommand _update;

        public DelegateCommand Update
        {
            get
            {
                _update = _update ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        if (CheckValidity())
                        {
                            FillCalling();
                            NameCallingChangedEvent?.Invoke(OperationType.Update, _calling);
                            ClearAll();
                        }
                    }));
                return _update;
            }
            set
            {
                _update = value;
                RaisePropertyChanged("Update");
            }
        }

        private DelegateCommand _del;

        public DelegateCommand Delete
        {
            get
            {
                _del = _del ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        if (CheckValidity())
                        {
                            ClearAll();
                            NameCallingChangedEvent?.Invoke(OperationType.Delete, _calling);
                        }
                    }));
                return _del;
            }
            set
            {
                _del = value;
                RaisePropertyChanged("Delete");
            }
        }


        public delegate void ErrOccured(MessageType msg, MessageLevel level = MessageLevel.Warning);
        public event ErrOccured ErrOccuredEvent;
        public delegate void ShowInfo(MessageType msg, MessageLevel level = MessageLevel.Info);
        public event ShowInfo ShowInfoEvent;
        public delegate void NameCallingChanged(OperationType op, NameCallingModel calling);
        public event NameCallingChanged NameCallingChangedEvent;

        private NameCallingBussiness _nameCalling;
        private NameCallingModel _calling;
        private bool _isInput;

        public NameCallingViewModel(NameCallingBussiness nameCalling, ClassSelecterBussiness classSelecter, bool isInput = true)
        {
            _nameCalling = nameCalling;
            _calling = new NameCallingModel();

            ClassCount = -1;
            CallingNames = new NameCallingListViewModel();
            ClassSeleter = new ClassSelecterViewModel(classSelecter, true, false);
            ClassSeleter.TraineesChangedEvent += CallingNames.OnLoadTrainees;

            VisInput = isInput ? Visibility.Visible : Visibility.Collapsed;
            VisEdit = isInput ? Visibility.Collapsed : Visibility.Visible;
            _isInput = isInput;
        }

        public void FillCalling()
        {
            _calling.ClassDate = ClassDate;
            _calling.ClassType = ClassSeleter.ClassType;
            _calling.ClassID = ClassSeleter.GetCurrentClassID();
            _calling.TeacherID = ClassSeleter.GetCurrentTeacherID();
            CallingNames.GetCallingNames(ref _calling);
           
        }

        public void ClearAll()
        {
            ClassSeleter.ClearAll();
            CallingNames.ClearAll();
            ClassCount = 0;
        }

        public void LoadByCalling(NameCallingModel model)
        {
            _calling = model;
            ClassSeleter.LoadByCalling(model);
            ClassDate = model.ClassDate;
            ClassCount = 1;
            CallingNames.LoadTraineesWithState(model.TraineesWithCallingState);
        }

        private bool CheckValidity()
        {
            if (ClassSeleter.ClassType < 0)
            {
                return RaiseErrOccuredEvent(MessageType.ClassTypeErr);
            }
            if (string.IsNullOrEmpty(ClassSeleter.SelectedClass))
            {
                return RaiseErrOccuredEvent(MessageType.ClassNameErr);
            }
            if (string.IsNullOrEmpty(ClassSeleter.SelectedTeacher))
            {
                return RaiseErrOccuredEvent(MessageType.TeacherErr);
            }
            if (ClassCount < 1)
            {
                return RaiseErrOccuredEvent(MessageType.ClassCountErr);
            }
            if (ClassDate.Year < 2000)
            {
                return RaiseErrOccuredEvent(MessageType.ClassDateErr);
            }
            if(!CallingNames.CheckValidity())
            {
                return RaiseErrOccuredEvent(MessageType.NameCallingNoCallErr);
            }
            return true;
        }

        private bool RaiseErrOccuredEvent(MessageType msg)
        {
            ErrOccuredEvent?.Invoke(msg, MessageLevel.Warning);
            return false;
        }
    }
}
