using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class ReservedDateRepository
    {
        private const string FilePath = "../../../Resources/Data/reserveddates.csv";

        private readonly Serializer<ReservedDate> _serializer;

        private List<ReservedDate> reservedDates;

        public ReservedDateRepository()
        {
            _serializer = new Serializer<ReservedDate>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            reservedDates = _serializer.FromCSV(FilePath);
        }

        public void AddReservedDate(ReservedDate _reservedDate)
        {
            reservedDates.Add(_reservedDate);
            _serializer.ToCSV(FilePath, reservedDates);
        }

        public List<ReservedDate> GetAllReservedDates()
        {
            return reservedDates;
        }

        public List<ReservedDate> GetReservedDatesByPropertyId(int propertyId)
        {
            return reservedDates.FindAll(t => t.PropertyId == propertyId);
        }


        private void SaveChanges()
        {
            _serializer.ToCSV(FilePath, reservedDates);
        }
    }
}
