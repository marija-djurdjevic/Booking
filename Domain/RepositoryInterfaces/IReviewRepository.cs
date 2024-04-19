using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IReviewRepository
    {
        Review AddReview(Review review);
        void UpdateReview(Review updatedReview);
        void DeleteReview(int reviewId);
        List<Review> GetAllReviews();
        Review GetReviewById(int reviewId);
        void SaveChanges();
        int NextId();
        int GetReviewId(Review review);
    }
}
