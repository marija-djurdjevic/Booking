using BookingApp.Domain.Models;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
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
using BookingApp.WPF.ViewModels.TouristViewModels;
using BookingApp.Aplication.Dto;
using GalaSoft.MvvmLight.Messaging;

namespace BookingApp.View.TouristView
{
    /// <summary>
    /// Interaction logic for TourBookingWindow.xaml
    /// </summary>
    public partial class TourBookingWindow : Window
    {
        public TourBookingWindow(TourDto selectedTour, int userId)
        {
            InitializeComponent();
            DataContext = new TourBookingViewModel(selectedTour, userId);
            Messenger.Default.Register<NotificationMessage>(this, CloseWindow);
        }

        private void CloseWindow(NotificationMessage message)
        {
            if (message.Notification == "CloseTourBookingWindowMessage")
            {
                this.Close();
            }
        }
    }
}
