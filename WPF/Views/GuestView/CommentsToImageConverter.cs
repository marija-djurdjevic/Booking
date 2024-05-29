using BookingApp.Domain.Models;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BookingApp.WPF.Views.GuestView
{
    public class CommentsToImageConverter : IValueConverter
    {
    public OwnerRepository? OwnerRepository { get; set; }
    public ForumRepository? ForumRepository {  get; set; }
    public GuestRepository? GuestRepository {  get; set; }
    public List<int>? Comments { get; set; }
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        ForumRepository = new ForumRepository();
        GuestRepository = new GuestRepository();
        OwnerRepository = new OwnerRepository();
        Comments = new List<int>();
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
}
