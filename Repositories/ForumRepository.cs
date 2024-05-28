using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repositories
{
    public class ForumRepository : IForumRepository
    {
        private const string FilePath = "../../../Resources/Data/forums.csv";

        private readonly Serializer<Forum> _serializer;

        private List<Forum> forums;

        public ForumRepository()
        {
            _serializer = new Serializer<Forum>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            forums = _serializer.FromCSV(FilePath);
        }

        public Forum AddForum(Forum forum)
        {
            int nextId = NextId();
            forum.Id = nextId;
            forums.Add(forum);
            _serializer.ToCSV(FilePath, forums);
            return forum;
        }

        public void Save(Forum forum)
        {
            forums = GetAll();
            forums.Add(forum);
            _serializer.ToCSV(FilePath, forums);
        }

        public List<Forum> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Update(Forum updatedforum)
        {
            Forum existingforum = forums.FirstOrDefault(t => t.Id == updatedforum.Id);
            if (existingforum != null)
            {
                int index = forums.IndexOf(existingforum);
                forums[index] = updatedforum;
                _serializer.ToCSV(FilePath, forums);
            }
        }

        public int NextId()
        {
            if (forums.Count < 1)
            {
                return 1;
            }
            return forums.Max(t => t.Id) + 1;
        }    
    }
}
