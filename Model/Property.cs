using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.Model
{
    enum PropertyType { Apartment, House, Cabin };
     class Property : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        PropertyType Type { get; set; }
        public int MaxGuests { get; set; }
        public int MinReservationDays { get; set; }
        public int CancellationDeadline { get; set; }
        public List<string> ImagesPaths { get; set; }

        public Property()
        {
            Location = new Location();
            ImagesPaths = new List<string>();

        }
        public Property(int id, string name, Location location, PropertyType type, int maxGuests, int minReservationDays, int cancellationDeadline, List<string> imagesPaths)
        {
            Id = id;
            Name = name;
            Type = type;
            MaxGuests = maxGuests;
            MinReservationDays = minReservationDays;
            CancellationDeadline = cancellationDeadline;
            ImagesPaths = imagesPaths;
            Location = location;


        }
        public Property(string name, Location location, PropertyType type, int maxGuests, int minReservationDays, int cancellationDeadline, List<string> imagesPaths)
        {
            Name = name;
            Type = type;
            MaxGuests = maxGuests;
            MinReservationDays = minReservationDays;
            CancellationDeadline = cancellationDeadline;
            ImagesPaths = imagesPaths;
            Location = location;


        }

        public string[] ToCSV()
        {
            if (ImagesPaths == null)
            {
                string[] csvValues = { Id.ToString(), Name, Location.Country, Location.City, Type.ToString(), MaxGuests.ToString(), MinReservationDays.ToString(), CancellationDeadline.ToString() };
                return csvValues;
            }
            else
            {
                string imagesPathsStr = string.Join("|", ImagesPaths);
                string[] csvValues = { Id.ToString(), Name, Location.Country, Location.City, Type.ToString(), MaxGuests.ToString(), MinReservationDays.ToString(), CancellationDeadline.ToString(), imagesPathsStr };
                return csvValues;
            }
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Location.City = values[2];
            Location.Country = values[3];
            Type = (PropertyType)Enum.Parse(typeof(PropertyType),values[4]);
            MaxGuests = Convert.ToInt32(values[5]);
            MinReservationDays = Convert.ToInt32(values[6]);
            CancellationDeadline = Convert.ToInt32(values[7]);
            Location.Country = values[7];
            for (int i = 8; i < values.Length; i++)
            {
                ImagesPaths.Add(values[i]);
            }


        }


    }
}
