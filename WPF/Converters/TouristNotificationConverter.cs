using BookingApp.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows;

namespace BookingApp.WPF.Converters
{
    public class TouristNotificationConverter : MarkupExtension, IValueConverter
    {
        public TouristNotificationConverter() { }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            NotificationType type = (NotificationType)value;
            string par = (string)parameter;
            if (type != null)
            {
                if (type == NotificationType.TouristJoined)
                {
                    return par == "joined" ? Visibility.Visible : Visibility.Collapsed;
                }
                else if (type == NotificationType.ToursOfInterestCreated)
                {
                    return par == "created" ? Visibility.Visible : Visibility.Collapsed;
                }
                else if (type == NotificationType.RequestAccepted)
                {
                    return par == "accepted" ? Visibility.Visible : Visibility.Collapsed;
                }
                else if (type == NotificationType.VoucherWon)
                {
                    return par == "voucher" ? Visibility.Visible : Visibility.Collapsed;
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
