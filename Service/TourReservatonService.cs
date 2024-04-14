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

        public int  GetTouristsForTour(int tourId)
        {
            var tourists = tourReservationRepository.GetByTourId(tourId);
            return tourists.Where(t => t.JoinedKeyPoint != null && !string.IsNullOrWhiteSpace(t.JoinedKeyPoint.Name)).Count();
        }

        public bool IsUserOnTour(int userId, int tourId)
        {
            return tourReservationRepository.IsUserOnTour(userId, tourId);
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
            tourReservationRepository.UpdateReservation(reservationData);
        }
    }
}
