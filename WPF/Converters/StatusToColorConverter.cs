using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using static QuestPDF.Helpers.Colors;

namespace BookingApp.WPF.Converters
{
    public class StatusToColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush validBrush = new SolidColorBrush(Color.FromRgb(165, 247, 166)); // #A5F7A6
            SolidColorBrush invalidBrush = new SolidColorBrush(Color.FromRgb(255, 154, 154)); // #FF9A9A

            if (value is TouristExperience experience && experience.CommentStatus == "Valid")
            {
                return validBrush;
            }
            return invalidBrush;
        }





        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
