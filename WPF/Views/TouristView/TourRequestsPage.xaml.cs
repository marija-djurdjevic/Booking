using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModel.TouristViewModel;
using BookingApp.WPF.ViewModels.TouristViewModels;
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

namespace BookingApp.WPF.Views.TouristView
{
    /// <summary>
    /// Interaction logic for TourRequestsPage.xaml
    /// </summary>
    public partial class TourRequestsPage : Page
    {
        private TourRequestsViewModel tourRequestsViewModel;
        public TourRequestsPage(User loggedInUser)
        {
            InitializeComponent();
            tourRequestsViewModel = new TourRequestsViewModel(loggedInUser);
            DataContext = tourRequestsViewModel;
        }

        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void InboxButtonClick(object sender, RoutedEventArgs e)
        {
            tourRequestsViewModel.OpenInbox();
        }

        private void CreateButtonClick(object sender, RoutedEventArgs e)
        {
            tourRequestsViewModel.CreateTourRequest();
        }

        private void SortingComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(tourRequestsViewModel != null)
                tourRequestsViewModel.SortingSelectionChanged();
        }
    }
}
