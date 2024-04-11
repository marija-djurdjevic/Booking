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
        public KeyPoint JoinedKeyPoint { get; set; }
        public bool IsOnTour { get; set; }
        public bool IsUser { get; set; }

        public TourReservation()
        {
            JoinedKeyPoint = new KeyPoint();
            IsUser = false;
        }

        public TourReservation(int tourId, int userId,bool isUser)
        {
            TourId = tourId;
            UserId = userId;
            JoinedKeyPoint = new KeyPoint();
            IsUser = isUser;
        }

        public TourReservation(int tourId, int userId, string touristFirstName, string touristLastName, int touristAge)
        {
            TourId = tourId;
            UserId = userId;
            TouristFirstName = touristFirstName;
            TouristLastName = touristLastName;
            TouristAge = touristAge;
            JoinedKeyPoint = new KeyPoint();
            IsUser = false;
        }

        public TourReservation(int tourId, int userId, string touristFirstName, string touristLastName, int touristAge, KeyPoint joinedKeyPoint) : this(tourId, userId, touristFirstName, touristLastName, touristAge)
        {
            JoinedKeyPoint = joinedKeyPoint;
            IsUser = false;
        }

        public TourReservation(int tourId, int userId, string touristFirstName, string touristLastName, int touristAge, KeyPoint joinedKeyPoint, bool isOnTour) : this(tourId, userId, touristFirstName, touristLastName, touristAge, joinedKeyPoint)
        {
            this.IsOnTour = false;
            JoinedKeyPoint = new KeyPoint();
            IsUser = false;
        }

        public TourReservation(int tourId, Tourist loggedInTourist,bool isUser)
        {
            TourId = tourId;
            UserId = loggedInTourist.Id;
            TouristFirstName = loggedInTourist.FirstName;
            TouristLastName = loggedInTourist.LastName;
            TouristAge = loggedInTourist.Age;
            JoinedKeyPoint = new KeyPoint();
            IsUser = isUser;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { TourId.ToString(), UserId.ToString(), TouristFirstName, TouristLastName, TouristAge.ToString(), JoinedKeyPoint.Name, JoinedKeyPoint.OrdinalNumber.ToString(), IsOnTour.ToString(),IsUser.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            TourId = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            TouristFirstName = values[2];
            TouristLastName = values[3];
            TouristAge = Convert.ToInt32(values[4]);
            JoinedKeyPoint = new KeyPoint { Name = values[5], OrdinalNumber = Convert.ToInt32(values[6]) };
            IsOnTour = Convert.ToBoolean(values[7]);
            IsUser = Convert.ToBoolean(values[8]);
        }
    }
}
