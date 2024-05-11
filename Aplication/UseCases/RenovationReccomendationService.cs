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
        private readonly IPropertyReservationRepository propertyReservationRepository;

        public RenovationReccomendationService(IRenovationReccomendationRepository renovationReccomendationRepository, IOwnerReviewRepository ownerReviewRepository, IPropertyReservationRepository propertyReservationRepository)
        {
            this.renovationReccomendationRepository = renovationReccomendationRepository;
            this.ownerReviewRepository = ownerReviewRepository;
            this.propertyReservationRepository = propertyReservationRepository;
        }

        public void SaveRenovationReccomendation(RenovationReccomendation renovationReccomendation)
        {
            renovationReccomendationRepository.AddRenovationReccomendation(renovationReccomendation);
        }

        public int GetOwnerReviewId()
        {
            return ownerReviewRepository.NextId();
        }
        public int GetRenovationRecommendationsCountForProperty(string propertyName, int year)
        {
            var propertyReservationsForYear = propertyReservationRepository.GetAll()
                .Where(r => r.PropertyName == propertyName && r.StartDate.Year == year);

            return renovationReccomendationRepository.GetAll()
                .Count(r => propertyReservationsForYear.Any(pr => pr.PropertyName == propertyName && pr.Id == r.OwnerReviewId));
        }
    }
}
