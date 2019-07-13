using Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace DancingTrainingManagement.Components.NameCallingManagement.ValueConverters.StateConverter
{
    class StateConverterBase : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;
            if ((int)value == ActiveNum)
            {
                return ActiveColor;
            }
            else
            {
                return DeactiveColor;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        protected Color ActiveColor;
        private Color DeactiveColor = GlobalVariables.DarkMainBackColor;
        protected int ActiveNum;
    }
}
