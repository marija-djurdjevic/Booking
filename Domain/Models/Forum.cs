using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class Forum : ISerializable
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public Location Location { get; set; }
        public string Comment {  get; set; }
        public int GuestsComments {  get; set; }
        public int OwnersComments {  get; set; }
        public int Comments {  get; set; }
        public bool IsClosed { get; set; }

        public Forum()
        {

            Location = new Location();
            GuestsComments = 0;
            OwnersComments = 0;
            Comments = 0;
            IsClosed = false;

        }

        public Forum(int guestId, Location location, string comment, int guestcomments, bool isClosed, int ownersComments, int comments)
        {
            GuestId = guestId;
            Location = location;
            Comment = comment;
            GuestsComments = guestcomments;
            IsClosed = isClosed;
            OwnersComments = ownersComments;
            Comments = comments;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuestId.ToString(), Location.Country, Location.City, Comment, GuestsComments.ToString(), IsClosed.ToString(), OwnersComments.ToString(), Comments.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            Location.Country = values[2];
            Location.City = values[3];
            Comment = values[4];
            GuestsComments = Convert.ToInt32(values[5]);
            IsClosed = Convert.ToBoolean(values[6]);
            OwnersComments = Convert.ToInt32(values[7]);
            Comments = Convert.ToInt32(values[8]);
        }
    }
}
