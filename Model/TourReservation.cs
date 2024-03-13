using BookingApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookingApp.Serializer;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class TourReservation : ISerializable
    {

        public int TourId { get; set; }
        public int UserId { get; set; }
        public string TouristFirstName { get; set; }
        public string TouristLastName { get; set; }
        public int TouristAge { get; set; }
        public KeyPoints JoinedKeyPoint { get; set; }

        public TourReservation() { }

        public TourReservation(int tourId, int userId)
        {
            TourId = tourId;
            UserId = userId;
            JoinedKeyPoint = new KeyPoints();
        }

        public TourReservation(int tourId, int userId, string touristFirstName, string touristLastName, int touristAge)
        {
            TourId = tourId;
            UserId = userId;
            TouristFirstName = touristFirstName;
            TouristLastName = touristLastName;
            TouristAge = touristAge;
        }

        public TourReservation(int tourId, int userId, string touristFirstName, string touristLastName, int touristAge, KeyPoints joinedKeyPoint) : this(tourId, userId, touristFirstName, touristLastName, touristAge)
        {
            JoinedKeyPoint = joinedKeyPoint;
        }

        public TourReservation(int tourId, Tourist loggedInTourist)
        {
            TourId = tourId;
            UserId = loggedInTourist.Id;
            TouristFirstName = loggedInTourist.FirstName;
            TouristLastName = loggedInTourist.LastName;
            TouristAge = loggedInTourist.Age;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { TourId.ToString(), UserId.ToString(), TouristFirstName, TouristLastName, TouristAge.ToString(), JoinedKeyPoint.KeyName };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            TourId = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            TouristFirstName = values[2];
            TouristLastName = values[3];
            TouristAge = Convert.ToInt32(values[4]);
            JoinedKeyPoint = new KeyPoints { KeyName = values[5] };
        }
    }
}
