using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IRenovationReccomendationRepository
    {
        void AddRenovationReccomendation(RenovationReccomendation _renovationReccomendation);
        List<RenovationReccomendation> GetAll();
        List<RenovationReccomendation> GetRenovationRequestDataById(int id);
        int NextId();
    }
}
