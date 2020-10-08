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
            switch ((CallingStatisticState)value)
            {
                case CallingStatisticState.Presence:
                    return GlobalVariables.IncomeColor;
                case CallingStatisticState.Leave:
                    return GlobalVariables.SecondaryColor;
                case CallingStatisticState.Absence:
                    return GlobalVariables.ExpenseColor;
                case CallingStatisticState.Giving:
                    return GlobalVariables.LightMainColor;
                case CallingStatisticState.Overdue:
                    return GlobalVariables.YellowColor;
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
