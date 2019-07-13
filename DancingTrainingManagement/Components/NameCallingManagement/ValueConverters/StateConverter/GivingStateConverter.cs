using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.NameCallingManagement.ValueConverters.StateConverter
{
    class GivingStateConverter:StateConverterBase
    {
        public GivingStateConverter()
        {
            ActiveColor = GlobalVariables.LightMainColor;
            ActiveNum = 3;
        }
    }
}
