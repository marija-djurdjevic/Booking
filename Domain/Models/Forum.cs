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
        public int Comments {  get; set; }

        public Forum()
        {
            Location = new Location();
            Comments = 0;
        }

        public Forum(int guestId, Location location, string comment, int comments)
        {
            GuestId = guestId;
            Location = location;
            Comment = comment;
            Comments = comments;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuestId.ToString(), Location.Country, Location.City, Comment, Comments.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            Location.Country = values[2];
            Location.City = values[3];
            Comment = values[4];
            Comments = Convert.ToInt32(values[5]);
        }
    }
}
