using BookingApp.Domain.Models;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Aplication.UseCases
{
    public class MyToursService
    {
        private TourRepository tourRepository;
        private TouristExperienceRepository experienceRepository;
        private TourReservationRepository tourReservationRepository;
        private LiveTourRepository liveTourRepository;

        public MyToursService()
        {
            tourRepository = new TourRepository();
            tourReservationRepository = new TourReservationRepository();
            experienceRepository = new TouristExperienceRepository();
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

        public bool CanTouristRateTour(int userId, int tourId)
        {
            List<TourReservation> reservationsAttendedByUser = tourReservationRepository.GetReservationsAttendedByUser(userId);
            LiveTour liveTour = liveTourRepository.GetLiveTourById(tourId);

            if (liveTour != null)
                return reservationsAttendedByUser.Any(x => x.TourId == tourId) && !liveTour.IsLive && !experienceRepository.IsTourRatedByUser(tourId, userId);
            return false;
        }

        public List<Tour> GetMyActiveReserved(int userId)
        {
            List<Tour> myActiveReservedTours = new List<Tour>();
            foreach (TourReservation tourReservation in tourReservationRepository.GetByUserId(userId))
            {
                LiveTour liveTour = liveTourRepository.GetLiveTourById(tourReservation.TourId);
                if (liveTour != null && liveTour.IsLive)
                {
                    Tour activeTour = tourRepository.GetTourById(tourReservation.TourId);
                    activeTour.KeyPoints = liveTour.KeyPoints;
                    myActiveReservedTours.Add(activeTour);
                }

            }
            return myActiveReservedTours.DistinctBy(x => x.Id).ToList();
        }

        public List<Tour> GetMyFinishedTours(int userId)
        {
            List<Tour> myTours = GetMyReserved(userId);
            List<LiveTour> allFinishedTours = liveTourRepository.GetFinishedTours();
            // Filtriranje elemenata iz prve liste na osnovu id-a u drugoj listi
            List<Tour> filteredTours = myTours.Where(tour => allFinishedTours.Any(liveTour => liveTour.TourId == tour.Id)).ToList();
            return filteredTours;
        }

        public string GetTourStatusMessage(int userId, int tourId)
        {
            List<TourReservation> reservationsAttendedByUser = tourReservationRepository.GetReservationsAttendedByUser(userId);
            LiveTour liveTour = liveTourRepository.GetLiveTourById(tourId);
            if (liveTour == null)
            {
                return "Unstarted";
            }
            else if (liveTour.IsLive)
            {
                return "Unfinished";
            }
            else if (experienceRepository.IsTourRatedByUser(tourId, userId))
            {
                return "Rated";
            }
            else if (!reservationsAttendedByUser.Any(x => x.TourId == tourId))
            {
                return "Tourist absent";
            }
            return "";
        }

    }
}
