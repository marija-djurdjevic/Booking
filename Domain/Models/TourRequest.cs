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
        public int ComplexId { get; set; }
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
            ComplexId = -1;
        }

        public TourRequest(int id, int guideId,int complexId, int touristId, Location location, string description, string language, int touristNumber, DateTime startDate, DateTime endDate, List<Tuple<string, string, int>> persons, TourRequestStatus status, DateTime acceptedDate)
        {
            Id = id;
            GuideId = guideId;
            TouristId = touristId;
            Location = location;
            Description = description;
            Language = language;
            TouristNumber = touristNumber;
            StartDate = startDate;
            EndDate = endDate;
            Persons = persons;
            Status = status;
            AcceptedDate = acceptedDate;
            ComplexId = complexId;
        }

        public string[] ToCSV()
        {
            string persons = "";
            foreach (var person in Persons)
            {
                persons += person.Item1 + "," + person.Item2 + "," + person.Item3 + "|";
            }
            persons=persons.Remove(persons.Length - 1);
            string[] csvValues = { Id.ToString(), TouristId.ToString(), GuideId.ToString(),ComplexId.ToString(), Location.City, Location.Country, Description, Language, TouristNumber.ToString(), StartDate.ToString("dd.MM.yyyy HH:mm:ss"), EndDate.ToString("dd.MM.yyyy HH:mm:ss"), Status.ToString(), AcceptedDate.ToString("dd.MM.yyyy HH:mm:ss"), persons };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TouristId = Convert.ToInt32(values[1]);
            GuideId = Convert.ToInt32(values[2]);
            ComplexId = Convert.ToInt32(values[3]);
            Location.City = values[4];
            Location.Country = values[5];
            Description = values[6];
            Language = values[7];
            TouristNumber = Convert.ToInt32(values[8]);
            StartDate = DateTime.ParseExact(values[9], "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture); ;
            EndDate = DateTime.ParseExact(values[10], "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture); ;
            Status = (TourRequestStatus)Enum.Parse(typeof(TourRequestStatus), values[11]);
            AcceptedDate = DateTime.ParseExact(values[12], "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture); ;
            for (int i = 13; i < values.Length; i++)
            {
                var osoba = values[i].Split(',');
                Tuple<string, string, int> osobaTuple = new Tuple<string, string, int>(osoba[0], osoba[1], Convert.ToInt32(osoba[2]));
                Persons.Add(osobaTuple);
            }
        }
    }
}
