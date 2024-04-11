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
    public partial class NotificationWindow: Window
    {
        private List<Notification> _notifications;
     
        public NotificationWindow(List<Notification> notifications)
        {
            InitializeComponent();
            _notifications = notifications;
            NotificationsListBox.ItemsSource = _notifications;
           
        }
      
    }

    public class NotificationViewModel
    {
        public string Message { get; set; }

        public NotificationViewModel(int unratedGuests)
        {
            if(unratedGuests != 0) {
                Message = $"You have {unratedGuests} unrated guest(s).";
            }
            else
            {
                Message = $"No notifications.";
            }

        }
    }

}
