using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class PropertyReservation : ISerializable
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public string PropertyName { get; set; }
        public int Guests { get; set; }
        public int Days { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int GuestId { get; set; }
        public int OwnerId { get; set; }
        public string GuestFirstName { get; set; }
        public string GuestLastName { get; set; }
        public bool Canceled { get; set; }

        public PropertyReservation() { }

        public PropertyReservation(int propertyReservationId)
        {
            Id = propertyReservationId;
        }

        public PropertyReservation(int id, int propertyId, int guests, int days, int guestId, string guestFirstName, string guestLastName, DateTime startDate, DateTime endDate, string propertyName, bool canceled, int ownerId)
        {
            Id = id;
            PropertyId = propertyId;
            Guests = guests;
            Days = days;
            GuestId = guestId;
            GuestFirstName = guestFirstName;
            GuestLastName = guestLastName;
            StartDate = startDate;
            EndDate = endDate;
            PropertyName = propertyName;
            Canceled = canceled;
            OwnerId = ownerId;
        }

        public PropertyReservation(int propertyId, int guests, int days, int guestId, string guestFirstName, string guestLastName, DateTime startDate, DateTime endDate, string propertyName, bool canceled, int ownerId)
        {
            PropertyId = propertyId;
            Guests = guests;
            Days = days;
            GuestId = guestId;
            GuestFirstName = guestFirstName;
            GuestLastName = guestLastName;
            StartDate = startDate;
            EndDate = endDate;
            PropertyName = propertyName;
            Canceled = canceled;
            OwnerId = ownerId;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), PropertyId.ToString(), Guests.ToString(), Days.ToString(), GuestId.ToString(), GuestFirstName, GuestLastName, StartDate.ToString("dd.MM.yyyy HH:mm:ss"), EndDate.ToString("dd.MM.yyyy HH:mm:ss"), PropertyName, Canceled.ToString(), OwnerId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            PropertyId = Convert.ToInt32(values[1]);
            Guests = Convert.ToInt32(values[2]);
            Days = Convert.ToInt32(values[3]);
            GuestId = Convert.ToInt32(values[4]);
            GuestFirstName = values[5];
            GuestLastName = values[6];
            StartDate = DateTime.ParseExact(values[7], "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            EndDate = DateTime.ParseExact(values[8], "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            PropertyName = values[9];
            Canceled = Convert.ToBoolean(values[10]);
            OwnerId = Convert.ToInt32(values[11]);
        }
    }
}
