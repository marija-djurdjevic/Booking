using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repositories
{
    internal class TourRequestRepository : ITourRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/tourRequest.csv";

        private readonly Serializer<TourRequest> _serializer;

        private List<TourRequest> tourRequests;

        public TourRequestRepository()
        {
            _serializer = new Serializer<TourRequest>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            tourRequests = _serializer.FromCSV(FilePath);
        }

        public void Delete(int id)
        {
            tourRequests = GetAll();
            TourRequest existingTourRequest = tourRequests.FirstOrDefault(t => t.Id == id);
            if (existingTourRequest != null)
            {
                tourRequests.Remove(existingTourRequest);
                _serializer.ToCSV(FilePath, tourRequests);
            }
        }

        public List<TourRequest> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public TourRequest GetById(int id)
        {
            tourRequests = GetAll();
            return tourRequests.FirstOrDefault(t => t.Id == id);
        }

        public int NextId()
        {
            tourRequests = _serializer.FromCSV(FilePath);
            if (tourRequests.Count < 1)
            {
                return 1;
            }
            return tourRequests.Max(t => t.Id) + 1;
        }

        public void Save(TourRequest tourRequest)
        {
            tourRequests = GetAll();
            tourRequest.Id = NextId();
            tourRequests.Add(tourRequest);
            _serializer.ToCSV(FilePath, tourRequests);
        }

        public void Update(TourRequest updatedTourRequest)
        {
            tourRequests = GetAll();
            TourRequest existingTourRequest = tourRequests.FirstOrDefault(t => t.Id == updatedTourRequest.Id);
            if (existingTourRequest != null)
            {
                int index = tourRequests.IndexOf(existingTourRequest);
                tourRequests[index] = updatedTourRequest;
                _serializer.ToCSV(FilePath, tourRequests);
            }
        }
    }
}
