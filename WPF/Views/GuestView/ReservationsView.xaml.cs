using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels.GuestViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.GuestView
{
    /// <summary>
    /// Interaction logic for ReservationsView.xaml
    /// </summary>
    public partial class ReservationsView : Page
    {
        private ReservationsViewModel viewModel;

        public ReservationsView(Guest loggedInGuest)
        {
            InitializeComponent();
            viewModel = new ReservationsViewModel(loggedInGuest);
            DataContext = viewModel;
        }

        private void Cancel_Button(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are You sure?", "Cancel reservation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (viewModel.Cancel(sender) == false)
                {
                    MessageBox.Show("The cancellation deadline for this reservation has passed.");
                }
            }
        }

        private void Change_Button(object sender, RoutedEventArgs e)
        {
            ChangeReservation changeReservatin = viewModel.ChangeReservation(sender);
            NavigationService.Navigate(changeReservatin);
        }

        private void MakeReview_Button(object sender, RoutedEventArgs e)
        {
            OwnerRating ownerRating = viewModel.MakeReview(sender);
            if(ownerRating != null)
            {
                NavigationService.Navigate(ownerRating);
            }
            else
            {
                MessageBox.Show("The deadline for making review is 5 days after the reservation.");
            }
            
        }

        private void Upcoming_Click(object sender, RoutedEventArgs e)
        {
            viewModel.GetUpcomingReservations();
            TypeTextBlock.Text = "Upcoming";
            TypePopup.IsOpen = false;
        }

        private void Completed_Click(object sender, RoutedEventArgs e)
        {
            viewModel.GetCompletedReservations();
            TypeTextBlock.Text = "Completed";
            TypePopup.IsOpen = false;
        }

        private void AllReservations_Click(object sender, RoutedEventArgs e)
        {
            viewModel.GetAllReservations();
            TypeTextBlock.Text = "All";
            TypePopup.IsOpen = false;
        }

    }
}
