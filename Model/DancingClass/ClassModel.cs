using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ClassModel
    {
        public string ClassID { get; set; }
        public virtual string ClassName { get; }
        public string TeacherID { get; set; }
        public string TeacherName { get; set; }
        public decimal CostPerTerm { get; set; }
        public int ClassType { get; set; }
        public int ClassSerial { get; set; }
    }
}
