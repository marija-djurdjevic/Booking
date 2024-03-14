using BookingApp.Dto;
using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class ReviewRepository
    {
        private const string FilePath = "../../../Resources/Data/reviews.csv";

        private readonly Serializer<Review> _serializer;

        private List<Review> _reviews;

        public int idd;

        public ReviewRepository()
        {
            _serializer = new Serializer<Review>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            _reviews = _serializer.FromCSV(FilePath);
        }

        public Review AddProperty(Review review)
        {
            int nextId = NextId();
            review.Id = nextId;
            _reviews.Add(review);
            _serializer.ToCSV(FilePath, _reviews);
            return review;
        }

        public void UpdateReview(Review updatedReview)
        {
            Review existingReview = _reviews.FirstOrDefault(t => t.Id == updatedReview.Id);
            if (existingReview != null)
            {
                int index = _reviews.IndexOf(existingReview);
                _reviews[index] = updatedReview;
                _serializer.ToCSV(FilePath, _reviews);
            }
        }

        public void DeleteReview(int reviewId)
        {
            Review existingReview = _reviews.FirstOrDefault(t => t.Id == reviewId);
            if (existingReview != null)
            {
                _reviews.Remove(existingReview);
                _serializer.ToCSV(FilePath, _reviews);
            }
        }

        public List<Review> GetAllReviews()
        {
            return _reviews;
        }

        public Review GetReviewById(int reviewId)
        {
            return _reviews.FirstOrDefault(t => t.Id == reviewId);
        }


        private void SaveChanges()
        {
            _serializer.ToCSV(FilePath, _reviews);
        }

        public int NextId()
        {
            if (_reviews.Count < 1)
            {
                return 1;
            }
            return _reviews.Max(t => t.Id) + 1;
        }


        public int GetReviewId(Review review)
        {
            return review.Id;
        }
    }
}

