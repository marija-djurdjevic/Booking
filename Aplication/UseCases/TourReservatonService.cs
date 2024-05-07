using BookingApp.Domain.Models;
using BookingApp.Repositories;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Aplication.UseCases
{
    public class TourReservationService
    {
        private readonly ITourReservationRepository tourReservationRepository;

        public TourReservationService(ITourReservationRepository tourReservationRepository)
        {
            this.tourReservationRepository = tourReservationRepository;
        }

        public List<TourReservation> GetByTourId(int tourId)
        {
            var tourReservations = tourReservationRepository.GetAll();
            return tourReservations.FindAll(t => t.TourId == tourId);
        }

        public int GetTouristsForTour(int tourId)
        {
            var tourists = GetByTourId(tourId);
            return tourists.Where(t => t.JoinedKeyPoint != null && !string.IsNullOrWhiteSpace(t.JoinedKeyPoint.Name)).Count();
        }

        public void Save(TourReservation tourReservation)
        {
            tourReservationRepository.Save(tourReservation);
        }

        public void Update(TourReservation updatedTourReservation)
        {
            tourReservationRepository.Update(updatedTourReservation);
        }

        public List<TourReservation> GetByUserId(int userId)
        {
            var tourReservations = tourReservationRepository.GetAll();
            return tourReservations.FindAll(t => t.UserId == userId);
        }

        public List<TourReservation> GetReservationsAttendedByUser(int userId)
        {
            var tourReservations = GetByUserId(userId);
            return tourReservations.FindAll(t => t.JoinedKeyPoint.OrdinalNumber > 0 && t.IsUser);
        }

        public bool IsUserOnTour(int userId, int tourId)
        {
            var userReservations = GetByUserId(userId);
            return userReservations.Any(r => r.TourId == tourId && r.IsOnTour && r.IsUser);
        }

        public void DeleteByTourId(int tourId)
        {
            tourReservationRepository.DeleteByTourId(tourId);
        }

        public void UpdateReservation(TourReservation reservationData)
        {
            tourReservationRepository.UpdateReservation(reservationData);
        }


       
    }
}
