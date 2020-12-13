using BLL.CommonBussiness;
using DancingTrainingManagement.UICore;
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
                        TeacherName = _bussiness.ChangeTeacher(int.Parse(o.ToString()));
                    }));
                return _changeTeacher;
            }
            set
            {
                _changeTeacher = value;
                RaisePropertyChanged("ChangeTeacher");
            }
        }


        private TeacherSelecterBussiness _bussiness;
        public TeacherSelecterWithArrowViewModel()
        {
            _bussiness = new TeacherSelecterBussiness();
            TeacherName = _bussiness.GetCurrentTeacher();
        }
    }
}
