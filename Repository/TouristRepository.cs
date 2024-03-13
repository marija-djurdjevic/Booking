using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class TouristRepository
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

        public void Save(Tourist tourist)
        {
            tourists = GetAll();
            tourists.Add(tourist);
            _serializer.ToCSV(FilePath, tourists);
        }

        public List<Tourist> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Tourist GetByUserId(int Id)
        {
            tourists = _serializer.FromCSV(FilePath);
            return tourists.Find(t => t.Id == Id);
        }
    }
}
