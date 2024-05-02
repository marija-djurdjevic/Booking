using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ITourRequestRepository
    {
        List<TourRequest> GetAll();
        void Save(TourRequest tourRequest);
        int NextId();
        void Delete(int id);
        void Update(TourRequest updatedTourRequest);
        TourRequest GetById(int id);
        List<TourRequest> GetByComplexId(int complexId);
    }
}
