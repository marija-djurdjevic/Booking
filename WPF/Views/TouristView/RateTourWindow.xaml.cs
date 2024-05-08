using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Build.Evaluation;
using BookingApp.Repositories;
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
using System.Windows.Controls.Primitives;
using Microsoft.Win32;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels.TouristViewModels;
using BookingApp.Aplication.Dto;
using GalaSoft.MvvmLight.Messaging;

namespace BookingApp.View.TouristView
{
    /// <summary>
    /// Interaction logic for RateTourWindow.xaml
    /// </summary>
    public partial class RateTourWindow : Window
    {
        public RateTourWindow(TourDto selectedTour, User loggedInTourist)
        {
            InitializeComponent();
            DataContext = new RateTourViewModel(selectedTour, loggedInTourist);
            Messenger.Default.Register<NotificationMessage>(this, CloseWindow);
        }

        private void CloseWindow(NotificationMessage message)
        {
            if (message.Notification == "CloseRateTourWindowMessage")
            {
                this.Close();
            }
        }        
    }
}
