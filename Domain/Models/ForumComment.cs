using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class ForumComment : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ForumId { get; set; }
        public int AuthorId {  get; set; }
        public string Comment { get; set; }
        public bool GuestVisited { get; set; }
        public int ReportsCount { get; set; }

        public ForumComment()
        {
            ReportsCount = 0;
            
        }

        public ForumComment(int guestId, int forumId, string comment, int authorId, bool guestVisited)
        {
            UserId = guestId;
            ForumId = forumId;
            Comment = comment;
            AuthorId = authorId;
            GuestVisited = guestVisited;
        }
        public ForumComment(int guestId, int forumId, string comment, int authorId, bool guestVisited, int reportsCount)
        {
            UserId = guestId;
            ForumId = forumId;
            Comment = comment;
            AuthorId = authorId;
            GuestVisited = guestVisited;
            ReportsCount = reportsCount;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), UserId.ToString(), ForumId.ToString(), Comment, AuthorId.ToString(), GuestVisited.ToString(), ReportsCount.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            ForumId = Convert.ToInt32(values[2]);
            Comment = values[3];
            AuthorId = Convert.ToInt32(values[4]);
            GuestVisited = Convert.ToBoolean(values[5]);
            ReportsCount = Convert.ToInt32(values[6]);
        }
    }
}
