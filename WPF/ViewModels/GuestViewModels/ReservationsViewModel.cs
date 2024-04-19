using BookingApp.Aplication;
using BookingApp.Aplication.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.GuestView;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace BookingApp.WPF.ViewModel.GuestViewModel
{
    public class ReservationsViewModel
    {
        public ObservableCollection<PropertyReservation> GuestsReservations { get; set; }
        public PropertyReservation SelectedReservation { get; set; }
        public Property SelectedProperty { get; set; }
        public Guest LoggedInGuest { get; set; }
        public PropertyReservationService propertyReservationService;

        public ReservationsViewModel(Guest loggedInGuest)
        {
            LoggedInGuest = loggedInGuest;
            propertyReservationService = new PropertyReservationService(Injector.CreateInstance<IPropertyRepository>(), Injector.CreateInstance<IPropertyReservationRepository>());
            SelectedReservation = new PropertyReservation();
            GuestsReservations = new ObservableCollection<PropertyReservation>();
            GuestsReservations = propertyReservationService.GetGuestReservations(loggedInGuest.Id);
        }

        public bool Cancel(object sender)
        {
            Button cancelButton = sender as Button;
            SelectedReservation = cancelButton.Tag as PropertyReservation;
            if (propertyReservationService.CanCancelReservation(SelectedReservation))
            {
                CancelReservation(SelectedReservation);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CancelReservation(PropertyReservation SelectedReservation)
        {
            propertyReservationService.CancelReservation(SelectedReservation);
            GuestsReservations.Clear();
            propertyReservationService.UpdateGuestReservations(LoggedInGuest.Id).ForEach(GuestsReservations.Add);
        }

        public ChangeReservation ChangeReservation(object sender)
        {
            Button changeButton = sender as Button;
            SelectedReservation = changeButton.Tag as PropertyReservation;
            SelectedProperty = propertyReservationService.GetPropertyByReservation(SelectedReservation);
            ChangeReservation changeReservatin = new ChangeReservation(SelectedReservation, SelectedProperty, LoggedInGuest);
            return changeReservatin;
        }

        public OwnerRating? MakeReview(object sender)
        {
            Button makeReviewButton = sender as Button;
            SelectedReservation = makeReviewButton.Tag as PropertyReservation;
            SelectedProperty = propertyReservationService.GetPropertyByReservation(SelectedReservation);
            if (propertyReservationService.CanMakeReview(SelectedReservation))
            {
                OwnerRating ownerRating = new OwnerRating(SelectedReservation, SelectedProperty, LoggedInGuest);
                return ownerRating;
            }
            else
            {
                return null;

            }
        }
    }
}
