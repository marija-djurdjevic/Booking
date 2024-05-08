using BookingApp.Domain.Models;
using BookingApp.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace BookingApp.WPF.Converters
{
    public class RequestStatusConverter : MarkupExtension, IValueConverter
    {
        public RequestStatusConverter() { }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TourRequestStatus status = (TourRequestStatus)value;
            string par = (string)parameter;
            if (status != null)
            {
                if (par != null)
                {
                    if (status == TourRequestStatus.Pending)
                    {
                        return par == "color" ? Brushes.DarkOrange : "/Resources/Icons/TouristIcons/Pending.png";
                    }
                    else if (status == TourRequestStatus.Accepted)
                    {
                        return par == "color" ? Brushes.Green : "/Resources/Icons/TouristIcons/accepted.png";
                    }
                    else if (status == TourRequestStatus.Invalid)
                    {
                        return par == "color" ? Brushes.Red : "/Resources/Icons/TouristIcons/Invalid.png";
                    }
                }
                else
                {
                    if (status == TourRequestStatus.Accepted)
                    {
                        return Visibility.Visible;
                    }
                    else
                    {
                        return Visibility.Collapsed;
                    }
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
