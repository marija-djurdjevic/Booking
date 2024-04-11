using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Service
{
    public class TourReservationService
    {
        private readonly TourReservationRepository _tourReservationRepository;

        public TourReservationService()
        {
            _tourReservationRepository = new TourReservationRepository();
        }


        public List<TourReservation> GetByTourId(int tourId)
        {
           return _tourReservationRepository.GetByTourId(tourId);
        }



        public void UpdateReservation(TourReservation reservationData)
        {
            var tourReservations = _tourReservationRepository.GetAll();
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
            _tourReservationRepository.SaveChanges();
        }
    }
}
