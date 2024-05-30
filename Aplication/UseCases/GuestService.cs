using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Aplication.UseCases
{
    public class GuestService
    {
        public IGuestRepository guestRepository;
        public IGuestNotificationRepository guestNotificationRepository;
        public IPropertyReservationRepository propertyReservationRepository;
        public GuestService(IGuestRepository guestRepository, IGuestNotificationRepository guestNotificationRepository, IPropertyReservationRepository propertyReservationRepository)
        {
            this.guestRepository = guestRepository;
            this.guestNotificationRepository = guestNotificationRepository;
            this.propertyReservationRepository = propertyReservationRepository;
        }

        public Guest GetGuestById(int id)
        {
            return guestRepository.GetByUserId(id);
        }

        public void CheckSuperGuestStatus(Guest Guest, List<PropertyReservation> GuestsReservations)
        {
            GuestsReservations = propertyReservationRepository.GetAll().FindAll(r => r.StartDate >= DateTime.Now.AddDays(-365) && r.GuestId == Guest.Id);
            if (GuestsReservations.Count() >= 10)
            {
                Guest.IsSuperGuest = true;
                Guest.SuperGuestStartDate = DateTime.Now;
                Guest.Points = 5;
                GuestNotification GuestNotification = new GuestNotification()
                {
                    GuestId = Guest.Id,
                    Message = "Congratulations! You have become a Super Guest. You have won 5 points that you can use as a discount on bookings in the next year!",
                    Read = false
                };
                guestNotificationRepository.AddNotification(GuestNotification);
            }
            else
            {
                Guest.IsSuperGuest = false;
                Guest.Points = 0;
            }
            guestRepository.Update(Guest);

        }

        public void CheckSuperGuestExpiryDate(Guest Guest, List<PropertyReservation> GuestsReservations)
        {
            if (Guest.IsSuperGuest == false || DateTime.Now >= Guest.SuperGuestStartDate.AddYears(1))
            {
                CheckSuperGuestStatus(Guest, GuestsReservations);
            }
        }
    }

    
}
