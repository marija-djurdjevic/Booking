using System.Windows;
using System.Windows.Controls;
using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModel.GuestViewModel;

namespace BookingApp.GuestView
{
    /// <summary>
    /// Interaction logic for ChangeReservation.xaml
    /// </summary>
    public partial class ChangeReservation : Page
    {

        private ChangeReservationViewModel viewModel;
       
        public ChangeReservation(PropertyReservation selectedReservation, Property selectedProperty, Guest guest)
        {
            InitializeComponent();
            viewModel = new ChangeReservationViewModel(selectedReservation, selectedProperty, guest);
            DataContext = viewModel;
            
        }

        private void DatePicker_SelectedDate1Changed(object sender, SelectionChangedEventArgs e)
        {
            viewModel.ChangeDate1(sender);
        }

        private void DatePicker_SelectedDate2Changed(object sender, SelectionChangedEventArgs e)
        {
            viewModel.ChangeDate2(sender);
        }

        private void SendRequest_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SendRequest();
            MessageBox.Show("Request sent!");

        }

    }
}
