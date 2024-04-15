using BookingApp.Domain.Models;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModel.TouristViewModel;
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

namespace BookingApp.View.TouristView
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private SettingsViewModel settingsViewModel;
        public SettingsWindow(User loggedInUser)
        {
            InitializeComponent();
            settingsViewModel = new SettingsViewModel(loggedInUser);
            DataContext = settingsViewModel;
        }

        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void CloseButtonCLick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
