using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.NameCallingManagement.ValueConverters.StateConverter
{
    class AbsenceStateConverter: StateConverterBase
    {
        public AbsenceStateConverter()
        {
            ActiveColor = GlobalVariables.ExpenseColor;
            ActiveNum = 2;
        }        
    }
}
