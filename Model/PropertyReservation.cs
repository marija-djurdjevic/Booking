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

        public string GuestFirstName { get; set; }
        public string GuestLastName { get; set; }

        public PropertyReservation() { }

        public PropertyReservation(int propertyReservationId)
        {
            PropertyReservationId = propertyReservationId;
        }

        public PropertyReservation(int propertyReservationId, int propertyId, string guestFirstName, string guestLastName, int guests)
        {
            PropertyReservationId = propertyReservationId;
            PropertyId = propertyId;
            GuestFirstName = guestFirstName;
            GuestLastName = guestLastName;
            Guests = guests;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { PropertyReservationId.ToString(), PropertyId.ToString(), GuestFirstName, GuestLastName, Guests.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            PropertyReservationId = Convert.ToInt32(values[0]);
            PropertyId = Convert.ToInt32(values[1]);
            GuestFirstName = values[2];
            GuestLastName = values[3];
            Guests = Convert.ToInt32(values[4]);
        }
    }
}
