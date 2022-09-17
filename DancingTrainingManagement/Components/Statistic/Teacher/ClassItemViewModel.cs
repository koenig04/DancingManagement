using Common;
using DancingTrainingManagement.UICore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DancingTrainingManagement.Components.Statistic.Teacher
{
    class ClassItemViewModel : ViewModelBase
    {
        private string name_;

        public string ClassName
        {
            get { return name_; }
            set { name_ = value; RaisePropertyChanged("ClassName"); }
        }

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
                        ClassSelectedEvent?.Invoke(classID_);
                    }));
                return _classClicked;
            }
            set
            {
                _classClicked = value;
                RaisePropertyChanged("ClassClicked");
            }
        }

        public delegate void ClassSelected(string id);
        public event ClassSelected ClassSelectedEvent;

        private string classID_;
        public ClassItemViewModel(string classID,string className)
        {
            classID_ = classID;
            ClassName = className;
            ClassFore = GlobalVariables.MainColor;
            ClassBkg = GlobalVariables.DarkBackColor;
        }

        public void ChangeToUnselected(string id)
        {
            if (id != classID_)
            {
                ClassFore = GlobalVariables.MainColor;
                ClassBkg = GlobalVariables.DarkBackColor;
            }
        }
    }
}
