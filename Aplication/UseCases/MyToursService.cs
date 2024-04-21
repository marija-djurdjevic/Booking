using BookingApp.Domain.Models;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookingApp.Domain.RepositoryInterfaces;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using BookingApp.Aplication.Dto;
using System.Windows;
using System.Windows.Input;

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
            return SortByDate(myReservedTours.DistinctBy(x => x.Id).ToList());
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
            return SortByDate(filteredTours);
        }

        //futured tours sort by date and past show on end
        public List<Tour> SortByDate(List<Tour> unsorted)
        {
            var sorted = unsorted.OrderBy(t => t.StartDateTime < System.DateTime.Now).ThenBy(t => t.StartDateTime).ToList();
            return sorted;
        }

        public void SortTours(ObservableCollection<Tuple<TourDto,Visibility,string>> unsorted, string sortBy)
        {
            var sorted = new List<Tuple<TourDto, Visibility, string>>();
            switch (sortBy)
            {
                case "System.Windows.Controls.ComboBoxItem: Date - Ascending":
                    sorted = unsorted.OrderBy(t => t.Item1.StartDateTime).ThenBy(t => t.Item2).ToList();
                    break;
                case "System.Windows.Controls.ComboBoxItem: Date - Descending":
                    sorted = unsorted.OrderByDescending(t => t.Item1.StartDateTime).ThenByDescending(t => t.Item2).ToList();
                    break;
                case "System.Windows.Controls.ComboBoxItem: Rate status - Ascending":
                    sorted = unsorted.OrderBy(t => t.Item2).ThenBy(t => t.Item1.StartDateTime).ToList();
                    break;
                case "System.Windows.Controls.ComboBoxItem: Rate status - Descending":
                    sorted = unsorted.OrderByDescending(t => t.Item2).ThenByDescending(t => t.Item1.StartDateTime).ToList();
                    break;
                default:
                    return;
            }
            unsorted.Clear();
            foreach (var sortedTour in sorted)
            {
                unsorted.Add(sortedTour);
            }
            return;
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
