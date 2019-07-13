using BLL.ClassManagement;
using BLL.TeacherManagement;
using Common;
using Model.DancingClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.StatisticManagement.TeachingStatistic
{
    public class TeachingCountGroup
    {
        public TeachingStatisticTypeEnum StatisticType { get; private set; }
        public string TeacherID { get; private set; }
        public string TeacherName { get; private set; }
        public Dictionary<DateTime, int> ClassCountGroup { get; private set; }
        public Dictionary<DateTime, List<ClassInfoForTeacherModel>> DetailsGroup { get; private set; }

        public TeachingCountGroup(TeachingStatisticTypeEnum statisticType, string teacherID, List<ClassInfoForTeacherModel> details,
            DateTime startDate, DateTime endDate, bool isSortByMonth, RegularClassManagement regularClass, BlockClassManagement blocks)
        {
            StatisticType = statisticType;
            TeacherID = teacherID;
            TeacherName = TeacherManagementBussiness.Instance.Teachers.Where(t => t.TeacherID == teacherID).First().TeacherName;
            List<DateTime> spans = PublicMathods.GetDateSpan(startDate, endDate, isSortByMonth);

            ClassCountGroup = new Dictionary<DateTime, int>();
            DetailsGroup = new Dictionary<DateTime, List<ClassInfoForTeacherModel>>();
            foreach (DateTime span in spans)
            {
                ClassCountGroup.Add(span, details == null ?
                    0 :
                    details.Count(d => d.ClassDate >= span && d.ClassDate < (isSortByMonth ? span.AddMonths(1) : span.AddYears(1))));
                DetailsGroup.Add(span,
                    details == null ?
                    null :
                    details.Where(d => d.ClassDate >= span && d.ClassDate < (isSortByMonth ? span.AddMonths(1) : span.AddYears(1))).ToList());
                if (DetailsGroup[span] != null)
                {
                    foreach (ClassInfoForTeacherModel item in DetailsGroup[span])
                    {
                        item.ClassName = item.ClassType == 0 ?
                            regularClass.GetClassNameInHis(item.ClassID, item.ClassDate) :
                            blocks.BlockClassCollection.Where(b => b.ClassID == item.ClassID).First().ClassName;
                    }
                }
            }
        }
    }
}
