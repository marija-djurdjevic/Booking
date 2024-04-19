using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IUserRepository
    {
        List<User> GetAll();
        void Save(User user);
        int NextId();
        void Delete(int id);
        void Update(User updatedUser);
        User GetById(int id);
        User GetByUsername(string username);
    }
}
