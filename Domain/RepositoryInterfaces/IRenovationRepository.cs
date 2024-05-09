using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IRenovationRepository
    {
        public Renovation AddRenovation(Renovation renovation);
        public void UpdateRenovation(Renovation updatedRenovation);
        public void DeleteRenovation(int renovationId);
        public Renovation Save(Renovation newRenovation);
        public List<Renovation> GetAllRenovations();
        public Renovation GetRenovationById(int renovationId);
        public void SaveChanges();
        public int NextId();
        public int GetRenovationId(Renovation renovation);

    }
}
