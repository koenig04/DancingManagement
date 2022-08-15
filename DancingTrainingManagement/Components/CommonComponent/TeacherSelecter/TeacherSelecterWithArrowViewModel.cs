using BLL.CommonBussiness;
using DancingTrainingManagement.UICore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.CommonComponent.TeacherSelecter
{
    class TeacherSelecterWithArrowViewModel : ViewModelBase
    {
        private string teacherName_;

        public string TeacherName
        {
            get { return teacherName_; }
            set
            {
                teacherName_ = value;
                RaisePropertyChanged("TeacherName");
            }
        }

        private DelegateCommand _changeTeacher;

        public DelegateCommand ChangeTeacher
        {
            get
            {
                _changeTeacher = _changeTeacher ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        currentTeacher_ = _bussiness.ChangeTeacher(int.Parse(o.ToString()));
                        TeacherName = currentTeacher_.TeacherName;
                        SelectedTeacherChangedEvent?.Invoke(currentTeacher_);
                    }));
                return _changeTeacher;
            }
            set
            {
                _changeTeacher = value;
                RaisePropertyChanged("ChangeTeacher");
            }
        }

        public delegate void SelectedTeacherChanged(TeacherModel teacher);
        public event SelectedTeacherChanged SelectedTeacherChangedEvent;

        private TeacherModel currentTeacher_;
        private TeacherSelecterBussiness _bussiness;
        public TeacherSelecterWithArrowViewModel()
        {
            _bussiness = new TeacherSelecterBussiness();
            TeacherName = _bussiness.GetCurrentTeacher();
        }
    }
}
