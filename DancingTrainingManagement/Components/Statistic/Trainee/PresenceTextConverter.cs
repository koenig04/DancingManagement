using Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace DancingTrainingManagement.Components.Statistic.Trainee
{
    class PresenceTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;
            switch ((CallingStatisticState)value)
            {
                case CallingStatisticState.Presence:
                case CallingStatisticState.Overdue:
                    return "出勤";
                case CallingStatisticState.Leave:
                    return "请假";
                case CallingStatisticState.Absence:
                    return "旷课";
                case CallingStatisticState.Giving:
                    return "送课";
                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
