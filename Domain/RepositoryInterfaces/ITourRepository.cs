using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ITourRepository
    {
        List<Tour> GetAll();
        void Save(Tour tour);
        int NextId();
        void Delete(int id);
        void Update(Tour updatedTour);
        Tour GetById(int id);
    }
}
