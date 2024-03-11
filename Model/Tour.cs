using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Model.Enums;
using BookingApp.Serializer;

namespace BookingApp.Model
{
    public class Tour : ISerializable    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int MaxTouristsNumber { get; set; }
        public DateTime StartDate { get; set; }
        public double Duration { get; set; }
        public List<string> ImagesPaths { get; set; }
        public Location Location { get; set; }


        public Tour()
        {
            Location = new Location();
            ImagesPaths = new List<string>();
           
    }
        public Tour( string name, string description, string language, int maxTouristsNumber,DateTime startDate, double duration, List<string> imagesPaths, Location location )
        {
            //Id = id;
            Name = name;
            Description = description;
            Language = language;
            MaxTouristsNumber = maxTouristsNumber;
            StartDate = startDate;
            Duration = duration;
            ImagesPaths = imagesPaths;
            Location = location;
          

        }

        public string[] ToCSV()
        {
            if (ImagesPaths == null)
            {
                string[] csvValues = { Id.ToString(), Name, Description, Language, MaxTouristsNumber.ToString(), StartDate.ToString(), Duration.ToString(), Location.Country, Location.City };
                return csvValues;
            }
            else
            {
                string imagesPathsStr = string.Join("|", ImagesPaths);
                string[] csvValues = { Id.ToString(), Name, Description, Language, MaxTouristsNumber.ToString(), StartDate.ToString(), Duration.ToString(), Location.Country, Location.City, imagesPathsStr };
                return csvValues;
            }
        }

        public void FromCSV(string[] values) 
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Description = values[2];
            Language = values[3];
            MaxTouristsNumber = Convert.ToInt32(values[4]);
            StartDate = DateTime.Parse(values[5]);
            Duration = Convert.ToInt32(values[6]);
            Location.Country = values[7];
            Location.City = values[8];
            for (int i=9; i<values.Length; i++)
            {
                ImagesPaths.Add(values[i]);
            }


        }
    }
}
