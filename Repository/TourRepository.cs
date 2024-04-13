using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class TourRepository
    {
        private const string FilePath = "../../../Resources/Data/tours.csv";

        private readonly Serializer<Tour> _serializer;

        private List<Tour> tours;

        private KeyPointRepository keyPoints;

        public TourRepository()
        {
            _serializer = new Serializer<Tour>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }
            keyPoints = new KeyPointRepository();
            tours = GetAll();
        }

        public Tour Save(Tour tour)
        {
            tours = GetAll();
            int nextId = NextId();
            tour.Id = nextId;
            tours.Add(tour);
            _serializer.ToCSV(FilePath, tours);
            return tour;
        }

        public void Update(Tour updatedTour)
        {
            tours = GetAll();
            Tour existingTour = tours.FirstOrDefault(t => t.Id == updatedTour.Id);
            if (existingTour != null)
            {
                int index = tours.IndexOf(existingTour);
                tours[index] = updatedTour;
                _serializer.ToCSV(FilePath, tours);
            }
        }

        public void Delete(int tourId)
        {
            tours = GetAll();
            Tour existingTour = tours.FirstOrDefault(t => t.Id == tourId);
            if (existingTour != null)
            {
                tours.Remove(existingTour);
                _serializer.ToCSV(FilePath, tours);
            }
        }

        public List<Tour> GetAll()
        {
            tours = _serializer.FromCSV(FilePath);
            foreach (Tour tour in tours)
            {
                tour.KeyPoints = keyPoints.GetTourKeyPoints(tour.Id);
            }
            return tours;
        }

        public List<Tour> GetTodayTours()
        {
            tours = GetAll();
            string todayDate = DateTime.Now.ToString("yyyy-MM-dd");
            List<Tour> toursWithTodayDate = tours.Where(t => t.StartDateTime.ToString("yyyy-MM-dd").StartsWith(todayDate)).ToList();
            return toursWithTodayDate;
        }

        public List<Tour> GetUpcomingTours()
        {
            tours = GetAll();
            DateTime today = DateTime.Today;
            List<Tour> upcomingTours = tours.Where(t => t.StartDateTime.Date > today).ToList();
            return upcomingTours;
        }



        public Tour GetTourById(int tourId)
        {
            tours = GetAll();
            return tours.FirstOrDefault(t => t.Id == tourId);
        }


        public int NextId()
        {
            tours = _serializer.FromCSV(FilePath);
            if (tours.Count < 1)
            {
                return 1;
            }
            return tours.Max(t => t.Id) + 1;
        }

        public List<Tour> GetUnBookedToursInCity(String City)
        {
            List<Tour> unBookedToursInCity=GetAll()
                .Where(t => t.Location.City.Equals(City, StringComparison.OrdinalIgnoreCase))
                .ToList();
            unBookedToursInCity.RemoveAll(t => t.MaxTouristsNumber <= 0);
            return unBookedToursInCity;
        }
    }
}
