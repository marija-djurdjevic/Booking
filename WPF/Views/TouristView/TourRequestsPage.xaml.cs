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
    public partial class TourRequestsPage : Page
    {
        public TourRequestsPage(User loggedInUser)
        {
            InitializeComponent();
            DataContext = new TourRequestsViewModel(loggedInUser);
            TabControla.Focus();
            
            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                switch (message.Notification)
                {
                    case "ChangeTabLeftRequests":
                        if (Stats.IsSelected)
                            TabControla.SelectedItem = Requests;
                        break;

                    case "ChangeTabRightRequests":
                        if (Requests.IsSelected)
                            TabControla.SelectedItem = Stats;
                        break;

                    case "ScrollRequestsToTop":
                        if (Requests.IsSelected)
                            RequestsSkrol.ScrollToTop();
                        else if (Stats.IsSelected)
                            StatsSkrol.ScrollToTop();
                        break;

                    case "ScrollRequestsToBottom":
                        if (Requests.IsSelected)
                            RequestsSkrol.ScrollToBottom();
                        else if (Stats.IsSelected)
                            StatsSkrol.ScrollToBottom();
                        break;

                    case "ScrollRequestsDown":
                        double newOffset = RequestsSkrol.VerticalOffset + 40; // Adjust the amount to scroll as needed
                        if (Requests.IsSelected)
                            RequestsSkrol.ScrollToVerticalOffset(newOffset);
                        else if (Stats.IsSelected)
                            StatsSkrol.ScrollToVerticalOffset(newOffset);
                        break;

                    case "ScrollRequestsUp":
                        double newOffsetUp = RequestsSkrol.VerticalOffset - 40; // Adjust the amount to scroll as needed
                        if (Requests.IsSelected)
                            RequestsSkrol.ScrollToVerticalOffset(newOffsetUp);
                        else if (Stats.IsSelected)
                            StatsSkrol.ScrollToVerticalOffset(newOffsetUp);
                        break;
                }
            });
        }
    }
}
