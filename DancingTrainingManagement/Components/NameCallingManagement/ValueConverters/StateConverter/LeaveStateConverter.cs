﻿using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.NameCallingManagement.ValueConverters.StateConverter
{
    class LeaveStateConverter : StateConverterBase
    {
        public LeaveStateConverter()
        {
            ActiveColor = GlobalVariables.SecondaryColor;
            ActiveNum = 1;
        }
    }
}
