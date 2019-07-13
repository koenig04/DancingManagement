using Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace DancingTrainingManagement.Components.CommonComponent.Converter
{
    class PresenceColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;
            switch ((CallingState)value)
            {
                case CallingState.Presence:
                    return GlobalVariables.IncomeColor;
                case CallingState.Leave:
                    return GlobalVariables.SecondaryColor;
                case CallingState.Absence:
                    return GlobalVariables.ExpenseColor;
                case CallingState.Giving:
                    return GlobalVariables.LightMainColor;
                default:
                    return System.Windows.Media.Colors.White;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
