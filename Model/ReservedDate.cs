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
        public int PropertyId { get; set; }

        public DateTime DateValue {  get; set; }
        public User User { get; set; }

        public ReservedDate() { }

        public ReservedDate(DateTime dateValue)
        {
            DateValue = dateValue;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { PropertyId.ToString(), DateValue.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            PropertyId = Convert.ToInt32(values[0]);
            DateValue = Convert.ToDateTime(values[1]);
        }
    }
}
