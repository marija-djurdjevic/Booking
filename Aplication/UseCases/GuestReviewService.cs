using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BookingApp.Aplication.UseCases
{
    public class GuestReviewService
    {
        private readonly IReviewRepository reviewRepository;
        public IPropertyReservationRepository propertyReservationRepository { get; set; }
        public IOwnerReviewRepository ownerReviewRepository { get; set; }

        public GuestReviewService(IReviewRepository reviewRepository, IPropertyReservationRepository propertyReservationRepository, IOwnerReviewRepository ownerReviewRepository)
        {
            this.reviewRepository = reviewRepository;
            this.propertyReservationRepository = propertyReservationRepository;
            this.ownerReviewRepository = ownerReviewRepository;
        }

        public List<OwnerReview> GetOwnerReviews()
        {
            return ownerReviewRepository.GetAllReviews(); 
        }

        public List<Review> GetGuestReviews()
        {
            return reviewRepository.GetAllReviews();    
        }
        public List<Review> FindAAppropriateReviews(List<Review> AllGuestReviews, List<OwnerReview> OwnerReviews)
        {
            OwnerReviews = GetOwnerReviews();
            AllGuestReviews = GetGuestReviews();
            List<Review> GuestReviews = new List<Review>();
            foreach (var review in AllGuestReviews)
            {
                foreach (var ownerreview in OwnerReviews)
                {
                    if (review.ReservationId == ownerreview.ReservationId && GuestReviews.Contains(review) == false)
                    {
                        GuestReviews.Add(review);
                    }

                }
            }

            return GuestReviews;
        }

        public void MakeReviewPairs(List<Review> GuestReviews, ObservableCollection<KeyValuePair<Review, PropertyReservation>> PropertiesReviews)
        {
            foreach (var review in GuestReviews)
            {
                var propertyReservation = propertyReservationRepository.GetAll().FirstOrDefault(p => p.Id == review.ReservationId);
                PropertiesReviews.Add(new KeyValuePair<Review, PropertyReservation>(review, propertyReservation));
            }
        }

        public double CalculateAvarageCleanlinessScore(List<Review> GuestReviews)
        {
            return GuestReviews.Any() ? Math.Round(GuestReviews.Average(r => r.Cleanliness), 2) : 0;

        }

        public double CalculateAvarageRulesScore(List<Review> GuestReviews)
        {
            return GuestReviews.Any() ? Math.Round(GuestReviews.Average(r => r.Rules), 2) : 0;

        }
    }
}
