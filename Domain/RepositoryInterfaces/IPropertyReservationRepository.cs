using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IPropertyReservationRepository
    {
        void AddPropertyReservation(PropertyReservation _propertyReservation);
        List<PropertyReservation> GetAll();
        List<PropertyReservation> GetReservationDataById(int id);
        void Update(PropertyReservation updatedPropertyReservation);
        PropertyReservation GetReservationById(int id);
        void UpdatePropertyReservation(PropertyReservation updatedReservation);
        int NextId();
    }
}
