using BookingApp.Repositories;
using BookingApp.WPF.ViewModels.TouristViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Aplication.UseCases
{
    public class TourService
    {
        private ITourRepository tourRepository;
        private ILiveTourRepository liveTourRepository;
        private IKeyPointRepository keyPointRepository;
        private ITouristExperienceRepository touristExperienceRepository;
        private readonly KeyPointService keyPointService;
        private readonly TouristExperienceService touristExperienceService;
        public TourService(ITourRepository tourRepository, ILiveTourRepository liveTourRepository)
        {
            this.tourRepository = tourRepository;
            this.liveTourRepository = liveTourRepository;
            this.keyPointRepository = Injector.CreateInstance<IKeyPointRepository>();
            this.touristExperienceRepository = Injector.CreateInstance<ITouristExperienceRepository>();
            keyPointService = new KeyPointService(keyPointRepository, liveTourRepository);
            touristExperienceService = new TouristExperienceService(touristExperienceRepository);
        }

        public List<Tour> GetToursWithKeyPoints()
        {
            var tours = tourRepository.GetAll();
            foreach (var tour in tours)
            {
                tour.KeyPoints = keyPointService.GetTourKeyPoints(tour.Id);
            }
            return tours;
        }

        public List<Tour> GetAll()
        {
            return tourRepository.GetAll();
        }

        public List<Tour> GetAllSorted()
        {
            return SortByDate(GetAll());
        }

        //futured tours sort by date and past show on end
        public List<Tour> SortByDate(List<Tour> unsorted)
        {
            var sorted = unsorted
                .OrderBy(t => t.StartDateTime.Date < DateTime.Now.Date) // Ture sa datumom manjim od trenutnog dolaze prvo
                .ThenByDescending(t => t.StartDateTime.Date < DateTime.Now.Date ? t.StartDateTime : DateTime.MaxValue) // Sortiraj ture sa datumom manjim od trenutnog u rastucem redosledu
                .ThenBy(t => t.StartDateTime.Date >= DateTime.Now.Date ? t.StartDateTime : DateTime.MinValue) // Sortiraj ture sa datumom vecim ili jednakim trenutnom u opadajucem redosledu
                .ToList();
            return sorted;
        }

        public int GetNumberOfTouristsForTour(int tourId)
        {
            return touristExperienceService.GetNumberOfTouristsForTour(tourId);
        }

        public void Update(Tour tour)
        {
            tourRepository.Update(tour);
        }

        public void Delete(int tourId)
        {
            tourRepository.Delete(tourId);
        }

        public List<Tour> GetTodayTours()
        {
            var tours = tourRepository.GetAll();
            string todayDate = DateTime.Now.ToString("yyyy-MM-dd");
            List<Tour> toursWithTodayDate = tours.Where(t => t.StartDateTime.Date == DateTime.Today).ToList();
            var liveTours = liveTourRepository.GetAll();
            var liveToursIds = liveTours.Where(t => !t.IsLive).Select(t => t.TourId).ToList();
            toursWithTodayDate.RemoveAll(t => liveToursIds.Contains(t.Id));

            return toursWithTodayDate;
        }

        public List<Tour> GetUpcomingTours()
        {
            var tours = tourRepository.GetAll();
            DateTime today = DateTime.Today;
            List<Tour> upcomingTours = tours.Where(t => t.StartDateTime.Date > today).ToList();
            return upcomingTours;
        }

        public Tour GetTourById(int tourId)
        {
            var tours = tourRepository.GetAll();
            return tours.FirstOrDefault(t => t.Id == tourId);
        }

        public List<string> GetCitiesCountriesFromCSV(string filePath, int maxLines)
        {
            string[] lines = File.ReadAllLines(filePath);
            List<string> locations = new List<string>();

            for (int i = 0; i < maxLines && i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(',');
                if (parts.Length == 2)
                {
                    string location = $"{parts[0].Trim()}, {parts[1].Trim()}";
                    locations.Add(location);
                }
            }

            return locations;
        }
        public List<Tour> GetUnBookedToursInCity(string City)
        {
            List<Tour> unBookedToursInCity = tourRepository.GetAll()
                .Where(t => t.Location.City.Equals(City, StringComparison.OrdinalIgnoreCase))
                .ToList();
            unBookedToursInCity.RemoveAll(t => t.MaxTouristsNumber <= 0);
            return unBookedToursInCity;
        }

    }
}
