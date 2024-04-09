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


        private void SaveChanges()
        {
            _serializer.ToCSV(FilePath, tours);
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


        public List<Tour> GetMatchingTours(TourDto searchParams)
        {
            tours = GetAll();
            List<Tour> matchingTours = tours.Where(t => 
            IsCityMatch(t, searchParams) && 
            IsCountryMatch(t, searchParams) && 
            IsDurationMatch(t, searchParams) && 
            IsLanguageMatch(t, searchParams) && 
            IsMaxTouristNumberMatch(t, searchParams)).ToList();
            return matchingTours;
        }

        public bool IsCityMatch(Tour t, TourDto searchParams)
        {
            return string.IsNullOrEmpty(searchParams.LocationDto.City) || t.Location.City.ToLower().Contains(searchParams.LocationDto.City.ToLower());
        }

        public bool IsCountryMatch(Tour t, TourDto searchParams)
        {
            return string.IsNullOrEmpty(searchParams.LocationDto.Country) || t.Location.Country.ToLower().Contains(searchParams.LocationDto.Country.ToLower());
        }

        public bool IsDurationMatch(Tour t, TourDto searchParams)
        {
            return searchParams.Duration == 0 || t.Duration == searchParams.Duration;
        }

        public bool IsLanguageMatch(Tour t, TourDto searchParams)
        {
            return string.IsNullOrEmpty(searchParams.Language) || t.Language.ToLower().Contains(searchParams.Language.ToLower());
        }

        public bool IsMaxTouristNumberMatch(Tour t, TourDto searchParams)
        {
            return searchParams.MaxTouristNumber == 0 || (t.MaxTouristsNumber >= searchParams.MaxTouristNumber && searchParams.MaxTouristNumber > 0);
        }

        public List<Tour> GetUnBookedToursInCity(String City)
        {
            List<Tour> unBookedToursInCity=GetAll()
                .Where(t => t.Location.City.Equals(City, StringComparison.OrdinalIgnoreCase))
                .ToList();
            unBookedToursInCity.RemoveAll(t => t.MaxTouristsNumber <= 0);
            return unBookedToursInCity;
        }

        public List<Tour> GetMyReserved(int userId)
        {
            TourReservationRepository tourReservationRepository = new TourReservationRepository();
            List<Tour> myReservedTours=new List<Tour>();
            foreach(TourReservation tourReservation in tourReservationRepository.GetByUserId(userId))
            {
                myReservedTours.Add(GetTourById(tourReservation.TourId));
            }
            return myReservedTours.DistinctBy(x=>x.Id).ToList();
        }

        public List<Tour> GetMyActiveReserved(int userId)
        {
            TourReservationRepository tourReservationRepository = new TourReservationRepository();
            LiveTourRepository liveTourRepository = new LiveTourRepository();
            List<Tour> myActiveReservedTours = new List<Tour>();
            foreach (TourReservation tourReservation in tourReservationRepository.GetByUserId(userId))
            {
                LiveTour liveTour = liveTourRepository.GetLiveTourById(tourReservation.TourId);
                if (tourReservation.IsOnTour)
                {
                    Tour activeTour = GetTourById(tourReservation.TourId);
                    activeTour.KeyPoints = liveTour.KeyPoints;
                    myActiveReservedTours.Add(activeTour);
                }
                    
            }
            return myActiveReservedTours.DistinctBy(x => x.Id).ToList();
        }
    }
}
