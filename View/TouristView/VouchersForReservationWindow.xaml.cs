using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.ViewModel.TouristViewModel;
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

namespace BookingApp.View.TouristView
{
    /// <summary>
    /// Interaction logic for VouchersForReservationWindow.xaml
    /// </summary>
    public partial class VouchersForReservationWindow : Window
    {
        public VouchersForReservationViewModel VouchersForReservationViewModel { get; set; }
        public VouchersForReservationWindow(User loggedInUser)
        {
            InitializeComponent();
            VouchersForReservationViewModel = new VouchersForReservationViewModel(loggedInUser);
            DataContext = VouchersForReservationViewModel;
        }
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            if(VouchersForReservationViewModel.Confirm())
            {
                Close();
            }
        }

        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
