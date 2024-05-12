using BookingApp.Domain.Models;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.Views.GuestView
{
    /// <summary>
    /// Interaction logic for GuestsNotifications.xaml
    /// </summary>
    public partial class GuestsNotifications : Page
    {
        public GuestNotificationsRepository GuestNotificationsRepository { get; set; }
        public Guest LoggedInGuest { get; set; }
        public List<GuestNotification> Notifications { get; set; }
        public GuestsNotifications(Guest guest)
        {
            InitializeComponent();
            DataContext = this;
            GuestNotificationsRepository = new GuestNotificationsRepository();
            Notifications = new List<GuestNotification>();
            Notifications = GuestNotificationsRepository.GetAll().FindAll(n => n.GuestId == guest.Id);
            Notifications.Reverse();
            NotificationsListBox.ItemsSource = Notifications;
        }

        public void SignAsRead()
        {
            foreach (var notifikacija in Notifications.Where(n => !n.Read))
            {
                notifikacija.Read = true;
                GuestNotificationsRepository.Update(notifikacija);
            }
        }

        private void NotificationsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NotificationsListBox.SelectedItem != null)
            {
                var selectedNotification = NotificationsListBox.SelectedItem as GuestNotification;
                if (selectedNotification != null && !selectedNotification.Read)
                {
                    selectedNotification.Read = true;
                    GuestNotificationsRepository.Update(selectedNotification); 
                    NotificationsListBox.Items.Refresh(); 
                }
            }
        }


    }
    
}
