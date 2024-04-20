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
using System.Windows.Shapes;

namespace BookingApp.WPF.Views.TouristView
{
    /// <summary>
    /// Interaction logic for CreateTourRequestWindow.xaml
    /// </summary>
    public partial class CreateTourRequestWindow : Window
    {
        private CreateTourRequestViewModel createTourRequestViewModel;
        public CreateTourRequestWindow(User loggedInUser)
        {
            createTourRequestViewModel = new CreateTourRequestViewModel(loggedInUser);
            InitializeComponent();
            DataContext = createTourRequestViewModel;
        }

        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            createTourRequestViewModel.Confirm();
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
            createTourRequestViewModel.CityComboBoxLostFocus();
        }

        private void CountryComboBoxChanged(object sender, RoutedEventArgs e)
        {
            createTourRequestViewModel.CountryComboBoxChanged();
        }

        private void OpenDropDownClick(object sender, EventArgs e)
        {
            createTourRequestViewModel.OpenDropDownClick(sender);
        }
    }
}
