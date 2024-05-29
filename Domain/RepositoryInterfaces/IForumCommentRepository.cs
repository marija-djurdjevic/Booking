using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IForumCommentRepository
    {
        public ForumComment AddForumComment(ForumComment forumComment);
        public void Save(ForumComment forumComment);
        public List<ForumComment> GetAll();
        public void Update(ForumComment updatedforumComment);
        public int NextId();
    }
}
