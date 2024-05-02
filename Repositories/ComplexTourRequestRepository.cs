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
    internal class ComplexTourRequestRepository : IComplexTourRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/complexTourRequest.csv";

        private readonly Serializer<ComplexTourRequest> _serializer;

        private List<ComplexTourRequest> complexTourRequests;

        public ComplexTourRequestRepository()
        {
            _serializer = new Serializer<ComplexTourRequest>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            complexTourRequests = _serializer.FromCSV(FilePath);
        }

        public void Delete(int id)
        {
            complexTourRequests = GetAll();
            ComplexTourRequest existingTourRequest = complexTourRequests.FirstOrDefault(t => t.Id == id);
            if (existingTourRequest != null)
            {
                complexTourRequests.Remove(existingTourRequest);
                _serializer.ToCSV(FilePath, complexTourRequests);
            }
        }

        public List<ComplexTourRequest> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public ComplexTourRequest GetById(int id)
        {
            complexTourRequests = GetAll();
            return complexTourRequests.FirstOrDefault(t => t.Id == id);
        }

        public int NextId()
        {
            complexTourRequests = _serializer.FromCSV(FilePath);
            if (complexTourRequests.Count < 1)
            {
                return 1;
            }
            return complexTourRequests.Max(t => t.Id) + 1;
        }

        public int Save(ComplexTourRequest tourRequest)
        {
            complexTourRequests = GetAll();
            tourRequest.Id = NextId();
            complexTourRequests.Add(tourRequest);
            _serializer.ToCSV(FilePath, complexTourRequests);
            return tourRequest.Id;
        }

        public void Update(ComplexTourRequest updatedTourRequest)
        {
            complexTourRequests = GetAll();
            ComplexTourRequest existingTourRequest = complexTourRequests.FirstOrDefault(t => t.Id == updatedTourRequest.Id);
            if (existingTourRequest != null)
            {
                int index = complexTourRequests.IndexOf(existingTourRequest);
                complexTourRequests[index] = updatedTourRequest;
                _serializer.ToCSV(FilePath, complexTourRequests);
            }
        }
    }
}
