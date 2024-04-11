using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace BookingApp.Model
{
    public class OwnerReview : ISerializable
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int ReservationId { get; set; }
        public int Cleanliness { get; set; }
        public int Correctness { get; set; }
        public string Comment { get; set; }
        public List<string> ImagesPaths { get; set; }

        public OwnerReview() {

            ImagesPaths = new List<string>();
        }
        public OwnerReview(int id, int ownerId, int reservationId,  int cleanliness, int correctnes, string comment, List<string> imagesPaths)
        {
            Id = id;
            OwnerId = ownerId;
            ReservationId = reservationId;
            Cleanliness = cleanliness;
            Correctness = correctnes;
            Comment = comment;
            ImagesPaths = imagesPaths;
        }
        public OwnerReview(int ownerId, int reservationId, int cleanliness, int correctnes, string comment, List<string> imagesPaths)
        {
            OwnerId = ownerId;
            ReservationId = reservationId;
            Cleanliness = cleanliness;
            Correctness = correctnes;
            Comment = comment;
            ImagesPaths = imagesPaths;
        }
        public string[] ToCSV()
        {
            if (ImagesPaths == null)
            {
                string[] csvValues = { Id.ToString(), OwnerId.ToString(), ReservationId.ToString(), Cleanliness.ToString(), Correctness.ToString(), Comment };
                return csvValues;
            }
            else
            {
                string imagesPathsStr = string.Join("|", ImagesPaths);
                string[] csvValues = { Id.ToString(), OwnerId.ToString(), ReservationId.ToString(), Cleanliness.ToString(), Correctness.ToString(), Comment, imagesPathsStr };
                return csvValues;
            }
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            OwnerId = Convert.ToInt32(values[1]);
            ReservationId = Convert.ToInt32((string)values[2]);
            Cleanliness = Convert.ToInt32(values[3]);
            Correctness = Convert.ToInt32(values[4]);
            Comment = values[5];
            for (int i = 8; i < values.Length; i++)
            {
                ImagesPaths.Add(values[i]);
            }
        }
    }
}
