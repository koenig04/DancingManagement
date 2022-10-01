using BLL.ClassManagement;
using Model.DancingClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.StatisticManagement.TeachingStatistic
{
    public class TeachingCountByClass
    {
        public string ClassID { get; set; }
        public string ClassName { get; set; }
        public Dictionary<DateTime, int> Info { get; set; }

        public TeachingCountByClass(List<ClassInfoForTeacherModel> details, string classID, RegularClassManagement regularClass)
        {
            ClassID = classID;
            ClassName = regularClass.GetClassNameInHis(ClassID, (from d in details where d.ClassID == classID select d).First().ClassDate);
            var classInfo = (from d in details
                             where d.ClassID == classID
                             select d).ToList();
            List<DateTime> classDate = (from d in classInfo
                                        orderby d.ClassDate
                                        select d.ClassDate).Distinct().ToList();

            Info = new Dictionary<DateTime, int>();
            foreach(var date in classDate)
            {
                int count = (from d in classInfo
                             where d.ClassDate == date
                             select d).Count();
                Info.Add(date, count);
            }
        }
    }
}
