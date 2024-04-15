using BookingApp.Domain.Models;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Aplication.UseCases
{
    public class TouristExperienceService
    {
        private readonly TouristExperienceRepository touristExperienceRepository;

        public TouristExperienceService()
        {
            touristExperienceRepository = new TouristExperienceRepository();
        }

        public int GetNumberOfTouristsForTour(int tourId)
        {
            return touristExperienceRepository.GetNumberOfTouristsForTour(tourId);
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
    }
}
