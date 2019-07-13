using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.DancingClass
{
    public class ClassInfoForTeacherModel
    {
        public string NameCallingID { get; set; }
        public string ClassID { get; set; }
        public string ClassName { get; set; }
        public int ClassType { get; set; }
        public string TeacherID { get; set; }
        public DateTime ClassDate { get; set; }
    }
}
