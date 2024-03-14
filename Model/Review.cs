using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class Review : ISerializable
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public int GuestId { get; set; }
        public int Cleanliness {  get; set; }
        public int Rules { get; set; }
        public string Comment { get; set; }
        public Review() { }
        public Review(int id, int reservationId, int guestId, int cleanliness, int rules, string comment) {
            Id = id;
            ReservationId = reservationId;
            GuestId = guestId;
            Cleanliness = cleanliness;
            Rules = rules;
            Comment = comment;
        }
        public Review(int reservationId, int guestId, int cleanliness, int rules, string comment)
        {
            ReservationId = reservationId;
            GuestId = guestId;
            Cleanliness = cleanliness;
            Rules = rules;
            Comment = comment;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), ReservationId.ToString(), GuestId.ToString(), Cleanliness.ToString(), Rules.ToString(), Comment };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            ReservationId = Convert.ToInt32(values[1]);
            GuestId = Convert.ToInt32(values[2]);
            Cleanliness = Convert.ToInt32(values[3]);
            Rules = Convert.ToInt32(values[4]);
            Comment = values[5];
        }
    }
}
