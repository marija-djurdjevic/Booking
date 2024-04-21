using BookingApp.Domain.Models.Enums;
using BookingApp.View.GuideView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class TourRequest : BookingApp.Serializer.ISerializable
    {
        public int Id { get; set; }
        public int GuideId { get; set; }
        public int TouristId { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int TouristNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Tuple<string, string, int>> Persons { get; set; }
        public TourRequestStatus Status { get; set; }
        public DateTime AcceptedDate { get; set; }

        public TourRequest()
        {
            Location = new Location();
            Persons = new List<Tuple<string, string, int>>();
            Status = TourRequestStatus.Pending;
        }

        public TourRequest(int id, int touristId, Location location, string description, string language, int touristNumber, DateTime startDate, DateTime endDate)
        {
            Id = id;
            TouristId = touristId;
            Location = location;
            Description = description;
            Language = language;
            TouristNumber = touristNumber;
            StartDate = startDate;
            EndDate = endDate;
            Status = TourRequestStatus.Pending;
        }

        public string[] ToCSV()
        {
            string persons = "";
            foreach (var person in Persons)
            {
                persons += person.Item1 + "," + person.Item2 + "," + person.Item3 + ",";
            }
            string[] csvValues = { Id.ToString(), TouristId.ToString(), GuideId.ToString(), Location.City, Location.Country, Description, Language, TouristNumber.ToString(), StartDate.ToString("dd.MM.yyyy HH:mm:ss"), EndDate.ToString("dd.MM.yyyy HH:mm:ss"), Status.ToString(), AcceptedDate.ToString("dd.MM.yyyy HH:mm:ss"), persons };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TouristId = Convert.ToInt32(values[1]);
            GuideId = Convert.ToInt32(values[2]);
            Location.City = values[3];
            Location.Country = values[4];
            Description = values[5];
            Language = values[6];
            TouristNumber = Convert.ToInt32(values[7]);
            StartDate = DateTime.ParseExact(values[8], "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture); ;
            EndDate = DateTime.ParseExact(values[9], "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture); ;
            Status = (TourRequestStatus)Enum.Parse(typeof(TourRequestStatus), values[10]);
            AcceptedDate = DateTime.ParseExact(values[11], "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture); ;
            for (int i = 12; i < values.Length; i++)
            {
                var osoba = values[i].Split(',');
                Tuple<string, string, int> osobaTuple = new Tuple<string, string, int>(osoba[0], osoba[1], Convert.ToInt32(osoba[2]));
                Persons.Add(osobaTuple);
            }
        }
    }
}
