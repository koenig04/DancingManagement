using Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace DancingTrainingManagement.Components.CommonComponent.Message
{
    class MessageLevelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;

            switch((MessageLevel)value)
            {
                case MessageLevel.Info:
                    return GlobalVariables.MainColor;
                case MessageLevel.Warning:
                    return GlobalVariables.ExpenseColor;
                default:
                    return GlobalVariables.MainColor;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
