using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class ReservedDate : ISerializable
    {

        public int ReservationId;
        public int PropertyId;
        public DateTime Value;

        public ReservedDate()
        {
        }

        public ReservedDate(int propertyId, DateTime value, int reservationId)
        {
            PropertyId = propertyId;
            Value = value;
            ReservationId = reservationId;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { PropertyId.ToString(), Value.ToString(), ReservationId.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            PropertyId = Convert.ToInt32(values[0]);
            Value = Convert.ToDateTime(values[1]);
            ReservationId = Convert.ToInt32(values[2]);
        }
    }
}
