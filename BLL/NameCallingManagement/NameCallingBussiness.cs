using DAL;
using Model;
using Model.DancingClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.NameCallingManagement
{
    public class NameCallingBussiness
    {
        public delegate void OverdueChanged();
        public event OverdueChanged OverdueChangedEvent;

        private NameCallingInfo _dal = new NameCallingInfo();

        public NameCallingBussiness()
        {

        }

        public void Add(NameCallingModel calling)
        {
            _dal.Add(calling);
            if (calling.ClassType == 0)
            {
                OverdueChangedEvent?.Invoke();
            }
        }

        public int GetClassCountForTeacher(string teacherID, DateTime startDate, DateTime endDate)
        {
            return _dal.GetClassCountForTeacher(teacherID, startDate, endDate);
        }

        public List<ClassInfoForTeacherModel> GetClassInfoForTeacher(string teacherID, DateTime startDate, DateTime endDate)
        {
            return _dal.GetClassInfoForTeacher(teacherID, startDate, endDate);
        }

        public List<NameCallingModel> GetListByTrainee(string traineeID, DateTime startDate, DateTime endDate)
        {
            return _dal.GetListByTrainee(traineeID, startDate, endDate);
        }

        public List<NameCallingModel> GetListForCurrentTerm(string traineeID)
        {
            return _dal.GetListForCurrentTerm(traineeID);
        }

        public List<NameCallingModel> GetListForPreviousTerm(string traineeID)
        {
            return _dal.GetListForPreviousTerm(traineeID);
        }

        public List<NameCallingModel> GetListByDate(DateTime callingDate)
        {
            return _dal.GetListByDate(callingDate);
        }

        public void Del(string callingID)
        {
            _dal.Del(callingID);
            OverdueChangedEvent?.Invoke();
        }

        public void Update(NameCallingModel model)
        {
            _dal.Update(model);
            OverdueChangedEvent?.Invoke();
        }
    }
}
