using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BookingApp.WPF.Converters
{
    public class MonthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string monthNumber)
            {
                int.TryParse(monthNumber, out int month);
                string monthName = CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(month);
                return char.ToUpper(monthName[0]) + monthName.Substring(1);
            }
            return value;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
