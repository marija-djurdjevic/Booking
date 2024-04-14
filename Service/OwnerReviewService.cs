using BookingApp.Dto;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class OwnerReviewService
    {
        public OwnerReviewRepository OwnerReviewRepository;
        public ReviewRepository ReviewRepository;
        public PropertyReservationRepository ReservationRepository;

        public OwnerReviewService()
        {
            OwnerReviewRepository = new OwnerReviewRepository();
        }

        public void SaveReview(OwnerReviewDto ownerReview)
        {
           OwnerReviewRepository.AddOwnerReview(ownerReview.ToOwnerReview());
        }
        public ObservableCollection<KeyValuePair<OwnerReview, PropertyReservation>> LoadOwnerReviewsWithReservations()
        {
            return OwnerReviewRepository.LoadOwnerReviewsWithReservations();
        }
    }
}
