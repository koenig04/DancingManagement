using BLL.TeacherManagement;
using Common;
using DancingTrainingManagement.UICore;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.TeachingManagement.TeacherManagement
{
    class TeacherListViewModel : ViewModelBase
    {
        private ObservableCollection<TeacherViewModel> _teachers;

        public ObservableCollection<TeacherViewModel> Teachers
        {
            get { return _teachers; }
            set
            {
                _teachers = value;
                RaisePropertyChanged("Teachers");
            }
        }

        private DelegateCommand _add;

        public DelegateCommand AddTeacher
        {
            get
            {
                _add = _add ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        TeacherModel newTeacher = new TeacherModel() { TeacherName = "新教师", TeacherID = "999" };
                        Teachers.Add(new TeacherViewModel(newTeacher));
                        Teachers.Last().ChangeToSelected();
                        _isAddingTeacher = true;
                        OnTeacherOperated(OperationType.Add, newTeacher);
                    }));
                return _add;
            }
            set
            {
                _add = value;
                RaisePropertyChanged("AddTeacher");
            }
        }

        public delegate void OperateTeacher(OperationType operation, TeacherModel teacher);
        public event OperateTeacher OperateTeacherEvent;

        private bool _isAddingTeacher = false;

        public TeacherListViewModel()
        {
            Teachers = new ObservableCollection<TeacherViewModel>();
            TeacherManagementBussiness.Instance.Teachers.ForEach(t =>
            {
                Teachers.Add(new TeacherViewModel(t));
                Teachers.Last().TeacherOperatedEvent += OnTeacherOperated;
            });
            TeacherManagementBussiness.Instance.TeacherChangedEvent += (operation, teacher, previousTeacher) =>
            {
                if (operation == OperationType.Update)
                {
                    Teachers.Remove(Teachers.Where(t => t.TeacherID == previousTeacher.TeacherID).First());
                }
                if(operation== OperationType.Add)
                {
                    Teachers.RemoveAt(Teachers.Count - 1);
                }
                Teachers.Add(new TeacherViewModel(teacher));
                Teachers.Last().TeacherOperatedEvent += OnTeacherOperated;
            };
        }

        private void OnTeacherOperated(OperationType operation, TeacherModel model)
        {
            if (Teachers.Where(t => t.TeacherID == "999").FirstOrDefault() != null && !_isAddingTeacher)
            {
                Teachers.RemoveAt(Teachers.Count - 1);
            }
            if(_isAddingTeacher)
            {
                _isAddingTeacher = false;
            }
            foreach (TeacherViewModel item in Teachers.Where(c => c.TeacherID != model.TeacherID))
            {
                item.ChangeToUnselected();
            }
            OperateTeacherEvent?.Invoke(operation, model);
        }
    }
}
