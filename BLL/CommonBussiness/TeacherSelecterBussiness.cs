using BLL.TeacherManagement;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.CommonBussiness
{
    public class TeacherSelecterBussiness
    {
        public delegate void SelectedTeacherChanged(string teacherID);
        public event SelectedTeacherChanged SelectedTeacherChangedEvent;

        private int currentTeacherIndex_;

        public TeacherSelecterBussiness()
        {
            currentTeacherIndex_ = 0;
        }

        public string GetCurrentTeacher()
        {
            return TeacherManagementBussiness.Instance.Teachers[currentTeacherIndex_].TeacherName;
        }

        public TeacherModel GetCurrentTeacherModel()
        {
            return TeacherManagementBussiness.Instance.Teachers[currentTeacherIndex_];
        }

        public TeacherModel ChangeTeacher(int changing)
        {
            currentTeacherIndex_ += changing;
            if (currentTeacherIndex_ < 0)
            {
                currentTeacherIndex_ = TeacherManagementBussiness.Instance.Teachers.Count - 1;
            }
            else if (currentTeacherIndex_ == TeacherManagementBussiness.Instance.Teachers.Count)
            {
                currentTeacherIndex_ = 0;
            }

            SelectedTeacherChangedEvent?.Invoke(TeacherManagementBussiness.Instance.Teachers[currentTeacherIndex_].TeacherID);
            return TeacherManagementBussiness.Instance.Teachers[currentTeacherIndex_];
        }
    }
}
