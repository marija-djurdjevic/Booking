using BookingApp.Domain.Models;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private const string FilePath = "../../../Resources/Data/users.csv";

        private readonly Serializer<User> _serializer;

        private List<User> _users;

        public UserRepository()
        {
            _serializer = new Serializer<User>();
            _users = _serializer.FromCSV(FilePath);
        }

        public List<User> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public User GetById(int userId)
        {
            _users = GetAll();
            return _users.FirstOrDefault(t => t.Id == userId);
        }

        public void Save(User user)
        {
            _users = GetAll();
            user.Id = NextId();
            _users.Add(user);
            _serializer.ToCSV(FilePath, _users);
        }

        public void Update(User updatedUser)
        {
            _users = GetAll();
            User existingUser = _users.FirstOrDefault(t => t.Id == updatedUser.Id);
            if (existingUser != null)
            {
                int index = _users.IndexOf(existingUser);
                _users[index] = updatedUser;
                _serializer.ToCSV(FilePath, _users);
            }
        }

        public void Delete(int userId)
        {
            _users = GetAll();
            User existingUser = _users.FirstOrDefault(t => t.Id == userId);
            if (existingUser != null)
            {
                _users.Remove(existingUser);
                _serializer.ToCSV(FilePath, _users);
            }
        }

        public int NextId()
        {
            _users = _serializer.FromCSV(FilePath);
            if (_users.Count < 1)
            {
                return 1;
            }
            return _users.Max(t => t.Id) + 1;
        }

        public User GetByUsername(string username)
        {
            _users = _serializer.FromCSV(FilePath);
            return _users.FirstOrDefault(u => u.Username == username);
        }
    }
}
