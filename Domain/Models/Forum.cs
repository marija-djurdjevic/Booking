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
        public string Comment;

        public Forum()
        {
            
        }

        public Forum(int guestId, Location location, string comment)
        {
            GuestId = guestId;
            Location = location;
            Comment = comment;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuestId.ToString(), Location.Country, Location.City, Comment };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            Location.Country = values[2];
            Location.City = values[3];
            Comment = values[4];
        }
    }
}
