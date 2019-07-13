using Common;
using DancingTrainingManagement.UICore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace DancingTrainingManagement.Components.TeachingManagement.TeacherManagement
{
    class TeacherViewModel : ViewModelBase
    {
        private Color _classBkg;

        public Color TeacherBkg
        {
            get { return _classBkg; }
            set
            {
                _classBkg = value;
                RaisePropertyChanged("TeacherBkg");
            }
        }

        private Color _classFore;

        public Color TeacherFore
        {
            get { return _classFore; }
            set
            {
                _classFore = value;
                RaisePropertyChanged("TeacherFore");
            }
        }

        private string _className;

        public string TeacherName
        {
            get { return _className; }
            set
            {
                _className = value;
                RaisePropertyChanged("TeacherName");
            }
        }

        private DelegateCommand _classClicked;

        public DelegateCommand TeacherClicked
        {
            get
            {
                _classClicked = _classClicked ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        TeacherBkg = GlobalVariables.MainColor;
                        TeacherFore = GlobalVariables.MainBackColor;
                        TeacherOperatedEvent?.Invoke(OperationType.Select, _model);
                    }));
                return _classClicked;
            }
            set
            {
                _classClicked = value;
                RaisePropertyChanged("TeacherClicked");
            }
        }

        public string TeacherID
        {
            get
            {
                return _model.TeacherID;
            }
        }

        public delegate void TeacherOperated(OperationType operation, TeacherModel model);
        public event TeacherOperated TeacherOperatedEvent;

        private TeacherModel _model;

        public TeacherViewModel(TeacherModel model)
        {
            _model = model;
            TeacherName = model.TeacherName;
            TeacherFore = GlobalVariables.MainColor;
            TeacherBkg = GlobalVariables.MainBackColor;
        }

        public void ChangeToUnselected()
        {
            TeacherFore = GlobalVariables.MainColor;
            TeacherBkg = GlobalVariables.MainBackColor;
        }

        public void ChangeToSelected()
        {
            TeacherBkg = GlobalVariables.MainColor;
            TeacherFore = GlobalVariables.MainBackColor;
        }
    }
}
