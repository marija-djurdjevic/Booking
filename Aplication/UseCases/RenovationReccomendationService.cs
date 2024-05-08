using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Aplication.UseCases
{
    public class RenovationReccomendationService
    {
        private readonly IRenovationReccomendationRepository renovationReccomendationRepository;
        private readonly IOwnerReviewRepository ownerReviewRepository;

        public RenovationReccomendationService(IRenovationReccomendationRepository renovationReccomendationRepository, IOwnerReviewRepository ownerReviewRepository)
        {
            this.renovationReccomendationRepository = renovationReccomendationRepository;
            this.ownerReviewRepository = ownerReviewRepository;
        }

        public void SaveRenovationReccomendation(RenovationReccomendation renovationReccomendation)
        {
            renovationReccomendationRepository.AddRenovationReccomendation(renovationReccomendation);
        }

        public int GetOwnerReviewId()
        {
            return ownerReviewRepository.NextId();
        }

    }
}
