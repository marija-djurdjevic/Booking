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

        //public int Id;
        public int PropertyId;
        public DateTime Value;

        public ReservedDate()
        {
        }

        public ReservedDate(int propertyId, DateTime value)
        {
            PropertyId = propertyId;
            Value = value;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { PropertyId.ToString(), Value.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            PropertyId = Convert.ToInt32(values[0]);
            Value = Convert.ToDateTime(values[1]);
        }
    }
}
