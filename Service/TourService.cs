using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.Service
{
    public class TourService
    {
        private readonly TourRepository _tourRepository;
        private readonly KeyPointService _keyPointService;
        private readonly TouristExperienceService _touristExperienceService;

        public TourService()
        {
            _tourRepository = new TourRepository();
            _keyPointService = new KeyPointService();
            _touristExperienceService = new TouristExperienceService();
        }

        public List<Tour> GetToursWithKeyPoints()
        {
            var tours = _tourRepository.GetAll();
            foreach (var tour in tours)
            {
                tour.KeyPoints = _keyPointService.GetTourKeyPoints(tour.Id);
            }
            return tours;
        }

        public int GetNumberOfTouristsForTour(int tourId)
        {
            return _touristExperienceService.GetNumberOfTouristsForTour(tourId);
        }



        public List<Tour> GetTodayTours()
        {
            var tours = _tourRepository.GetAll();
            string todayDate = DateTime.Now.ToString("yyyy-MM-dd");
            List<Tour> toursWithTodayDate = tours.Where(t => t.StartDateTime.Date == DateTime.Today).ToList();
            return toursWithTodayDate;
        }

        public List<Tour> GetUpcomingTours()
        {
            var tours = _tourRepository.GetAll();
            DateTime today = DateTime.Today;
            List<Tour> upcomingTours = tours.Where(t => t.StartDateTime.Date > today).ToList();
            return upcomingTours;
        }




        public Tour GetTourById(int tourId)
        {
            var tours = _tourRepository.GetAll();
            return tours.FirstOrDefault(t => t.Id == tourId);
        }




    }
}
