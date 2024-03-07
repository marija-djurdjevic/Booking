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

        public TourRepository()
        {
            _serializer = new Serializer<Tour>();
            _tours = _serializer.FromCSV(FilePath);
        }

        public Tour AddTour(string name, string description, string language, int maxTouristsNumber, List<DateTime> tourStartDates, double duration, List<string> imagesPaths, Location location)
        {
            int nextId = NextId();
            Tour newTour = new Tour(nextId, name, description, language, maxTouristsNumber, tourStartDates, duration, imagesPaths, location);
            _tours.Add(newTour);
            _serializer.ToCSV(FilePath, _tours);
            return newTour;
        }

        private int NextId()
        {
            if (_tours.Count < 1)
            {
                return 1;
            }
            return _tours.Max(t => t.Id) + 1;
        }
    }
}
