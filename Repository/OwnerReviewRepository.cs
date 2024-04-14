using BookingApp.Dto;
using BookingApp.Model;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BookingApp.Repository
{
    public class OwnerReviewRepository
    {
        private const string FilePath = "../../../Resources/Data/ownersreviews.csv";

        private readonly Serializer<OwnerReview> _serializer;

        private List<OwnerReview> _ownersreviews;

        public int id;
        private readonly ReviewRepository _reviewRepository;
        private readonly PropertyReservationRepository _reservationRepository;

        public OwnerReviewRepository()
        {
            _serializer = new Serializer<OwnerReview>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            _ownersreviews = _serializer.FromCSV(FilePath);
            _reviewRepository = new ReviewRepository();
            _reservationRepository = new PropertyReservationRepository();
        }

        public OwnerReview AddOwnerReview(OwnerReview ownerReview)
        {
            int nextId = NextId();
            ownerReview.Id = nextId;
            _ownersreviews.Add(ownerReview);
            _serializer.ToCSV(FilePath, _ownersreviews);
            return ownerReview;
        }

        public List<OwnerReview> GetAllReviews()
        {
            return _ownersreviews;
        }

        public OwnerReview GetReviewById(int ownerReviewId)
        {
            return _ownersreviews.FirstOrDefault(t => t.Id == ownerReviewId);
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
        public ObservableCollection<KeyValuePair<OwnerReview, PropertyReservation>> LoadOwnerReviewsWithReservations()
        {
            var ownerReviews = GetAllReviews();
            var guestReviews = _reviewRepository.GetAllReviews();
            var guestReviewReservationIds = guestReviews.Select(review => review.ReservationId).ToList();

            var ownerReviewsWithReservations = new ObservableCollection<KeyValuePair<OwnerReview, PropertyReservation>>();

            foreach (var ownerReview in ownerReviews)
            {
                if (guestReviewReservationIds.Contains(ownerReview.ReservationId))
                {
                    var reservation = _reservationRepository.GetReservationById(ownerReview.ReservationId);

                    if (reservation != null)
                    {
                        var reviewWithReservation = new KeyValuePair<OwnerReview, PropertyReservation>(ownerReview, reservation);
                        ownerReviewsWithReservations.Add(reviewWithReservation);
                    }
                }
            }

            return ownerReviewsWithReservations;
        }
    }
}

