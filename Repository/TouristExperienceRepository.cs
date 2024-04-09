using System;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Model;
using BookingApp.Serializer;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class TouristExperienceRepository
    {
        private const string FilePath = "../../../Resources/Data/touristExperiences.csv";

        private readonly Serializer<TouristExperience> _serializer;

        private List<TouristExperience> touristExperiences;

        public TouristExperienceRepository()
        {
            _serializer = new Serializer<TouristExperience>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            touristExperiences = GetAll();
        }

        public TouristExperience Save(TouristExperience touristExperience)
        {
            touristExperiences = GetAll();
            int nextId = NextId();
            touristExperience.Id = nextId;
            touristExperiences.Add(touristExperience);
            _serializer.ToCSV(FilePath, touristExperiences);
            return touristExperience;
        }

        public void Update(TouristExperience updatedTouristExperience)
        {
            touristExperiences = GetAll();
            TouristExperience existingTouristExperience = touristExperiences.FirstOrDefault(t => t.Id == updatedTouristExperience.Id);
            if (existingTouristExperience != null)
            {
                int index = touristExperiences.IndexOf(existingTouristExperience);
                touristExperiences[index] = updatedTouristExperience;
                _serializer.ToCSV(FilePath, touristExperiences);
            }
        }

        public void Delete(int touristExperienceId)
        {
            touristExperiences = GetAll();
            TouristExperience existingTouristExperience = touristExperiences.FirstOrDefault(t => t.Id == touristExperienceId);
            if (existingTouristExperience != null)
            {
                touristExperiences.Remove(existingTouristExperience);
                _serializer.ToCSV(FilePath, touristExperiences);
            }
        }

        public List<TouristExperience> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        private int NextId()
        {
            return touristExperiences.Count > 0 ? touristExperiences.Max(t => t.Id) + 1 : 1;
        }


        public List<int> GetTouristIdsByTourId(int tourId)
        {
            touristExperiences = GetAll();

           
            var touristIds = touristExperiences.Where(te => te.TourId == tourId).Select(te => te.TouristId).ToList();

            return touristIds;
        }

        public int GetNumberOfTouristsForTour(int tourId)
        {
            touristExperiences = GetAll();
            return touristExperiences.Count(t => t.TourId == tourId);
        }


        public bool IsTourRatedByUser(int tourId, int userId)
        {
            touristExperiences = GetAll();
            return touristExperiences.Any(t => t.TourId == tourId && t.TouristId == userId);
        }
    }
}
