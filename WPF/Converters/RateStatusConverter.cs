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
using System.Runtime.CompilerServices;
using Xceed.Wpf.AvalonDock.Properties;

namespace BookingApp.WPF.Converters
{
    public class RateStatusConverter : MarkupExtension, IValueConverter
    {
        public RateStatusConverter() { }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string status = (string)value;
            string par = (string)parameter;
            if (status != null)
            {
                if (par != null)
                {
                    if (status == "Unstarted")
                    {
                        return par == "color" ? Brushes.DarkOrange : Visibility.Visible;
                    }
                    else if (status == "Unfinished")
                    {
                        return par == "color" ? Brushes.YellowGreen : Visibility.Visible;
                    }
                    else if (status == "Rated")
                    {
                        return par == "color" ? Brushes.DarkBlue : Visibility.Visible;
                    }
                    else if (status == "Tourist absent")
                    {
                        return par == "color" ? Brushes.Red : Visibility.Visible;
                    }
                    else
                    {
                        return par == "color" ? Brushes.White : Visibility.Collapsed;
                    }
                }
                else
                {
                    if (status == "Unstarted")
                    {
                        return "/Resources/Icons/TouristIcons/broken-image.png";
                    }
                    else if (status == "Unfinished")
                    {
                        return "/Resources/Icons/TouristIcons/broken-image.png";
                    }
                    else if (status == "Rated")
                    {
                        return "/Resources/Icons/TouristIcons/broken-image.png";
                    }
                    else if (status == "Tourist absent")
                    {
                        return "/Resources/Icons/TouristIcons/broken-image.png";
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
