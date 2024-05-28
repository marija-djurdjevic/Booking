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
        public int GuestId { get; set; }
        public int ForumId { get; set; }
        public int AuthorId {  get; set; }
        public string Comment { get; set; }

        public ForumComment()
        {
            
        }

        public ForumComment(int guestId, int forumId, string comment, int authorId)
        {
            GuestId = guestId;
            ForumId = forumId;
            Comment = comment;
            AuthorId = authorId;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuestId.ToString(), ForumId.ToString(), Comment, AuthorId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            ForumId = Convert.ToInt32(values[2]);
            Comment = values[3];
            AuthorId = Convert.ToInt32(values[4]);
        }
    }
}
