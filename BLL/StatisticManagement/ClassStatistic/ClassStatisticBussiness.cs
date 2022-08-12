using BLL.ClassManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.StatisticManagement.ClassStatistic
{
    public class ClassStatisticBussiness
    {
        private RegularClassManagement regularClass_;

        public ClassStatisticBussiness(RegularClassManagement regularClass)
        {
            regularClass_ = regularClass;
        }
    }
}
