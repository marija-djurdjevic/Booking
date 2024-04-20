using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ITouristExperienceRepository
    {
        List<TouristExperience> GetAll();
        void Save(TouristExperience touristExperience);
        int NextId();
        void Delete(int id);
        void Update(TouristExperience updatedTouristExperience);
        TouristExperience GetById(int id);
    }
}
