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
            switch ((CallingState)value)
            {
                case CallingState.Presence:
                    return "出勤";
                case CallingState.Leave:
                    return "请假";
                case CallingState.Absence:
                    return "旷课";
                case CallingState.Giving:
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
