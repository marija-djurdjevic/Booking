using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BookingApp.WPF.Converters
{
    public class DateRangeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is (DateTime startDate, DateTime endDate))
            {
                return $"{startDate:dd.MM.yyyy HH:mm}h - {endDate:dd.MM.yyyy HH:mm}h";
            }
            return string.Empty;
        }



        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
