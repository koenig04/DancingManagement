using BLL.StatisticManagement.ClassStatistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.Statistic.Class
{
    class ClassViewModel : ConcreteStatisticViewModelBase
    {

        ClassStatisticBussiness bussiness_;

        public ClassViewModel(ClassStatisticBussiness bussiness):base()
        {
            bussiness_ = bussiness;
        }
    }
}
