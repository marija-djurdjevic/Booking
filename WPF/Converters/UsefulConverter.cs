using System;
using System.Globalization;
using System.Windows.Data;
using BookingApp.Aplication.Dto;
using BookingApp.Domain.Models;

namespace BookingApp.WPF.Converters
{
    public class UsefulConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ForumDto forum)
            {
                if (forum.OwnersComments > 10 && forum.GuestsComments > 20)
                {
                    return "Very useful :)";
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
