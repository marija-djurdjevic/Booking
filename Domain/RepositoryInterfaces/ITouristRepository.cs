using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ITouristRepository
    {
        List<Tourist> GetAll();
        void Save(Tourist tourist);
        int NextId();
        void Delete(int id);
        void Update(Tourist updatedTourist);
        Tourist GetById(int id);
    }
}
