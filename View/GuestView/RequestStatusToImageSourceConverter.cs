using BookingApp.Model;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BookingApp.GuestView
{
    public class RequestStatusToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is RequestStatus status)
            {
                switch (status)
                {
                    case RequestStatus.Processing:
                        return new BitmapImage(new Uri("/Resources/Images/question.png", UriKind.Relative));
                    case RequestStatus.Approved:
                        return new BitmapImage(new Uri("/Resources/Images/correct.png", UriKind.Relative));
                    case RequestStatus.Declined:
                        return new BitmapImage(new Uri("/Resources/Images/delete-button.png", UriKind.Relative));
                    default:
                        return null;
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
