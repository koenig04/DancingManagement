using Common;
using DancingTrainingManagement.UICore;
using Model;
using System;
using System.Windows;
using System.Windows.Media;

namespace DancingTrainingManagement.Components.TeachingManagement.Common.ClassList
{
    public class ClassViewModel : ViewModelBase
    {
        private Color _classBkg;

        public Color ClassBkg
        {
            get { return _classBkg; }
            set
            {
                _classBkg = value;
                RaisePropertyChanged("ClassBkg");
            }
        }

        private Color _classFore;

        public Color ClassFore
        {
            get { return _classFore; }
            set
            {
                _classFore = value;
                RaisePropertyChanged("ClassFore");
            }
        }

        private string _className;

        public string ClassName
        {
            get { return _className; }
            set
            {
                _className = value;
                RaisePropertyChanged("ClassName");
            }
        }

        private DelegateCommand _classClicked;

        public DelegateCommand ClassClicked
        {
            get
            {
                _classClicked = _classClicked ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        ClassBkg = GlobalVariables.MainColor;
                        ClassFore = GlobalVariables.MainBackColor;
                        VisOperation = Visibility.Visible;
                        ClassOperatedEvent?.Invoke(OperationType.Select, _model);
                    }));
                return _classClicked;
            }
            set
            {
                _classClicked = value;
                RaisePropertyChanged("ClassClicked");
            }
        }

        private DelegateCommand _classModified;

        public DelegateCommand ClassModified
        {
            get
            {
                _classModified = _classModified ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        ClassOperatedEvent?.Invoke(OperationType.Update, _model);
                    }));
                return _classModified;
            }
            set
            {
                _classModified = value;
                RaisePropertyChanged("ClassModified");
            }
        }

        private Visibility _visoperation;

        public Visibility VisOperation
        {
            get { return _visoperation; }
            set
            {
                _visoperation = value;
                RaisePropertyChanged("VisOperation");
            }
        }


        public string ClassID
        {
            get
            {
                return _model.ClassID;
            }
        }

        public delegate void ClassOperated(OperationType operation, ClassModel model);
        public event ClassOperated ClassOperatedEvent;

        private ClassModel _model;

        public ClassViewModel(ClassModel model)
        {
            _model = model;
            ClassName = model.ClassName;
            ClassFore = GlobalVariables.MainColor;
            ClassBkg = GlobalVariables.MainBackColor;
            VisOperation = Visibility.Hidden;
        }

        public void ChangeToUnselected()
        {
            ClassFore = GlobalVariables.MainColor;
            ClassBkg = GlobalVariables.MainBackColor;
            VisOperation = Visibility.Hidden;
        }
    }
}
