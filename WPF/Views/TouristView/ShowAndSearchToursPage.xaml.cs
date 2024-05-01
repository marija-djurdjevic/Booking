using BookingApp.Repositories;
using BookingApp.View.TouristView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels.TouristViewModels;
using GalaSoft.MvvmLight.Messaging;

namespace BookingApp.View.TouristView
{
    /// <summary>
    /// Interaction logic for ShowAndSearchToursPage.xaml
    /// </summary>
    public partial class ShowAndSearchToursPage : Page
    {
        public ShowAndSearchToursPage(User loggedInUser)
        {
            InitializeComponent();
            DataContext = new ShowAndSearchToursViewModel(loggedInUser);
            Skrol.Focus();
            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                switch (message.Notification)
                {
                    case "ScrollToursToTop":
                        Skrol.ScrollToTop();
                        break;

                    case "ScrollToursToBottom":
                        Skrol.ScrollToBottom();
                        break;

                    case "ScrollToursDown":
                        double newOffset = Skrol.VerticalOffset + 40; // Adjust the amount to scroll as needed
                        Skrol.ScrollToVerticalOffset(newOffset);
                        break;

                    case "ScrollToursUp":
                        double newOffsetUp = Skrol.VerticalOffset - 40; // Adjust the amount to scroll as needed
                        Skrol.ScrollToVerticalOffset(newOffsetUp);
                        break;
                }
            });
        }
    }

}
