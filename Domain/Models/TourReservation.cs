using BookingApp.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookingApp.Serializer;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class TourReservation : ISerializable
    {
        public int Id { get; set; }
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

        public TourReservation(int tourId, int userId, bool isUser)
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

        public TourReservation(int id, int tourId, int userId, string touristFirstName, string touristLastName, int touristAge, KeyPoint joinedKeyPoint, bool isOnTour, bool isUser)
        {
            Id = id;
            TourId = tourId;
            UserId = userId;
            TouristFirstName = touristFirstName;
            TouristLastName = touristLastName;
            TouristAge = touristAge;
            JoinedKeyPoint = joinedKeyPoint;
            IsOnTour = isOnTour;
            IsUser = isUser;
        }

        public TourReservation(int tourId, int userId, string touristFirstName, string touristLastName, int touristAge, KeyPoint joinedKeyPoint) : this(tourId, userId, touristFirstName, touristLastName, touristAge)
        {
            JoinedKeyPoint = joinedKeyPoint;
            IsUser = false;
        }

        public TourReservation(int tourId, int userId, string touristFirstName, string touristLastName, int touristAge, KeyPoint joinedKeyPoint, bool isOnTour) : this(tourId, userId, touristFirstName, touristLastName, touristAge, joinedKeyPoint)
        {
            IsOnTour = false;
            JoinedKeyPoint = new KeyPoint();
            IsUser = false;
        }

        public TourReservation(int tourId, Tourist loggedInTourist, bool isUser)
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
            string[] csvValues = {Id.ToString(), TourId.ToString(), UserId.ToString(), TouristFirstName, TouristLastName, TouristAge.ToString(), JoinedKeyPoint.Name, JoinedKeyPoint.OrdinalNumber.ToString(), IsOnTour.ToString(), IsUser.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourId = Convert.ToInt32(values[1]);
            UserId = Convert.ToInt32(values[2]);
            TouristFirstName = values[3];
            TouristLastName = values[4];
            TouristAge = Convert.ToInt32(values[5]);
            JoinedKeyPoint = new KeyPoint { Name = values[6], OrdinalNumber = Convert.ToInt32(values[7]) };
            IsOnTour = Convert.ToBoolean(values[8]);
            IsUser = Convert.ToBoolean(values[9]);
        }
    }
}
