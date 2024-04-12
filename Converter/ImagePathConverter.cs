using BookingApp.UseCases;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace BookingApp.Converter
{
    public class ImagePathConverter : MarkupExtension, IValueConverter
    {
        public ImagePathConverter()
        { }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string imagePath && !string.IsNullOrEmpty(imagePath))
            {
                string absoluteImagePath = ImageService.GetAbsolutePath(value.ToString());
                if (System.IO.File.Exists(absoluteImagePath))
                {
                    return ImageService.GetAbsolutePath(value.ToString());
                }
                return ImageService.GetAbsolutePath("Resources\\Icons\\TouristIcons\\broken-image.png");
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}