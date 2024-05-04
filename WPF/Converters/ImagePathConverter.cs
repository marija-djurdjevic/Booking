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
            string brokenImage = ImageService.GetAbsolutePath("Resources\\Icons\\TouristIcons\\broken-image.png");
            if (value is string imagePath && !string.IsNullOrEmpty(imagePath))
            {
                string absoluteImagePath = ImageService.GetAbsolutePath(value.ToString());
                if (System.IO.File.Exists(absoluteImagePath))
                {
                    if (absoluteImagePath.EndsWith(".mp4"))
                        return brokenImage;
                    return ImageService.GetAbsolutePath(value.ToString());
                }
                return brokenImage;
            }

            return brokenImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}