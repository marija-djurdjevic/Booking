using BookingApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Serializer;


namespace BookingApp.Domain.Models
{
    public class RenovationReccomendation : ISerializable
    {
        public int Id { get; set; }
        public int GuestId {  get; set; }
        public int OwnerReviewId {  get; set; }
        public int UrgencyLevel {  get; set; }
        public string Comment {  get; set; }

        public RenovationReccomendation()
        {
        }

        public RenovationReccomendation(int id, int guestId, int ownerReviewId, int urgencyLevel, string comment)
        {
            Id = id;
            GuestId = guestId;
            OwnerReviewId = ownerReviewId;
            UrgencyLevel = urgencyLevel;
            Comment = comment;

        }
        public RenovationReccomendation(int guestId, int ownerReviewId, int urgencyLevel, string comment)
        {
            GuestId = guestId;
            OwnerReviewId = ownerReviewId;
            UrgencyLevel = urgencyLevel;
            Comment = comment;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuestId.ToString(), OwnerReviewId.ToString(), UrgencyLevel.ToString(), Comment };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            OwnerReviewId = Convert.ToInt32(values[2]);
            UrgencyLevel = Convert.ToInt32(values[3]);
            Comment = values[4];
        }
    }

}
