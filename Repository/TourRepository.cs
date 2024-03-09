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

        private List<Tour> _tours;

        public int idd;

        public TourRepository()
        {
            _serializer = new Serializer<Tour>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            _tours = _serializer.FromCSV(FilePath);
        }

        public Tour AddTour(Tour tour)
        {
            int nextId = NextId();
            tour.Id = nextId;
            _tours.Add(tour);
            _serializer.ToCSV(FilePath, _tours);
            return tour;
        }

        public void UpdateTour(Tour updatedTour)
        {
            Tour existingTour = _tours.FirstOrDefault(t => t.Id == updatedTour.Id);
            if (existingTour != null)
            {
                int index = _tours.IndexOf(existingTour);
                _tours[index] = updatedTour;
                _serializer.ToCSV(FilePath, _tours);
            }
        }

        public void DeleteTour(int tourId)
        {
            Tour existingTour = _tours.FirstOrDefault(t => t.Id == tourId);
            if (existingTour != null)
            {
                _tours.Remove(existingTour);
                _serializer.ToCSV(FilePath, _tours);
            }
        }

        public List<Tour> GetAllTours()
        {
            return _tours;
        }

        public Tour GetTourById(int tourId)
        {
            return _tours.FirstOrDefault(t => t.Id == tourId);
        }


        private void SaveChanges()
        {
            _serializer.ToCSV(FilePath, _tours);
        }

        public int NextId()
        {
            if (_tours.Count < 1)
            {
                return 1;
            }
            return _tours.Max(t => t.Id) + 1;
        }


        public int GetTourId(Tour tour)
        {
            return tour.Id;
        }
    }
}
