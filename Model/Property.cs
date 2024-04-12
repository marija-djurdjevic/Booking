using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.Model
{
     public enum PropertyType { Apartment, House, Cabin };
     public class Property : ISerializable
    {
        public int Id { get; set; }
        public int OwnerId {  get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public PropertyType Type { get; set; }
        public int MaxGuests { get; set; }
        public int MinReservationDays { get; set; }
        public int CancellationDeadline { get; set; }
        public List<string> ImagesPaths { get; set; }
        public List<ReservedDate> ReservedDates { get; set; }

        public Property()
        {
            Location = new Location();
            ImagesPaths = new List<string>();
            ReservedDates = new List<ReservedDate>();
        }
        public Property(int id, int ownerId, string name, Location location, PropertyType type, int maxGuests, int minReservationDays, int cancellationDeadline, List<string> imagesPaths)
        {
            Id = id;
            OwnerId = ownerId;
            Name = name;
            Type = type;
            MaxGuests = maxGuests;
            MinReservationDays = minReservationDays;
            CancellationDeadline = cancellationDeadline;
            ImagesPaths = imagesPaths;
            Location = location;


        }
        public Property(int ownerId, string name, Location location, PropertyType type, int maxGuests, int minReservationDays, int cancellationDeadline, List<string> imagesPaths)
        {
            OwnerId = ownerId;
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
                string[] csvValues = { Id.ToString(),OwnerId.ToString(), Name, Location.City, Location.Country, Type.ToString(), MaxGuests.ToString(), MinReservationDays.ToString(), CancellationDeadline.ToString() };
                return csvValues;
            }
            else
            {
                string imagesPathsStr = string.Join("|", ImagesPaths);
                string[] csvValues = { Id.ToString(),OwnerId.ToString(), Name, Location.City, Location.Country, Type.ToString(), MaxGuests.ToString(), MinReservationDays.ToString(), CancellationDeadline.ToString(), imagesPathsStr };
                return csvValues;
            }
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            OwnerId = Convert.ToInt32(values[1]);
            Name = values[2];
            Location.City = values[3];
            Location.Country = values[4];
            Type = (PropertyType)Enum.Parse(typeof(PropertyType),values[5]);
            MaxGuests = Convert.ToInt32(values[6]);
            MinReservationDays = Convert.ToInt32(values[7]);
            CancellationDeadline = Convert.ToInt32(values[8]);
            for (int i = 9; i < values.Length; i++)
            {
                ImagesPaths.Add(values[i]);
            }


        }


    }
}
