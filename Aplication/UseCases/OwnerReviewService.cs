using BookingApp.Aplication.Dto;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BookingApp.Aplication.UseCases
{
    public class OwnerReviewService
    {
        private readonly IOwnerReviewRepository ownerReviewRepository;

        public OwnerReviewService(IOwnerReviewRepository ownerReviewRepository)
        {
            this.ownerReviewRepository = ownerReviewRepository;
        }

        public void SaveReview(OwnerReviewDto ownerReview)
        {
            ownerReviewRepository.AddOwnerReview(ownerReview.ToOwnerReview());
        }
        public ObservableCollection<KeyValuePair<OwnerReview, PropertyReservation>> LoadOwnerReviewsWithReservations()
        {
            return ownerReviewRepository.LoadOwnerReviewsWithReservations();
        }
    }
}
