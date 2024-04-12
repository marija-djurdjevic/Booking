using BookingApp.Dto;
using BookingApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    
    public class Notification
    {
       
        public string Message { get; }

     
         public int GuestId { get; set; }
         public string GuestName { get; set; }
         public DateTime ReservationDate { get; set; }
         public int DaysRemaining { get; set; }
        // public string NotificationType { get; set; }

         public Notification(int guestId, string guestName, DateTime reservationDate, int daysRemaining)
         {
             GuestId = guestId;
             GuestName = guestName;
             ReservationDate = reservationDate;
             DaysRemaining = daysRemaining;
            // NotificationType = notificationType;
         }
        public Notification(string guestName, DateTime reservationDate)
        {
            
            Message = $"Guest {guestName} canceled a reservation on {reservationDate.ToShortDateString()}.";
        }
        public Notification(string guestName, DateTime reservationDate, int daysRemaining)
         {

             GuestName = guestName;
             ReservationDate = reservationDate;
             DaysRemaining = daysRemaining;
            // NotificationType = notificationType;
         }
         public Notification(PropertyReservation reservation, int daysRemaining)
         {
            GuestName = $"{reservation.GuestFirstName} {reservation.GuestLastName}";
            Message = $"Guest {GuestName} is not rated, {daysRemaining} days remaining.";
           
         }
        

    }

}
