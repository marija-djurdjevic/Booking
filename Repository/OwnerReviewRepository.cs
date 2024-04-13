using BookingApp.Dto;
using BookingApp.Model;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repository
{
    public class OwnerReviewRepository
    {
        private const string FilePath = "../../../Resources/Data/ownersreviews.csv";

        private readonly Serializer<OwnerReview> _serializer;

        private List<OwnerReview> _ownersreviews;

        public int id;

        public OwnerReviewRepository()
        {
            _serializer = new Serializer<OwnerReview>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            _ownersreviews = _serializer.FromCSV(FilePath);
        }

        public OwnerReview AddOwnerReview(OwnerReview ownerReview)
        {
            int nextId = NextId();
            ownerReview.Id = nextId;
            _ownersreviews.Add(ownerReview);
            _serializer.ToCSV(FilePath, _ownersreviews);
            return ownerReview;
        }

        public void UpdateOwnerReview(OwnerReview updatedOwnerReview)
        {
            OwnerReview existingReview = _ownersreviews.FirstOrDefault(t => t.Id == updatedOwnerReview.Id);
            if (existingReview != null)
            {
                int index = _ownersreviews.IndexOf(existingReview);
                _ownersreviews[index] = updatedOwnerReview;
                _serializer.ToCSV(FilePath, _ownersreviews);
            }
        }

        public void DeleteOwnerReview(int ownerReviewId)
        {
            OwnerReview existingReview = _ownersreviews.FirstOrDefault(t => t.Id == ownerReviewId);
            if (existingReview != null)
            {
                _ownersreviews.Remove(existingReview);
                _serializer.ToCSV(FilePath, _ownersreviews);
            }
        }

        public List<OwnerReview> GetAllReviews()
        {
            return _ownersreviews;
        }

        public OwnerReview GetReviewById(int ownerReviewId)
        {
            return _ownersreviews.FirstOrDefault(t => t.Id == ownerReviewId);
        }


        private void SaveChanges()
        {
            _serializer.ToCSV(FilePath, _ownersreviews);
        }

        public int NextId()
        {
            if (_ownersreviews.Count < 1)
            {
                return 1;
            }
            return _ownersreviews.Max(t => t.Id) + 1;
        }


        public int GetReviewId(OwnerReview OwnerReview)
        {
            return OwnerReview.Id;
        }
        public List<OwnerReview> GetReviewsByOwnerId(int ownerId)
        {
            return _ownersreviews.Where(review => review.OwnerId == ownerId).ToList();
        }
        /*public double CalculateAverageRating(OwnerReviewRepository ownerReviewRepository)
        {
            List<OwnerReview> ownerReviews = ownerReviewRepository.GetReviewsByOwnerId(this.id);
            if (ownerReviews.Count == 0)
            {
                return 0; 
            }
            double totalRating = ownerReviews.Sum(review => (review.Cleanliness + review.Correctness) / 2.0);
            return totalRating / ownerReviews.Count;
        }*/
        public double CalculateAverageRating(int ownerId)
    {
        List<OwnerReview> ownerReviews = GetReviewsByOwnerId(ownerId);
        if (ownerReviews.Count == 0)
        {
            return 0;
        }
        double totalRating = ownerReviews.Sum(review => (review.Cleanliness + review.Correctness) / 2.0);
        return totalRating / ownerReviews.Count;
    }
}
}

