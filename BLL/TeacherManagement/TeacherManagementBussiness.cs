using Common;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.TeacherManagement
{
    /// <summary>
    /// 教师管理类
    /// 教师名单相对比较稳定，所以只提供了Load功能
    /// </summary>
    public class TeacherManagementBussiness
    {
        public static TeacherManagementBussiness Instance = new TeacherManagementBussiness();
        public List<TeacherModel> Teachers { get; private set; }

        public delegate void TeacherChanged(OperationType operation, TeacherModel teacher, TeacherModel previousTeacher = null);
        public event TeacherChanged TeacherChangedEvent;
        private UserInfo _dal = new UserInfo();

        public TeacherManagementBussiness()
        {
            Teachers = _dal.GetTeachers();
        }

        public void AddTeacher(TeacherModel teacher)
        {
            string teacherID;
            _dal.AddTeacher(teacher, out teacherID);
            teacher.TeacherID = teacherID;
            Teachers.Add(teacher);
            TeacherChangedEvent?.Invoke(OperationType.Add, teacher);
        }

        public void UpdateTeacher(TeacherModel teacher)
        {
            _dal.UpdateTeacher(teacher);
            TeacherChangedEvent?.Invoke(OperationType.Update, teacher, Teachers.Where(t => t.TeacherID == teacher.TeacherID).First());
            Teachers[Teachers.IndexOf(Teachers.Where(t => t.TeacherID == teacher.TeacherID).First())] = teacher;
        }
    }
}
