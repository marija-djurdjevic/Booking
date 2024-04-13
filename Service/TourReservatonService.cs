using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Service
{
    public class TourReservationService
    {
        private readonly TourReservationRepository tourReservationRepository;

        public TourReservationService()
        {
            tourReservationRepository = new TourReservationRepository();
        }


        public List<TourReservation> GetByTourId(int tourId)
        {
           return tourReservationRepository.GetByTourId(tourId);
        }

        public void SaveChanges()
        { 
            tourReservationRepository.SaveChanges();
        }

        public void DeleteByTourId(int tourId)
        {
             tourReservationRepository.DeleteByTourId(tourId) ;
        }

            public void UpdateReservation(TourReservation reservationData)
        {
            var tourReservations = tourReservationRepository.GetAll();
            var existingReservation = tourReservations.FirstOrDefault(r => r.TourId == reservationData.TourId && r.TouristFirstName == reservationData.TouristFirstName && r.TouristLastName == reservationData.TouristLastName);
            if (existingReservation != null)
            {
                existingReservation.IsOnTour = reservationData.IsOnTour;
                existingReservation.JoinedKeyPoint = reservationData.JoinedKeyPoint;
            }
            else
            {
                tourReservations.Add(reservationData);
            }
            tourReservationRepository.SaveChanges();
        }
    }
}
