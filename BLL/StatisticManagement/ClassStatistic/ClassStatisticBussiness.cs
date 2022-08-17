using BLL.ClassManagement;
using BLL.NameCallingManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.StatisticManagement.ClassStatistic
{
    public class ClassStatisticBussiness
    {
        public AttendenceBussiness Attendence { get; private set; }

        private RegularClassManagement regularClass_;

        public ClassStatisticBussiness(RegularClassManagement regularClass, NameCallingBussiness calling)
        {
            Attendence = new AttendenceBussiness(regularClass, calling);
            regularClass_ = regularClass;
        }
    }
}
