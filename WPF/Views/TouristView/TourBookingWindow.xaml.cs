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

namespace BookingApp.View.TouristView
{
    /// <summary>
    /// Interaction logic for TourBookingWindow.xaml
    /// </summary>
    public partial class TourBookingWindow : Window
    {
        private TourBookingViewModel tourBookingViewModel;
        public TourBookingWindow(TourDto selectedTour, int userId)
        {
            InitializeComponent();
            tourBookingViewModel = new TourBookingViewModel(selectedTour, userId);
            DataContext = tourBookingViewModel;
        }

        private void ConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            if(tourBookingViewModel.Confirm())
            {
                Close();
            }
        }

        private void CancelButtonCLick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {

        }
        private void ImageButtonClick(object sender, RoutedEventArgs e)
        {
            tourBookingViewModel.ShowImage();
        }

        private void NextImageButtonClick(object sender, RoutedEventArgs e)
        {
            tourBookingViewModel.GetNextImage();
        }

        private void PreviousImageButtonClick(object sender, RoutedEventArgs e)
        {
            tourBookingViewModel.GetPreviousImage();
        }
    }
}
