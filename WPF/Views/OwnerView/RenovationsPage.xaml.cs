using BookingApp.Domain.Models;
using BookingApp.View;
using BookingApp.WPF.ViewModels.OwnerViewModel;
using BookingApp.WPF.ViewModels.OwnerViewModels;
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


namespace BookingApp.WPF.Views.OwnerView
{
    /// <summary>
    /// Interaction logic for RenovationsPage.xaml
    /// </summary>
    public partial class RenovationsPage : Page
    {
        User LoggedInUser;
        public RenovationsPage(User LoggedInUser)
        {
            InitializeComponent();
            this.LoggedInUser = LoggedInUser;
            
            DataContext = new RenovationsViewModel(LoggedInUser);
            
        }

        private void ScheduleRenovation_Click(object sender, RoutedEventArgs e)
        {
            ScheduleRenovationPage renovation = new ScheduleRenovationPage(LoggedInUser);
            this.NavigationService.Navigate(renovation);
            
        }
    }
}
