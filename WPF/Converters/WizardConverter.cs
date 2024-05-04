using BookingApp.Aplication.UseCases;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace BookingApp.WPF.Converters
{
    public class WizardConverter : MarkupExtension, IValueConverter
    {
        public WizardConverter()
        { }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null && value != null)
            {
                var image = value as string;

                string ImageNameFirstNumber = image.Split(".")[0].Split("\\").Last();

                if (int.TryParse(parameter.ToString(), out var parameterNumber) && int.TryParse(ImageNameFirstNumber, out var number))
                {
                    if (number == parameterNumber)
                    {
                        return "/Resources/Icons/TouristIcons/wizard.png";
                    }
                    else if (number < parameterNumber)
                    {
                        return "/Resources/Icons/TouristIcons/round.png";
                    }
                    else if (number > parameterNumber)
                    {
                        return "/Resources/Icons/TouristIcons/checked.png";
                    }
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}