using BookingApp.Model;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace BookingApp.Repository
{
    public class LiveTourRepository
    {
        private const string FilePath = "../../../Resources/Data/liveTour.csv";
        private readonly Serializer<LiveTour> _serializer;
        private List<LiveTour> liveTours;

        public LiveTourRepository()
        {
            _serializer = new Serializer<LiveTour>();

            if (!File.Exists(FilePath))
            {
                File.Create(FilePath).Close();
            }

            liveTours = _serializer.FromCSV(FilePath);
        }

        public void AddOrUpdateLiveTour(LiveTour liveTour)
        {
            var existingTour = liveTours.FirstOrDefault(t => t.TourId == liveTour.TourId);
            if (existingTour != null)
            {
                existingTour.KeyPoints = liveTour.KeyPoints;
                existingTour.IsLive = liveTour.IsLive;
            }
            else
            {
                liveTours.Add(liveTour);
            }

            SaveChanges();
        }

        public void RemoveLiveTour(int tourId)
        {
            liveTours.RemoveAll(t => t.TourId == tourId);
            SaveChanges();
        }

        public void SaveChanges()
        {
            _serializer.ToCSV(FilePath, liveTours);
        }

        public List<LiveTour> GetAllLiveTours()
        {
            return liveTours;
        }


        public List<int> GetFinishedTourIds()
        {
            return liveTours.Where(t => !t.IsLive).Select(t => t.TourId).ToList();
        }



        public LiveTour GetLiveTourById(int tourId)
        {
            return liveTours.FirstOrDefault(t => t.TourId == tourId);
        }

        public bool IsActiveTour()
        {
            return liveTours.Any(t => t.IsLive);
        }

        public void ActivateTour(int tourId)
        {
            var liveTour = liveTours.FirstOrDefault(t => t.TourId == tourId && !t.IsLive);
            if (liveTour != null)
            {
                liveTour.IsLive = true;
                SaveChanges();
            }
        }
    }
}
