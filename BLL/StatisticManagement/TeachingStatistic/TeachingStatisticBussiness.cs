using BLL.ClassManagement;
using BLL.NameCallingManagement;
using BLL.TeacherManagement;
using Common;
using DAL;
using Model.DancingClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.StatisticManagement.TeachingStatistic
{
    public class TeachingStatisticBussiness
    {
        public delegate void TeacherCountInfoChanged(List<TeachingCountGroup> teachingCountGroups, bool isSortedByMonth);
        public event TeacherCountInfoChanged TeacherCountInfoChangedEvent;
        public delegate void ShowDetails(List<ClassInfoForTeacherModel> details);
        public event ShowDetails ShowDetailsEvent;
        public delegate void IndividualTeacherCountInfoChanged(TeachingCountGroup info);
        public event IndividualTeacherCountInfoChanged IndividualTeacherCountInfoChangedEvent;
        public delegate void IndividualTeacherCountByClassChanged(List<TeachingCountByClass> info);
        public event IndividualTeacherCountByClassChanged IndividualTeacherCountByClassChangedEvent;

        private NameCallingBussiness _calling;
        private RegularClassManagement _regularClass;
        private BlockClassManagement _blocks;
        private List<TeachingCountGroup> _teachingCountGroups;
        public TeachingStatisticBussiness(NameCallingBussiness calling, RegularClassManagement regularClass, BlockClassManagement blocks)
        {
            _calling = calling;
            _regularClass = regularClass;
            _blocks = blocks;
            _teachingCountGroups = new List<TeachingCountGroup>();
        }

        public void SearchTeachingCount(DateTime startDate, DateTime endDate, bool isSortedByMonth)
        {
            _teachingCountGroups.Clear();
            List<string> teachers = (from t in TeacherManagementBussiness.Instance.Teachers
                                     select t.TeacherID).ToList();
            foreach (string teacherID in teachers)
            {
                _teachingCountGroups.Add(new TeachingCountGroup(TeachingStatisticTypeEnum.ClassCount,
                    teacherID,
                    _calling.GetClassInfoForTeacher(teacherID, startDate, endDate),
                    startDate,
                    endDate,
                    isSortedByMonth,
                    _regularClass,
                    _blocks));
            }

            TeacherCountInfoChangedEvent?.Invoke(_teachingCountGroups, isSortedByMonth);
        }

        public void GetDetails(string teacherID, DateTime summaryDate, bool isTotal)
        {
            List<ClassInfoForTeacherModel> res = new List<ClassInfoForTeacherModel>();
            if (isTotal)
            {
                foreach (List<ClassInfoForTeacherModel> lst in _teachingCountGroups.Where(t => t.TeacherID == teacherID).First().DetailsGroup.Values)
                {
                    res.AddRange(lst);
                }
            }
            else
            {
                res.AddRange(_teachingCountGroups.Where(t => t.TeacherID == teacherID).First().DetailsGroup[summaryDate]);
            }
            ShowDetailsEvent?.Invoke(res);
        }

        public void SearchTeachingCountForIndividualTeacher(DateTime startDate, DateTime endDate, string id)
        {
            IndividualTeacherCountInfoChangedEvent?.Invoke(new TeachingCountGroup(_calling.GetClassInfoForTeacher(id, startDate, endDate),
                id,
                startDate,
                endDate,
                _regularClass));
        }

        public void SearchTeachingCountByClassAndTeacher(DateTime startDate, DateTime endDate, string id)
        {
            var generalInfo = _calling.GetClassInfoForTeacher(id, startDate, endDate);
            if (generalInfo != null)
            {
                var classInfo = (from i in generalInfo
                                 select i.ClassID).Distinct().ToList();
                List<TeachingCountByClass> info = new List<TeachingCountByClass>();
                foreach (var i in classInfo)
                {
                    info.Add(new TeachingCountByClass(generalInfo, i, _regularClass));
                }
                IndividualTeacherCountByClassChangedEvent?.Invoke(info);
            }
        }
    }
}
