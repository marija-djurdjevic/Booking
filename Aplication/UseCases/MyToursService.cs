using BookingApp.Domain.Models;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookingApp.Domain.RepositoryInterfaces;
using System.Threading.Tasks;

namespace BookingApp.Aplication.UseCases
{
    public class MyToursService
    {
        private ITourRepository tourRepository;
        private TouristExperienceService experienceService;
        private ITourReservationRepository tourReservationRepository;
        private TourReservationService tourReservationService;
        private ILiveTourRepository liveTourRepository;
        private ITouristExperienceRepository touristExperienceRepository;

        public MyToursService(ITourRepository tourRepository,ITourReservationRepository tourReservationRepository,ILiveTourRepository liveTourRepository)
        {
            this.tourRepository = tourRepository;
            this.tourReservationRepository = tourReservationRepository;
            this.touristExperienceRepository = Injector.CreateInstance<ITouristExperienceRepository>();
            experienceService = new TouristExperienceService(touristExperienceRepository);
            this.liveTourRepository = liveTourRepository;
            tourReservationService = new TourReservationService(tourReservationRepository);
        }

        public List<Tour> GetMyReserved(int userId)
        {
            List<Tour> myReservedTours = new List<Tour>();
            foreach (TourReservation tourReservation in tourReservationService.GetByUserId(userId))
            {
                myReservedTours.Add(tourRepository.GetById(tourReservation.TourId));
            }
            return myReservedTours.DistinctBy(x => x.Id).ToList();
        }

        public bool CanTouristRateTour(int userId, int tourId)
        {
            List<TourReservation> reservationsAttendedByUser = tourReservationService.GetReservationsAttendedByUser(userId);
            LiveTour liveTour = liveTourRepository.GetById(tourId);

            if (liveTour != null)
                return reservationsAttendedByUser.Any(x => x.TourId == tourId) && !liveTour.IsLive && !experienceService.IsTourRatedByUser(tourId, userId);
            return false;
        }

        public List<Tour> GetMyActiveReserved(int userId)
        {
            List<Tour> myActiveReservedTours = new List<Tour>();
            foreach (TourReservation tourReservation in tourReservationService.GetByUserId(userId))
            {
                LiveTour liveTour = liveTourRepository.GetById(tourReservation.TourId);
                if (liveTour != null && liveTour.IsLive)
                {
                    Tour activeTour = tourRepository.GetById(tourReservation.TourId);
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
            List<TourReservation> reservationsAttendedByUser = tourReservationService.GetReservationsAttendedByUser(userId);
            LiveTour liveTour = liveTourRepository.GetById(tourId);
            if (liveTour == null)
            {
                return "Unstarted";
            }
            else if (liveTour.IsLive)
            {
                return "Unfinished";
            }
            else if (experienceService.IsTourRatedByUser(tourId, userId))
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
