using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.CommonComponent.Calender
{
    class CalenderItemInfoModel
    {
        public string Day { get; private set; }
        public List<string> Content { get; private set; }

        public CalenderItemInfoModel(string day,List<string> info)
        {
            Day = day;
            Content = info;
        }
    }
}
