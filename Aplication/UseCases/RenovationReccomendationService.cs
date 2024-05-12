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
            // Pronalazimo sve rezervacije za datu nekretninu i godinu
            var propertyReservationsForYear = propertyReservationRepository
                .GetAll()
                .Where(r => r.PropertyName == propertyName && r.StartDate.Year == year)
                .Select(r => r.Id)
                .ToList();

            // Pronalazimo sve preporuke za renoviranje koje su povezane sa rezervacijama za datu nekretninu i godinu
            var recommendationsForProperty = renovationReccomendationRepository
                .GetAll()
                .Where(r => propertyReservationsForYear.Contains(r.OwnerReviewId))
                .Count();

            return recommendationsForProperty;
        }
        public int GetRenovationRecommendationsForMonth(string propertyName, int year, int month)
        {
            // Pronalazimo sve rezervacije za datu nekretninu, godinu i mjesec
            var propertyReservationsForYearAndMonth = propertyReservationRepository
                .GetAll()
                .Where(r => r.PropertyName == propertyName && r.StartDate.Year == year && r.StartDate.Month == month)
                .Select(r => r.Id)
                .ToList();

            // Pronalazimo sve preporuke za renoviranje koje su povezane sa rezervacijama za datu nekretninu, godinu i mjesec
            var recommendationsForPropertyAndMonth = renovationReccomendationRepository
                .GetAll()
                .Count(r => propertyReservationsForYearAndMonth.Contains(r.OwnerReviewId));

            return recommendationsForPropertyAndMonth;
        }
    }
}
