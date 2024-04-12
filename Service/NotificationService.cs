using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Dto;
using BookingApp.Model;
using BookingApp.Repository;


namespace BookingApp.Service
{
    public class NotificationService
    {
        ReviewRepository reviewRepository = new ReviewRepository();
        PropertyReservationRepository propertyReservationRepository = new PropertyReservationRepository();
        public List<Notification> GetUnratedGuests()
        {
            List<Notification> notifications = new List<Notification>();

            List<Review> reviews = reviewRepository.GetAllReviews();
            List<PropertyReservation> reservations = propertyReservationRepository.GetAll();

            foreach (var reservation in reservations)
            {
                if (!HasReviewForReservation(reviews, reservation) && IsReservationExpired(reservation))
                {
                    int remainingDays = CalculateRemainingDays(reservation.EndDate);
                    Notification notification = CreateNotification(reservation, remainingDays);
                    notifications.Add(notification);
                }
            }

            return notifications;
        }
        private bool IsReservationExpired(PropertyReservation reservation)
        {
            // Provjerite da li je rezervacija istekla
            if (reservation.EndDate < DateTime.Today)
            {
                // Provjerite da li nije prošlo više od 5 dana od isteka rezervacije
                TimeSpan difference = DateTime.Today - reservation.EndDate;
                return difference.Days <= 5;
            }
            return false;
        }
        public List<Notification> GetCanceledReservations()
        {
            List<Notification> notifications = new List<Notification>();

            var reservations = propertyReservationRepository.GetAll();
            var canceledReservations = reservations.Where(r => r.Canceled).ToList();

            foreach (var reservation in canceledReservations)
            {
                int remainingDays = CalculateRemainingDays(reservation.EndDate);
                Notification notification = CreateCancellationNotification(reservation);
                notifications.Add(notification);
            }

            return notifications;
        }
        public Notification CreateCancellationNotification(PropertyReservation reservation)
        {
            string guestName = $"{reservation.GuestFirstName} {reservation.GuestLastName}";
            return new Notification(guestName, reservation.StartDate);
        }
        private bool HasReviewForReservation(List<Review> reviews, PropertyReservation reservation)
        {
            return reviews.Any(review => review.ReservationId == reservation.Id);
        }

        private int CalculateRemainingDays(DateTime endDate)
        {
            return (int)(endDate.Date - DateTime.Now.Date).TotalDays;
        }

        private Notification CreateNotification(PropertyReservation reservation, int remainingDays)
        {
            return new Notification(reservation, remainingDays);
        }
    }
}
