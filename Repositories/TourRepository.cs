using BookingApp.Domain.Models;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookingApp.Domain.RepositoryInterfaces;
using System.Threading.Tasks;

namespace BookingApp.Repositories
{
    public class TourRepository : ITourRepository
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

        public List<Tour> GetAll()
        {
            tours = _serializer.FromCSV(FilePath);
            foreach (Tour tour in tours)
            {
                tour.KeyPoints = keyPoints.GetTourKeyPoints(tour.Id);
            }
            return tours;
        }

        public Tour GetById(int tourId)
        {
            tours = GetAll();
            return tours.FirstOrDefault(t => t.Id == tourId);
        }

        public void Save(Tour tour)
        {
            tours = GetAll();
            int nextId = NextId();
            tour.Id = nextId;
            tours.Add(tour);
            _serializer.ToCSV(FilePath, tours);
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

        public int NextId()
        {
            tours = _serializer.FromCSV(FilePath);
            if (tours.Count < 1)
            {
                return 1;
            }
            return tours.Max(t => t.Id) + 1;
        }
    }
}
