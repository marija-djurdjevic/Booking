using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels.TouristViewModels;
using GalaSoft.MvvmLight.Messaging;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using Separator = LiveCharts.Wpf.Separator;

namespace BookingApp.WPF.Views.TouristView
{
    /// <summary>
    /// Interaction logic for TourRequestsPage.xaml
    /// </summary>
    public partial class ComplexTourRequestsPage : Page
    {
        public ComplexTourRequestsPage(User loggedInUser)
        {
            InitializeComponent();
            DataContext = new ComplexTourRequestsViewModel(loggedInUser);
            RequestsSkrol.Focus();

            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                switch (message.Notification)
                {
                    case "ScrollRequestsToTop":
                        RequestsSkrol.ScrollToTop();
                        break;

                    case "ScrollRequestsToBottom":
                        RequestsSkrol.ScrollToBottom();
                        break;

                    case "ScrollRequestsDown":
                        double newOffset = RequestsSkrol.VerticalOffset + 40; // Adjust the amount to scroll as needed
                        RequestsSkrol.ScrollToVerticalOffset(newOffset);
                        break;

                    case "ScrollRequestsUp":
                        double newOffsetUp = RequestsSkrol.VerticalOffset - 40; // Adjust the amount to scroll as needed
                        RequestsSkrol.ScrollToVerticalOffset(newOffsetUp);
                        break;
                }
            });
        }
    }
}
