using BookingApp.Domain.Models;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IForumRepository
    {
        public Forum AddForum(Forum forum);
        public void Save(Forum forum);
        public List<Forum> GetAll();
        public void Update(Forum updatedforum);
        public int NextId();
    }    
}
