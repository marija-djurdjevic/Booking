using BookingApp.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class Notification
    {
        public int GuestId { get; set; }
        public string GuestName { get; set; }
        public DateTime ReservationDate { get; set; }
        public int DaysRemaining { get; set; } 

        public Notification(int guestId, string guestName, DateTime reservationDate, int daysRemaining)
        {
            GuestId = guestId;
            GuestName = guestName;
            ReservationDate = reservationDate;
            DaysRemaining = daysRemaining;
        }
        public Notification(string guestName, DateTime reservationDate, int daysRemaining)
        {
           
            GuestName = guestName;
            ReservationDate = reservationDate;
            DaysRemaining = daysRemaining;
        }
        public Notification(PropertyReservation reservation, int daysRemaining)
        {
            GuestName = $"{reservation.GuestFirstName} {reservation.GuestLastName}";
            DaysRemaining = daysRemaining;
        }
    }

}
