using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Serializer;

namespace BookingApp.Model
{
    public class Tour : ISerializable    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int MaxTouristsNumber { get; set; }
        public List<DateTime> TourStartDates { get; set; }
        public double Duration { get; set; }
        public List<string> ImagesPaths { get; set; }
        public Location Location { get; set; }

        public Tour() { }
        public Tour(int id, string name, string description, string language, int maxTouristsNumber, List<DateTime> tourStartDates, double duration, List<string> imagesPaths, Location location)
        {
            Id = id;
            Name = name;
            Description = description;
            Language = language;
            MaxTouristsNumber = maxTouristsNumber;
            TourStartDates = tourStartDates;
            Duration = duration;
            ImagesPaths = imagesPaths;
            Location = location;
        }

        public string[] ToCSV()
        {
            string tourStartDatesStr = string.Join(",", TourStartDates.Select(date => date.ToString()));
            string imagesPathsStr = string.Join(",", ImagesPaths);

            string[] csvValues = { Id.ToString(), Name, Description, Language, MaxTouristsNumber.ToString(), tourStartDatesStr, Duration.ToString(), imagesPathsStr, Location.Id.ToString(), Location.Country, Location.City };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Description = values[2];
            Language = values[3];
            MaxTouristsNumber = Convert.ToInt32(values[4]);
            TourStartDates = values[5].Split(',').Select(dateStr => Convert.ToDateTime(dateStr)).ToList();
            Duration = Convert.ToDouble(values[6]);
            ImagesPaths = values[7].Split(',').ToList();
            Location = new Location(Convert.ToInt32(values[8]), values[9], values[10]);
        }
    }
}
