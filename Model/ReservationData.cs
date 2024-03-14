using BookingApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookingApp.Serializer;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class ReservationData : ISerializable
    {

        public int TourId { get; set; }
        public string TouristFirstName { get; set; }
        public string TouristLastName { get; set; }
        public int TouristAge { get; set; }

        public KeyPoints JoinedKeyPoint { get; set; }

        public bool IsOnTour { get; set; }

        public ReservationData() { }

        public ReservationData(int tourId)
        {
            TourId = tourId;
            JoinedKeyPoint = new KeyPoints();
        }

        public ReservationData(int tourId, string touristFirstName, string touristLastName, int touristAge)
        {
            TourId = tourId;
            TouristFirstName = touristFirstName;
            TouristLastName = touristLastName;
            TouristAge = touristAge;
            
        }

        public ReservationData(int tourId, string touristFirstName, string touristLastName, int touristAge, KeyPoints joinedKeyPoint) : this(tourId, touristFirstName, touristLastName, touristAge)
        {
            JoinedKeyPoint = joinedKeyPoint;
        }
        public ReservationData(int tourId, string touristFirstName, string touristLastName, int touristAge, KeyPoints joinedKeyPoint, bool isOnTour) : this(tourId, touristFirstName, touristLastName, touristAge, joinedKeyPoint)
        {
            IsOnTour = false;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { TourId.ToString(), TouristFirstName, TouristLastName, TouristAge.ToString(),JoinedKeyPoint.KeyName,JoinedKeyPoint.OrdinalNumber.ToString(),IsOnTour.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            TourId = Convert.ToInt32(values[0]);
            TouristFirstName = values[1];
            TouristLastName = values[2];
            TouristAge = Convert.ToInt32(values[3]);
            JoinedKeyPoint = new KeyPoints { KeyName = values[4], OrdinalNumber = Convert.ToInt32(values[5]) };
            IsOnTour = Convert.ToBoolean(values[6]);
        }


        
    }
}
