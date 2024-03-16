using BookingApp.Dto;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for Notification.xaml
    /// </summary>
    public partial class Notification: Window
    {
       
        public Notification(int unratedGuests)
        {
            InitializeComponent();
            DataContext = new NotificationViewModel(unratedGuests);
        }
    }

    public class NotificationViewModel
    {
        public string Message { get; set; }

        public NotificationViewModel(int unratedGuests)
        {
            Message = $"Imate {unratedGuests} neocijenjenih gostiju.";
        }
    }

}
