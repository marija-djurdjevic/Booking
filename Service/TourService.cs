using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.ViewModel.TouristViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace BookingApp.Service
{
    public class TourService
    {
        private TourRepository tourRepository;
        private LiveTourRepository liveTourRepository;
        private readonly KeyPointService keyPointService;
        private readonly TouristExperienceService touristExperienceService;

        public TourService()
        {
            tourRepository = new TourRepository();
            liveTourRepository = new LiveTourRepository();
            keyPointService = new KeyPointService();
            touristExperienceService = new TouristExperienceService();
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

        public int GetNumberOfTouristsForTour(int tourId)
        {
            return touristExperienceService.GetNumberOfTouristsForTour(tourId);
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
            var liveTours=liveTourRepository.GetAll();
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

    }
}
