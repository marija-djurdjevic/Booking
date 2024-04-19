using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IOwnerRepository
    {
        void Save(Owner owner);
        List<Owner> GetAll();
        Owner GetByUserId(int Id);
        void UpdateOwner(Owner updatedOwner);
        void Save();

    }
}
