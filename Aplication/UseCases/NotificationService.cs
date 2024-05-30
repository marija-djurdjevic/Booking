using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Models;
using BookingApp.Aplication.Dto;
using BookingApp.Repositories;
using BookingApp.Domain.RepositoryInterfaces;


namespace BookingApp.Aplication.UseCases
{
    public class NotificationService
    {
        private static int _lastNotifiedForumId = 0;
        ReviewRepository reviewRepository = new ReviewRepository();
        private readonly ForumRepository _forumRepository = new ForumRepository();
        private readonly GuestRepository _guestRepository = new GuestRepository();
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
        /* public List<Notification> GetNewForumPosts()
         {
             var newForumPosts = _forumRepository.GetAll()
                 .Where(f => f.Id > _lastNotifiedForumId && !f.IsClosed)
                 .Select(f => new Notification($"New forum post by guest {f.GuestId} in {f.Location.City}, {f.Location.Country}", f.GuestId, f.Comment))
                 .ToList();

             if (newForumPosts.Any())
             {
                 _lastNotifiedForumId = _forumRepository.GetAll().Max(f => f.Id);
             }

             return newForumPosts;
         }*/
        public List<Notification> GetNewForumPosts()
        {
            var newForumPosts = _forumRepository.GetAll()
                .Where(f => f.Id > _lastNotifiedForumId && !f.IsClosed)
            .Select(f =>
            {
                    var guest = _guestRepository.GetByUserId(f.GuestId);
                    var guestName = guest != null ? $"{guest.FirstName} {guest.LastName}" : $"Guest {f.GuestId}";
                    return new Notification($"New forum post by {guestName} in {f.Location.City}, {f.Location.Country}", f.GuestId, f.Comment);
                })
                .ToList();

            if (newForumPosts.Any())
            {
                _lastNotifiedForumId = _forumRepository.GetAll().Max(f => f.Id);
            }

            return newForumPosts;
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
