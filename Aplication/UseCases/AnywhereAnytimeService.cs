using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Aplication.UseCases
{
    public class AnywhereAnytimeService
    {
        private readonly IPropertyRepository propertyRepository;
        private readonly IPropertyReservationRepository propertyReservationRepository;
        private readonly IReservedDateRepository reservedDateRepository;
        private readonly IGuestRepository guestRepository;
        private readonly IGuestNotificationRepository guestNotificationRepository;
        private readonly IRenovationRepository renovationRepository;

        public AnywhereAnytimeService(
            IPropertyRepository propertyRepository,
            IPropertyReservationRepository propertyReservationRepository,
            IReservedDateRepository reservedDateRepository,
            IGuestRepository guestRepository,
            IGuestNotificationRepository guestNotificationRepository,
            IRenovationRepository renovationRepository)
        {
            this.propertyRepository = propertyRepository;
            this.propertyReservationRepository = propertyReservationRepository;
            this.reservedDateRepository = reservedDateRepository;
            this.guestRepository = guestRepository;
            this.guestNotificationRepository = guestNotificationRepository;
            this.renovationRepository = renovationRepository;
        }

        public List<Renovation> GetAllRenovations()
        {
            return renovationRepository.GetAllRenovations();

        }

        public Property GetById(int id)
        {
            return propertyRepository.GetPropertyById(id);
        }

        public List<Property> GetAllProperties()
        {
            return propertyRepository.GetAllProperties();
        }

        public List<ReservedDate> GetReservedDateById(int id)
        {
            return reservedDateRepository.GetReservedDatesByPropertyId(id);
        }

        public List<PropertyReservation> GetAllPropertyReservations()
        {
            return propertyReservationRepository.GetAll();
        }

        public void AddNotification(GuestNotification notification)
        {
            guestNotificationRepository.AddNotification(notification);
        }

        public void UpdateGuest(Guest guest)
        {
            guestRepository.Update(guest);
        }

        public int GetNextIdPR()
        {
            return propertyReservationRepository.NextId();
        }

        public void AddReservedDate(ReservedDate reservedDate)
        {
            reservedDateRepository.AddReservedDate(reservedDate);
        }

        public void UpdateProperty(Property property)
        {
            propertyRepository.UpdateProperty(property);
        }

        public void AddPropertyReservation(PropertyReservation reservation)
        {
            propertyReservationRepository.AddPropertyReservation(reservation);
        }
    }
    
}
