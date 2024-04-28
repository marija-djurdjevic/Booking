using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace BookingApp.Repositories
{
    public class LiveTourRepository : ILiveTourRepository
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

        public List<LiveTour> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public LiveTour GetById(int tourId)
        {
            liveTours = GetAll();
            return liveTours.FirstOrDefault(t => t.TourId == tourId);
        }

        public void Save(LiveTour liveTour)
        {
            liveTours = GetAll();
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
            _serializer.ToCSV(FilePath, liveTours);
        }

        public void Update(LiveTour liveTour)
        {
            liveTours = _serializer.FromCSV(FilePath);
            LiveTour current = liveTours.Find(c => c.TourId == liveTour.TourId);
            int index = liveTours.IndexOf(current);
            liveTours.Remove(current);
            liveTours.Insert(index, liveTour);
            _serializer.ToCSV(FilePath, liveTours);
        }
        public void Delete(int tourId)
        {
            liveTours = GetAll();
            liveTours.RemoveAll(t => t.TourId == tourId);
            _serializer.ToCSV(FilePath, liveTours);
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
    }
}
