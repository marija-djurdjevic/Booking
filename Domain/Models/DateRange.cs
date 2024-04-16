using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class DateRange : ISerializable
    {

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int PropertyId;

        public DateRange()
        {
        }

        public DateRange(int propertyId, DateTime start, DateTime end)
        {
            PropertyId = propertyId;
            Start = start;
            End = end;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { PropertyId.ToString(), Start.ToString(), End.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            PropertyId = Convert.ToInt32(values[0]);
            Start = Convert.ToDateTime(values[1]);
            End = Convert.ToDateTime(values[2]);
        }
    }
}
