using System;
using System.Globalization;
using System.Windows.Data;

namespace BookingApp.WPF.Converters
{
    public class GuestIdToOwnerLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int guestId)
            {
                return guestId == 0 ? "Owner" : string.Empty;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
