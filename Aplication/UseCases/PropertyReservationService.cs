using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.GuestView;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Aplication.UseCases
{
    public class PropertyReservationService
    {
        private readonly IPropertyRepository propertyRepository;
        private readonly IPropertyReservationRepository propertyReservationRepository;
        private readonly IReservedDateRepository reservedDateRepository;

        public PropertyReservationService(IPropertyRepository propertyRepository, IPropertyReservationRepository propertyReservationRepository, IReservedDateRepository reservedDateRepository)
        {
            this.propertyRepository = propertyRepository;
            this.propertyReservationRepository = propertyReservationRepository;
            this.reservedDateRepository = reservedDateRepository;
        }

        public ObservableCollection<PropertyReservation> GetGuestReservations(int guestId)
        {
            return new ObservableCollection<PropertyReservation>(propertyReservationRepository.GetAll().FindAll(r => r.GuestId == guestId && r.Canceled == false));
        }

        public List<PropertyReservation> UpdateGuestReservations(int guestId)
        {
            return propertyReservationRepository.GetAll().FindAll(r => r.GuestId == guestId && r.Canceled == false);
        }
        public bool CanCancelReservation(PropertyReservation SelectedReservation)
        {
            Property SelectedProperty = propertyRepository.GetPropertyById(SelectedReservation.PropertyId);
            if (DateTime.Now.AddDays(SelectedProperty.CancellationDeadline) <= SelectedReservation.StartDate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CancelReservation(PropertyReservation SelectedReservation)
        {
            SelectedReservation.Canceled = true;
            propertyReservationRepository.Update(SelectedReservation);
            reservedDateRepository.Delete(SelectedReservation.Id);
        }

        public Property GetPropertyByReservation(PropertyReservation SelectedReservation)
        {
            return propertyRepository.GetPropertyById(SelectedReservation.PropertyId);
        }

        public bool CanMakeReview(PropertyReservation SelectedReservation)
        {
            if (SelectedReservation.EndDate < DateTime.Now && DateTime.Now <= SelectedReservation.EndDate.AddDays(5))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<string> CheckAvailabilityForAllRequests(IEnumerable<ReservationChangeRequest> requests)
        {
            var allReservations = propertyReservationRepository.GetAll();

            List<string> availabilityStatus = new List<string>();

            foreach (var request in requests)
            {
                int reservationId = request.ReservationId;

                var reservationsForProperty = allReservations.Where(r => r.Id == reservationId).ToList();

                bool isAvailable = CheckAvailability(request.NewStartDate, request.NewEndDate, reservationsForProperty);
                availabilityStatus.Add(isAvailable ? "Free" : "Occupied");
            }

            return availabilityStatus;
        }

        private bool CheckAvailability(DateTime newStartDate, DateTime newEndDate, List<PropertyReservation> reservations)
        {
            foreach (var reservation in reservations)
            {

                if (newStartDate >= reservation.StartDate && newStartDate <= reservation.EndDate)
                {
                    return false;
                }
                if (newEndDate >= reservation.StartDate && newEndDate <= reservation.EndDate)
                {
                    return false;
                }
            }
            return true;
        }
        public void UpdateReservation(ReservationChangeRequest request)
        {
            var reservationsForProperty = propertyReservationRepository.GetReservationDataById(request.ReservationId);

            if (reservationsForProperty != null && reservationsForProperty.Any())
            {

                var updatedReservation = reservationsForProperty.First();
                updatedReservation.StartDate = request.NewStartDate;
                updatedReservation.EndDate = request.NewEndDate;

                propertyReservationRepository.UpdatePropertyReservation(updatedReservation);
            }
            else
            {
                throw new Exception("Reservation not found for ReservationId: " + request.ReservationId);
            }
        }
    }
}
