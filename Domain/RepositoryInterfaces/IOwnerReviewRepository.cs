using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IOwnerReviewRepository
    {
        OwnerReview AddOwnerReview(OwnerReview ownerReview);
        List<OwnerReview> GetAllReviews();
        OwnerReview GetReviewById(int ownerReviewId);
        int NextId();
        int GetReviewId(OwnerReview OwnerReview);
        List<OwnerReview> GetReviewsByOwnerId(int ownerId);
        double CalculateAverageRating(int ownerId);
        ObservableCollection<KeyValuePair<OwnerReview, PropertyReservation>> LoadOwnerReviewsWithReservations();
    }
}
