using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.View.TouristView;
using BookingApp.UseCases;
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
using BookingApp.ViewModel.TouristViewModel;

namespace BookingApp.View.TouristView
{
    /// <summary>
    /// Interaction logic for ShowAndSearchToursPage.xaml
    /// </summary>
    public partial class ShowAndSearchToursPage : Page
    {
        private ShowAndSearchToursViewModel showAndSearchToursViewModel;
        public ShowAndSearchToursPage(User loggedInUser)
        {
            InitializeComponent();
            showAndSearchToursViewModel = new ShowAndSearchToursViewModel(loggedInUser);
            DataContext = showAndSearchToursViewModel;
        }
        private void SelectedTourCard(object sender, MouseButtonEventArgs e)
        {
            showAndSearchToursViewModel.SelectedTourCard(sender);
        }

        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            showAndSearchToursViewModel.Search();
        }

        private void ShowAllToursButtonClick(object sender, RoutedEventArgs e)
        {
            showAndSearchToursViewModel.ShowAllTours();
        }
        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }

}
