using BookingApp.Domain.Models;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Repositories
{
    public class RenovationRepository : IRenovationRepository
    {
        private const string FilePath = "../../../Resources/Data/renovations.csv";

        private readonly Serializer<Renovation> _serializer;

        private List<Renovation> _renovations;

        public int idd;

        public RenovationRepository()
        {
            _serializer = new Serializer<Renovation>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            _renovations = _serializer.FromCSV(FilePath);
        }

        public Renovation AddRenovation(Renovation renovation)
        {
            int nextId = NextId();
            renovation.Id = nextId;
            _renovations.Add(renovation);
            _serializer.ToCSV(FilePath, _renovations);
            return renovation;
        }

        public void UpdateRenovation(Renovation updatedRenovation)
        {
            Renovation existingRenovation = _renovations.FirstOrDefault(t => t.Id == updatedRenovation.Id);
            if (existingRenovation != null)
            {
                int index = _renovations.IndexOf(existingRenovation);
                _renovations[index] = updatedRenovation;
                _serializer.ToCSV(FilePath, _renovations);
            }
        }

        public void DeleteRenovation(int renovationId)
        {
            Renovation existingRenovation = _renovations.FirstOrDefault(t => t.Id == renovationId);
            if (existingRenovation != null)
            {
                _renovations.Remove(existingRenovation);
                _serializer.ToCSV(FilePath, _renovations);
            }
        }

        public List<Renovation> GetAllRenovations()
        {
            return _renovations;
        }

        public Renovation GetRenovationById(int renovationId)
        {
            return _renovations.FirstOrDefault(t => t.Id == renovationId);
        }


        public void SaveChanges()
        {
            _serializer.ToCSV(FilePath, _renovations);
        }

        public int NextId()
        {
            if (_renovations.Count < 1)
            {
                return 1;
            }
            return _renovations.Max(t => t.Id) + 1;
        }


        public int GetRenovationId(Renovation renovation)
        {
            return renovation.Id;
        }
    }
}
