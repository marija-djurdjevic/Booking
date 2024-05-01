using BookingApp.Domain.Models;
using BookingApp.Repositories;
using BookingApp.View.TouristView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using BookingApp.WPF.ViewModels.TouristViewModels;
using BookingApp.Aplication.Dto;
using GalaSoft.MvvmLight.Messaging;

namespace BookingApp.View.TouristView
{
    /// <summary>
    /// Interaction logic for TouristsDataWindow.xaml
    /// </summary>
    public partial class TouristsDataWindow : Window
    {
        public TouristsDataWindow(int touristNumber, TourDto selectedTour, int userId,bool isRequest, TourRequestViewModel tourRequest)
        {
            InitializeComponent();
            DataContext = new TouristsDataViewModel(touristNumber, selectedTour, userId, isRequest, tourRequest);
            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                switch (message.Notification)
                {
                    case "ScrollDataToTop":
                        Skrol.ScrollToTop();
                        break;
                    case "ScrollDataToBottom":
                        Skrol.ScrollToBottom();
                        break;
                    case "ScrollDataDown":
                        double newOffset = Skrol.VerticalOffset + 40; // Adjust the amount to scroll as needed
                        Skrol.ScrollToVerticalOffset(newOffset);
                        break;
                    case "ScrollDataUp":
                        double newOffsetUp = Skrol.VerticalOffset - 40; // Adjust the amount to scroll as needed
                        Skrol.ScrollToVerticalOffset(newOffsetUp);
                        break;
                    case "CloseTouristsDataWindowMessage":
                        this.Close();
                        break;
                }
            });
        }
    }
}
