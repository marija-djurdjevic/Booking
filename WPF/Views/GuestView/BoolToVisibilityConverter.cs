using BookingApp.Domain.Models;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BookingApp.WPF.Views.GuestView
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public PropertyRepository? PropertyRepository { get; set; }
        public OwnerRepository? OwnerRepository { get; set; }
        public List<int>? SueprOwnersIds { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PropertyRepository = new PropertyRepository();
            OwnerRepository = new OwnerRepository();
            SueprOwnersIds = new List<int>();
            foreach (Owner owner in OwnerRepository.GetAll())
            {
                if (owner.IsSuperOwner == true)
                {
                    SueprOwnersIds.Add(owner.Id);
                }
            }
            if (value is int OwnerId)
            {
                if (SueprOwnersIds.Contains(OwnerId))
                {
                    return new BitmapImage(new Uri("/Resources/Images/star (2).png", UriKind.Relative));
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
