using BookingApp.DTO;
using BookingApp.Repository;
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
using BookingApp.Model;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Collections;
using BookingApp.ViewModel.TouristViewModel;

namespace BookingApp.View.TouristView
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public SearchViewModel searchViewModel { get; set; }
        public SearchWindow(ObservableCollection<TourDto> tours)
        {
            InitializeComponent();
            searchViewModel = new SearchViewModel(tours);
            DataContext = searchViewModel;
        }

        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            searchViewModel.Confirm();
            Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void CityComboBoxLostFocus(object sender, RoutedEventArgs e)
        {
            searchViewModel.CityComboBoxLostFocus();
        }

        private void CountryComboBoxChanged(object sender, RoutedEventArgs e)
        {
            searchViewModel.CountryComboBoxChanged();
        }

        private void OpenDropDownClick(object sender, EventArgs e)
        {
            searchViewModel.OpenDropDownClick(sender);
        }
    }
}
