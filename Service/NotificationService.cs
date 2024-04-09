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
            List<PropertyReservation> reservations = propertyReservationRepository.GetAllPropertyReservation();

            foreach (var reservation in reservations)
            {
                if (!HasReviewForReservation(reviews, reservation))
                {
                    int remainingDays = CalculateRemainingDays(reservation.EndDate);
                    Notification notification = CreateNotification(reservation, remainingDays);
                    notifications.Add(notification);
                }
            }

            return notifications;
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
