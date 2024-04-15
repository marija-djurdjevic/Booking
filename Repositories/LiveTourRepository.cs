using BookingApp.Domain.Models;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace BookingApp.Repositories
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
                Update(liveTour);
            }
            else
            {
                liveTours.Add(liveTour);
            }
            SaveChanges();
        }


        public List<LiveTour> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }


        public LiveTour Update(LiveTour liveTour)
        {
            liveTours = _serializer.FromCSV(FilePath);
            LiveTour current = liveTours.Find(c => c.TourId == liveTour.TourId);
            int index = liveTours.IndexOf(current);
            liveTours.Remove(current);
            liveTours.Insert(index, liveTour);
            _serializer.ToCSV(FilePath, liveTours);
            return liveTour;
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
            liveTours = GetAll().Where(t => t.IsLive).ToList();
            return liveTours;
        }

        public List<LiveTour> GetFinishedTours()
        {
            return liveTours.Where(t => !t.IsLive).ToList();
        }

        public List<int> GetFinishedTourIds()
        {
            return liveTours.Where(t => !t.IsLive).Select(t => t.TourId).ToList();
        }



        public LiveTour GetLiveTourById(int tourId)
        {
            return liveTours.FirstOrDefault(t => t.TourId == tourId);
        }


        public LiveTour FindLiveTourById(int tourId)
        {
            return liveTours.FirstOrDefault(t => t.TourId == tourId && t.IsLive);
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
                Update(liveTour);
            }
        }
    }
}
