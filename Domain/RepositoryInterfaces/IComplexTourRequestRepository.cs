using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IComplexTourRequestRepository
    {
        List<ComplexTourRequest> GetAll();
        int Save(ComplexTourRequest tourRequest);
        int NextId();
        void Delete(int id);
        void Update(ComplexTourRequest updatedTourRequest);
        ComplexTourRequest GetById(int id);
    }
}
