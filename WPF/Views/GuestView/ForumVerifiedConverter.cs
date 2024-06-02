using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BookingApp.WPF.Views.GuestView
{
    public class ForumVerifiedConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int GuestsComments && values[1] is int OwnersComments)
            {
                if (GuestsComments >= 20 && OwnersComments >= 10)
                {
                    return new BitmapImage(new Uri("/Resources/Images/verified.png", UriKind.Relative));
                }
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
