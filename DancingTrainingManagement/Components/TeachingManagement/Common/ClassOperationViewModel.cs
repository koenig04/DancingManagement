using BLL.TeacherManagement;
using Common;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace DancingTrainingManagement.Components.TeachingManagement.Common
{
    class ClassOperationViewModel : ViewModelBase
    {
        private string _title;
        /// <summary>
        /// 班级编辑页面的标题
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        private string _selectedTeacher;

        public string SelectedTeacher
        {
            get { return _selectedTeacher; }
            set
            {
                _selectedTeacher = value;
                RaisePropertyChanged("SelectedTeacher");
            }
        }

        private ObservableCollection<string> _teacherCollection;

        public ObservableCollection<string> TeacherCollection
        {
            get { return _teacherCollection; }
            set
            {
                _teacherCollection = value;
                RaisePropertyChanged("TeacherCollection");
            }
        }

        private string _selectedClassType;

        public string SelectedClassType
        {
            get { return _selectedClassType; }
            set
            {
                _selectedClassType = value;
                RaisePropertyChanged("SelectedClassType");
            }
        }

        private ObservableCollection<string> _classTypeCollection;

        public ObservableCollection<string> ClassTypeCollection
        {
            get { return _classTypeCollection; }
            set
            {
                _classTypeCollection = value;
                RaisePropertyChanged("ClassTypeCollection");
            }
        }

        private string _costPerTerm;

        public string CostPerTerm
        {
            get { return _costPerTerm; }
            set
            {
                _costPerTerm = value;
                RaisePropertyChanged("CostPerTerm");
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

        private Visibility _errTeacher;

        public Visibility ErrTeacherVis
        {
            get { return _errTeacher; }
            set
            {
                _errTeacher = value;
                RaisePropertyChanged("ErrTeacherVis");
            }
        }

        private Visibility _errClassType;

        public Visibility ErrClassTypeVis
        {
            get { return _errClassType; }
            set
            {
                _errClassType = value;
                RaisePropertyChanged("ErrClassTypeVis");
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

        public ClassOperationViewModel()
        {
            TeacherCollection = new ObservableCollection<string>();
            TeacherManagementBussiness.Instance.Teachers.ForEach(t => TeacherCollection.Add(t.TeacherName));
            TeacherManagementBussiness.Instance.TeacherChangedEvent += (operation, teacher, previousTeacher) =>
            {
                if (operation == OperationType.Update)
                {
                    TeacherCollection.Remove(previousTeacher.TeacherName);
                }
                TeacherCollection.Add(teacher.TeacherName);
            };
            ClassTypeCollection = new ObservableCollection<string>();
            Vis = Visibility.Hidden;
        }

        protected virtual void HideErr()
        {
            ErrVis = Visibility.Hidden;
            ErrClassTypeVis = Visibility.Hidden;
            ErrTeacherVis = Visibility.Hidden;
        }
    }
}
