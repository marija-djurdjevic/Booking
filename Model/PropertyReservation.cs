using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class PropertyReservation : ISerializable
    {
        public int PropertyReservationId { get; set; }
        public int PropertyId { get; set; }
        public int Guests { get; set; }
        public int Days {  get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string GuestFirstName { get; set; }
        public string GuestLastName { get; set; }

        public PropertyReservation() { }

        public PropertyReservation(int propertyReservationId)
        {
            PropertyReservationId = propertyReservationId;
        }

        public PropertyReservation(int propertyReservationId, int propertyId, int guests, int  days, string guestFirstName, string guestLastName, DateTime startDate, DateTime endDate)
        {
            PropertyReservationId = propertyReservationId;
            PropertyId = propertyId;
            Guests = guests;
            Days = days;
            GuestFirstName = guestFirstName;
            GuestLastName = guestLastName;
            StartDate = startDate;
            EndDate = endDate;
        }

        public PropertyReservation(int guests, int days, string guestFirstName, string guestLastName, DateTime startDate, DateTime endDate)
        {
            Guests = guests;
            Days = days;
            GuestFirstName = guestFirstName;
            GuestLastName = guestLastName;
            StartDate = startDate;
            EndDate = endDate;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { PropertyReservationId.ToString(), PropertyId.ToString(), Guests.ToString(), Days.ToString(), GuestFirstName, GuestLastName, StartDate.ToString(), EndDate.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            PropertyReservationId = Convert.ToInt32(values[0]);
            PropertyId = Convert.ToInt32(values[1]);
            Guests = Convert.ToInt32(values[2]);
            Days = Convert.ToInt32(values[3]);
            GuestFirstName = values[4];
            GuestLastName = values[5];
            StartDate = DateTime.Parse(values[6]);
            EndDate = DateTime.Parse(values[7]);
        }
    }
}
