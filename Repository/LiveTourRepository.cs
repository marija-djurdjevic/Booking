using BookingApp.Model;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BookingApp.Repository
{
    public class LiveTourRepository
    {
        private const string FilePath = "../../../Resources/Data/liveTour.csv";
        private readonly Serializer<LiveTour> _serializer;
        private List<LiveTour> _liveTours;

        public LiveTourRepository()
        {
            _serializer = new Serializer<LiveTour>();

            if (!File.Exists(FilePath))
            {
                File.Create(FilePath).Close();
            }

            _liveTours = _serializer.FromCSV(FilePath);
        }

        public void AddOrUpdateLiveTour(LiveTour liveTour)
        {
            var existingTour = _liveTours.FirstOrDefault(t => t.TourId == liveTour.TourId);
            if (existingTour != null)
            {
                existingTour.KeyPoints = liveTour.KeyPoints;
                existingTour.IsLive = liveTour.IsLive;
            }
            else
            {
                _liveTours.Add(liveTour);
            }

            SaveChanges();
        }

        public void RemoveLiveTour(int tourId)
        {
            _liveTours.RemoveAll(t => t.TourId == tourId);
            SaveChanges();
        }

        public void SaveChanges()
        {
            _serializer.ToCSV(FilePath, _liveTours);
        }

        public List<LiveTour> GetAllLiveTours()
        {
            return _liveTours;
        }

        public LiveTour GetLiveTourById(int tourId)
        {
            return _liveTours.FirstOrDefault(t => t.TourId == tourId);
        }

        public bool IsActiveTour()
        {
            return _liveTours.Any(t => t.IsLive);
        }

        public void SetTourAsLive(int tourId)
        {
            var liveTour = _liveTours.FirstOrDefault(t => t.TourId == tourId);
            if (liveTour != null)
            {   
                liveTour.IsLive = true;
                SaveChanges();
            }
        }
    }
}
