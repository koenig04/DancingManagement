using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.NameCallingManagement.ValueConverters.StateConverter
{
    class PresenceStateConverter:StateConverterBase
    {
        public PresenceStateConverter()
        {
            ActiveColor = GlobalVariables.IncomeColor;
            ActiveNum = 0;
        }
    }
}
