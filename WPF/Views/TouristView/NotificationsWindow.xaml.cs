using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels.TouristViewModels;
using GalaSoft.MvvmLight.Messaging;
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
using System.Windows.Shapes;

namespace BookingApp.View.TouristView
{
    /// <summary>
    /// Interaction logic for NotificationsWindow.xaml
    /// </summary>
    public partial class NotificationsWindow : Window
    {
        public NotificationsWindow(User loggedInUser)
        {
            InitializeComponent();
            DataContext = new TouristNotificationsViewModel(loggedInUser);
            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                switch (message.Notification)
                {
                    case "ScrollToTop":
                        Skrol.ScrollToTop();
                        break;
                    case "ScrollToBottom":
                        Skrol.ScrollToBottom();
                        break;
                    case "ScrollDown":
                        double newOffset = Skrol.VerticalOffset + 40; // Adjust the amount to scroll as needed
                        Skrol.ScrollToVerticalOffset(newOffset);
                        break;
                    case "ScrollUp":
                        double newOffsetUp = Skrol.VerticalOffset - 40; // Adjust the amount to scroll as needed
                        Skrol.ScrollToVerticalOffset(newOffsetUp);
                        break;
                    case "CloseNotificationsWindowMessage":
                        this.Close();
                        break;
                }
            });
        }
    }
}
