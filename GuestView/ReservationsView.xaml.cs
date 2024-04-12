using BookingApp.Dto;
using BookingApp.Model;
using BookingApp.Repository;
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
        public ObservableCollection<PropertyReservation> GuestsReservations { get; set; }
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
            GuestsReservations = new ObservableCollection<PropertyReservation>(PropertyReservationRepository.GetAll().FindAll(r => r.GuestId == loggedInGuest.Id && r.Canceled == false));
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
                    SelectedReservation.Canceled = true;
                    PropertyReservationRepository.Update(SelectedReservation);
                    ReservedDateRepository.Delete(SelectedReservation.Id);
                    GuestsReservations.Clear();
                    PropertyReservationRepository.GetAll().FindAll(r => r.GuestId == LoggedInGuest.Id && r.Canceled == false).ForEach(GuestsReservations.Add);
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
            Button makeReviewButton = sender as Button;
            SelectedReservation = makeReviewButton.Tag as PropertyReservation;
            SelectedProperty = PropertyRepository.GetPropertyById(SelectedReservation.PropertyId);
            if (SelectedReservation.EndDate < DateTime.Now && DateTime.Now <= SelectedReservation.EndDate.AddDays(5))
            {
                OwnerRating ownerRating = new OwnerRating(SelectedReservation, SelectedProperty, LoggedInGuest);
                NavigationService.Navigate(ownerRating);
            }
            else
            {
                MessageBox.Show("The deadline for making review is 5 days after the reservation.");
            }
            
        }
    }
}
