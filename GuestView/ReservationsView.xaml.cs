using BookingApp.Dto;
using BookingApp.Model;
using BookingApp.Repository;
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

namespace BookingApp.GuestView
{
    /// <summary>
    /// Interaction logic for ReservationsView.xaml
    /// </summary>
    public partial class ReservationsView : Page
    {
        public PropertyRepository PropertyRepository { get; set; }
        public PropertyReservationRepository PropertyReservationRepository { get; set; }
        public ReservedDateRepository ReservedDateRepository { get; set; }
        public List<PropertyReservation> GuestsReservations { get; set; }
        public PropertyReservation SelectedReservation { get; set; }
        public Property SelectedProperty { get; set; }
        public Guest LoggedInGuest { get; set; }
        public ReservationsView(Guest loggedInGuest)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInGuest = loggedInGuest;
            PropertyRepository = new PropertyRepository();
            PropertyReservationRepository = new PropertyReservationRepository();
            ReservedDateRepository = new ReservedDateRepository();
            SelectedReservation = new PropertyReservation();
            GuestsReservations = PropertyReservationRepository.GetAll().FindAll(r => r.GuestId == loggedInGuest.Id);
        }

        private void ReservationsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Cancel_Button(object sender, RoutedEventArgs e)
        {
            Button cancelButton = sender as Button;
            SelectedReservation = cancelButton.Tag as PropertyReservation;
            if (SelectedReservation != null)
            {
                SelectedProperty = PropertyRepository.GetPropertyById(SelectedReservation.PropertyId);
                if (DateTime.Now.AddDays(SelectedProperty.CancellationDeadline) <= SelectedReservation.StartDate)
                {
                    PropertyReservationRepository.Delete(SelectedReservation.Id);
                    ReservedDateRepository.Delete(SelectedReservation.Id);
                    MessageBox.Show("Succesfully canceled!");

                }
                else
                {
                    MessageBox.Show("The cancellation deadline for this reservation has passed.");
                }
            }
        }

        private void Change_Button(object sender, RoutedEventArgs e)
        {
            Button changeButton = sender as Button;
            SelectedReservation = changeButton.Tag as PropertyReservation;
            SelectedProperty = PropertyRepository.GetPropertyById(SelectedReservation.PropertyId);
            ChangeReservation changeReservatin = new ChangeReservation(SelectedReservation, SelectedProperty, LoggedInGuest);
            NavigationService.Navigate(changeReservatin);
        }

        private void MakeReview_Button(object sender, RoutedEventArgs e)
        {

        }
    }
}
