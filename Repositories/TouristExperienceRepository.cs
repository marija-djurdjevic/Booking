using System;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Serializer;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Repositories
{
    public class TouristExperienceRepository : ITouristExperienceRepository
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

        public List<TouristExperience> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public TouristExperience GetById(int touristExperiencesId)
        {
            touristExperiences = GetAll();
            return touristExperiences.FirstOrDefault(t => t.Id == touristExperiencesId);
        }

        public void Save(TouristExperience touristExperience)
        {
            touristExperiences = GetAll();
            int nextId = NextId();
            touristExperience.Id = nextId;
            touristExperiences.Add(touristExperience);
            _serializer.ToCSV(FilePath, touristExperiences);
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

        public int NextId()
        {
            return touristExperiences.Count > 0 ? touristExperiences.Max(t => t.Id) + 1 : 1;
        }
    }
}
