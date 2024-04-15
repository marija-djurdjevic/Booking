using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace BookingApp.WPF.Converters
{
    public class VisitedKeyPoint : MarkupExtension, IValueConverter
    {
        public VisitedKeyPoint()
        { }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool IsChecked = (bool)value;
            string par = (string)parameter;
            if (IsChecked != null && !string.IsNullOrEmpty(par))
            {
                if (IsChecked)
                {
                    return par == "a" ? Visibility.Visible : Visibility.Collapsed;
                }
                return par == "a" ? Visibility.Collapsed : Visibility.Visible;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
