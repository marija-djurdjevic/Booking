using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IGuestRepository
    {
        void Save(Guest guest);
        List<Guest> GetAll();
        void Update(Guest updatedguest);
        Guest GetByUserId(int Id);
    }
}
