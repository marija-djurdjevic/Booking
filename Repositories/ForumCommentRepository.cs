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
    public class ForumCommentRepository : IForumCommentRepository
    {
        private const string FilePath = "../../../Resources/Data/forumcomments.csv";

        private readonly Serializer<ForumComment> _serializer;

        private List<ForumComment> forumcomments;

        public ForumCommentRepository()
        {
            _serializer = new Serializer<ForumComment>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            forumcomments = _serializer.FromCSV(FilePath);
        }

        public ForumComment AddForumComment(ForumComment forumComment)
        {
            int nextId = NextId();
            forumComment.Id = nextId;
            forumcomments.Add(forumComment);
            _serializer.ToCSV(FilePath, forumcomments);
            return forumComment;
        }

        public void Save(ForumComment forumComment)
        {
            forumcomments = GetAll();
            forumcomments.Add(forumComment);
            _serializer.ToCSV(FilePath, forumcomments);
        }

        public List<ForumComment> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Update(ForumComment updatedforumComment)
        {
            ForumComment existingforum = forumcomments.FirstOrDefault(t => t.Id == updatedforumComment.Id);
            if (existingforum != null)
            {
                int index = forumcomments.IndexOf(existingforum);
                forumcomments[index] = updatedforumComment;
                _serializer.ToCSV(FilePath, forumcomments);
            }
        }

        public int NextId()
        {
            if (forumcomments.Count < 1)
            {
                return 1;
            }
            return forumcomments.Max(t => t.Id) + 1;
        }
    }
}
