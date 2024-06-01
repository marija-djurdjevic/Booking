using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Serializer;
using BookingApp.View;

namespace BookingApp.Domain.Models
{
    public class Renovation : ISerializable
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int PropertyId { get; set; }
        public DateTime StartDate {  get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string PropertyName { get; set; }

        public Renovation() { }
        public Renovation(int id, int ownerId, int propertyId, DateTime startDate, DateTime endDate, string description, int duration)
        {
            Id = id;
            OwnerId = ownerId;
            PropertyId = propertyId;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            Duration = duration;
        }
        public Renovation(int ownerId, int propertyId, DateTime startDate, DateTime endDate, string description, int duration)
        {
            OwnerId = ownerId;
            PropertyId = propertyId;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            Duration = duration;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), OwnerId.ToString(), PropertyId.ToString(), StartDate.ToString("dd.MM.yyyy HH:mm:ss"), EndDate.ToString("dd.MM.yyyy HH:mm:ss"), Description, Duration.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            OwnerId = Convert.ToInt32(values[1]);
            PropertyId = Convert.ToInt32(values[2]);
            StartDate = DateTime.ParseExact(values[3], "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            EndDate = DateTime.ParseExact(values[4], "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            Description = values[5];
            Duration = Convert.ToInt32(values[6]);
            
        }
    }
}
