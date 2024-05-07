using BookingApp.Domain.Models;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Aplication.UseCases
{
    public class OwnerService
    {
        public IOwnerReviewRepository ownerReviewRepository;
        public IOwnerRepository ownerRepository;
        public OwnerService(IOwnerReviewRepository ownerReviewRepository, IOwnerRepository ownerRepository)
        {
            this.ownerReviewRepository = ownerReviewRepository;
            this.ownerRepository = ownerRepository;
            //ownerReviewRepository = new OwnerReviewRepository();
            //ownerRepository = new OwnerRepository();
        }
        public void UpdateOwnerPropertiesBasedOnReviews()
        {
            List<Owner> allOwners = ownerRepository.GetAll();
            foreach (Owner owner in allOwners)
            {
                double averageRating = ownerReviewRepository.CalculateAverageRating(owner.Id);
                owner.OwnerAverage = averageRating;

                List<OwnerReview> ownerReviews = ownerReviewRepository.GetReviewsByOwnerId(owner.Id);
                if (ownerReviews.Count >= 50)
                {
                    if (averageRating > 4.5)
                    {
                        owner.IsSuperOwner = true;
                    }
                    else
                    {
                        owner.IsSuperOwner = false;
                    }
                }
                else
                {
                    owner.IsSuperOwner = false;
                }
                ownerRepository.UpdateOwner(owner);
            }
        }
    }
}
