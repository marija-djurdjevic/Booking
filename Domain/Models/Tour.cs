using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Models.Enums;
using BookingApp.Repositories;
using BookingApp.Serializer;

namespace BookingApp.Domain.Models
{
    public class Tour : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int MaxTouristsNumber { get; set; }
        public DateTime StartDateTime { get; set; }
        public double Duration { get; set; }
        public List<string> ImagesPaths { get; set; }
        public Location Location { get; set; }
        public List<KeyPoint> KeyPoints { get; set; }

        public int GuideId { get; set; }

        public Tour()
        {
            Location = new Location();
            ImagesPaths = new List<string>();
            KeyPoints = new List<KeyPoint>();
        }

        public Tour(string name, string description, string language, int maxTouristsNumber, DateTime startDateTime, double duration, List<string> imagesPaths, Location location,int guideId)
        {
            Name = name;
            Description = description;
            Language = language;
            MaxTouristsNumber = maxTouristsNumber;
            StartDateTime = startDateTime;
            Duration = duration;
            ImagesPaths = imagesPaths;
            Location = location;
            KeyPoints = new List<KeyPoint>();
            GuideId= guideId;
        }

        public Tour(int id, string name, string description, string language, int maxTouristNumber, DateTime startDate, double duration, List<string> imagesPaths, Location location, int guideId)
        {
            Id = id;
            Name = name;
            Description = description;
            Language = language;
            MaxTouristsNumber = maxTouristNumber;
            StartDateTime = startDate;
            Duration = duration;
            ImagesPaths = imagesPaths;
            Location = location;
            GuideId= guideId;
        }

        public string[] ToCSV()
        {

            string imagesPathsStr = string.Join("|", ImagesPaths);
            string[] csvValues = { Id.ToString(), Name, Description, Language, MaxTouristsNumber.ToString(), StartDateTime.ToString("dd.MM.yyyy HH:mm:ss"), Duration.ToString(), Location.Country, Location.City, GuideId.ToString(), imagesPathsStr };
            return csvValues;

        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Description = values[2];
            Language = values[3];
            MaxTouristsNumber = Convert.ToInt32(values[4]);

            // Parsiranje stringa datuma u DateTime koristeći određeni format
            StartDateTime = DateTime.ParseExact(values[5], "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

            Duration = Convert.ToInt32(values[6]);
            Location.Country = values[7];
            Location.City = values[8];
            GuideId = Convert.ToInt32(values[9]);
            for (int i = 10; i < values.Length; i++)
            {
                ImagesPaths.Add(values[i]);
            }
        }
    }
}
