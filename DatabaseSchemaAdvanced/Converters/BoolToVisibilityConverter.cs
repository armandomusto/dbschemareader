using DatabaseSchemaAdvanced.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DatabaseSchemaAdvanced.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(parameter == null || (string)parameter == "Positive")
            {
                if (value == null)
                    return Visibility.Visible;
                if (!(bool)value)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }else
            {
                if (value == null)
                    return Visibility.Collapsed;
                if (!(bool)value)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
