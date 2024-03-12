using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Serializer;

namespace BookingApp.Model
{
    public class Location : ISerializable
    {

        public string Country { get; set; }
        public string City { get; set; }

        public Location() { }
        public Location(string country, string city)
        {

            Country = country;
            City = city;
        }




        public string[] ToCSV()
        {
            string[] csvValues = { Country, City };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {

            Country = values[1];
            City = values[2];
        }


    }
}
