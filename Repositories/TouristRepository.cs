using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.Repositories
{
    public class TouristRepository : ITouristRepository
    {
        private const string FilePath = "../../../Resources/Data/tourists.csv";

        private readonly Serializer<Tourist> _serializer;

        private List<Tourist> tourists;

        public TouristRepository()
        {
            _serializer = new Serializer<Tourist>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            tourists = _serializer.FromCSV(FilePath);
        }

        public List<Tourist> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Tourist GetById(int touristId)
        {
            tourists = GetAll();
            return tourists.FirstOrDefault(t => t.Id == touristId);
        }

        public void Save(Tourist tourist)
        {
            tourists = GetAll();
            tourist.Id = NextId();
            tourists.Add(tourist);
            _serializer.ToCSV(FilePath, tourists);
        }

        public void Update(Tourist updatedTourist)
        {
            tourists = GetAll();
            Tourist existingTourist = tourists.FirstOrDefault(t => t.Id == updatedTourist.Id);
            if (existingTourist != null)
            {
                int index = tourists.IndexOf(existingTourist);
                tourists[index] = updatedTourist;
                _serializer.ToCSV(FilePath, tourists);
            }
        }

        public void Delete(int touristId)
        {
            tourists = GetAll();
            Tourist existingTourist = tourists.FirstOrDefault(t => t.Id == touristId);
            if (existingTourist != null)
            {
                tourists.Remove(existingTourist);
                _serializer.ToCSV(FilePath, tourists);
            }
        }

        public int NextId()
        {
            tourists = _serializer.FromCSV(FilePath);
            if (tourists.Count < 1)
            {
                return 1;
            }
            return tourists.Max(t => t.Id) + 1;
        }
    }
}
