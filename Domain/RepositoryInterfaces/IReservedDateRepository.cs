using BookingApp.Domain.Models;
using System.Collections.Generic;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IReservedDateRepository
    {
        void AddReservedDate(ReservedDate _reservedDate);
        List<ReservedDate> GetAllReservedDates();
        List<ReservedDate> GetReservedDatesByPropertyId(int propertyId);
        void Delete(int reservationId);
        void SaveChanges();
    }
}
