using BookingApp.Dto;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class OwnerReviewService
    {
        public OwnerReviewRepository OwnerReviewRepository;

        public OwnerReviewService()
        {
            OwnerReviewRepository = new OwnerReviewRepository();
        }

        public void SaveReview(OwnerReviewDto ownerReview)
        {
           OwnerReviewRepository.AddOwnerReview(ownerReview.ToOwnerReview());
        }

    }
}
