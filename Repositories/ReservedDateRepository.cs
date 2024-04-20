using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;
using System.Collections.Generic;

namespace BookingApp.Repositories
{
    public class ReservedDateRepository : IReservedDateRepository
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

        public void Delete(int reservationId)
        {
            reservedDates = GetAllReservedDates();
            reservedDates.RemoveAll(t => t.ReservationId == reservationId);
            _serializer.ToCSV(FilePath, reservedDates);
        }


        public void SaveChanges()
        {
            _serializer.ToCSV(FilePath, reservedDates);
        }
    }
}
