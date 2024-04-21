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
using BookingApp.WPF.ViewModel.TouristViewModel;
using BookingApp.Aplication.Dto;

namespace BookingApp.View.TouristView
{
    /// <summary>
    /// Interaction logic for TouristsDataWindow.xaml
    /// </summary>
    public partial class TouristsDataWindow : Window
    {
        private TouristsDataViewModel touristsDataViewModel;
        public TouristsDataWindow(int touristNumber, TourDto selectedTour, int userId,bool isRequest, TourRequest tourRequest)
        {
            InitializeComponent();
            touristsDataViewModel = new TouristsDataViewModel(touristNumber, selectedTour, userId,isRequest,tourRequest);
            DataContext = touristsDataViewModel;
        }

        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            if (touristsDataViewModel.Confirm())
            {
                Close();
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
         
        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
