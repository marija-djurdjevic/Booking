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

        public ReservationData() { }

        public ReservationData(int tourId)
        {
            TourId = tourId;
        }

        public ReservationData(int tourId, string touristFirstName, string touristLastName, int touristAge)
        {
            TourId = tourId;
            TouristFirstName = touristFirstName;
            TouristLastName = touristLastName;
            TouristAge = touristAge;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { TourId.ToString(), TouristFirstName, TouristLastName, TouristAge.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            TourId = Convert.ToInt32(values[0]);
            TouristFirstName = values[1];
            TouristLastName = values[2];
            TouristAge = Convert.ToInt32(values[3]);
        }
    }
}
