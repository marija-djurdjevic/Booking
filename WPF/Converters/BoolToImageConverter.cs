using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Properties;

namespace BookingApp.WPF.Converters
{
    public class BoolToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return DependencyProperty.UnsetValue;

            bool boolValue = (bool)value;

            // Učitaj odgovarajuću sliku u zavisnosti od vrednosti
            string imagePath = boolValue ? "/Resources/Images/checked-checkbox-icon-checkbox-ico-115632629493xkxpf63d8.png" : "/Resources/Images/unchecked.jpg";
            return new BitmapImage(new Uri(imagePath, UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
