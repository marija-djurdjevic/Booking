using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class TourService
    {
        private TourRepository tourRepository;
        private TourReservationRepository tourReservationRepository;
        private LiveTourRepository liveTourRepository;

        public TourService()
        {
            tourRepository = new TourRepository();
            tourReservationRepository = new TourReservationRepository();
            liveTourRepository = new LiveTourRepository();
        }

        public List<Tour> GetMyReserved(int userId)
        {
            List<Tour> myReservedTours = new List<Tour>();
            foreach (TourReservation tourReservation in tourReservationRepository.GetByUserId(userId))
            {
                myReservedTours.Add(tourRepository.GetTourById(tourReservation.TourId));
            }
            return myReservedTours.DistinctBy(x => x.Id).ToList();
        }
        public List<Tour> GetMyActiveReserved(int userId)
        {
            List<Tour> myActiveReservedTours = new List<Tour>();
            foreach (TourReservation tourReservation in tourReservationRepository.GetByUserId(userId))
            {
                LiveTour liveTour = liveTourRepository.GetLiveTourById(tourReservation.TourId);
                if (tourReservation.IsOnTour)
                {
                    Tour activeTour = tourRepository.GetTourById(tourReservation.TourId);
                    activeTour.KeyPoints = liveTour.KeyPoints;
                    myActiveReservedTours.Add(activeTour);
                }

            }
            return myActiveReservedTours.DistinctBy(x => x.Id).ToList();
        }
    }
}
