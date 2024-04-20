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
    public class TouristExperienceService
    {
        private readonly ITouristExperienceRepository touristExperienceRepository;

        public TouristExperienceService(ITouristExperienceRepository touristExperienceRepository)
        {
            this.touristExperienceRepository = touristExperienceRepository;
        }

        public void Save(TouristExperience touristExperience)
        {
            touristExperienceRepository.Save(touristExperience);
        }

        public int GetNumberOfTouristsForTour(int tourId)
        {
            var touristExperiences = touristExperienceRepository.GetAll();
            return touristExperiences.Count(t => t.TourId == tourId);
        }


        public List<int> GetTouristIdsByTourId(int tourId)
        {
            var touristExperiences = touristExperienceRepository.GetAll();
            return touristExperiences.Where(te => te.TourId == tourId).Select(te => te.TouristId).ToList();
        }

        public List<TouristExperience> GetTouristExperiencesForTour(int tourId)
        {
            var touristExperiences = touristExperienceRepository.GetAll();
            return touristExperiences.Where(te => te.TourId == tourId).ToList();
        }

        public void Update(TouristExperience updatedTouristExperience)
        {
            touristExperienceRepository.Update(updatedTouristExperience);

        }

        public bool IsTourRatedByUser(int tourId, int userId)
        {
            var touristExperiences = touristExperienceRepository.GetAll();
            return touristExperiences.Any(t => t.TourId == tourId && t.TouristId == userId);
        }
    }
}
