using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookingApp.Serializer;
using System.Threading.Tasks;
using BookingApp.Domain.Models.Enums;

namespace BookingApp.Domain.Models
{
    public class TouristGuideNotification : ISerializable
    {
        public int Id { get; set; }
        public int TouristId { get; set; }
        public int GuideId { get; set; }
        public int TourId { get; set; }
        public string GuideName { get; set; }
        public string TourName { get; set; }
        public List<string> AddedPersons { get; set; }
        public DateTime CreationTime { get; set; }
        public NotificationType Type { get; set; }
        public int RequestId { get; set; }
        public DateTime AcceptedTime { get; set; }
        public bool Seen { get; set; }
        public string VoucherMessageText { get; set; }
        public string ActiveKeyPoint { get; set; }

        public TouristGuideNotification()
        {
            AddedPersons = new List<string>();
            Seen = false;
        }

        public TouristGuideNotification(int touristId, int guideId, int tourId, List<string> addedPersons, DateTime date, NotificationType type, string activeKeyPoint, string guideName, string tourName)
        {
            TouristId = touristId;
            GuideId = guideId;
            TourId = tourId;
            AddedPersons = addedPersons;
            CreationTime = date;
            Type = type;
            Seen = false;
            ActiveKeyPoint = activeKeyPoint;
            GuideName = guideName;
            TourName = tourName;
        }
        public TouristGuideNotification(int touristId, int guideId, int tourId, DateTime date, NotificationType type, string guideName)
        {
            TouristId = touristId;
            GuideId = guideId;
            TourId = tourId;
            CreationTime = date;
            Type = type;
            Seen = false;
            GuideName = guideName;
            AddedPersons = new List<string>();
        }
        public TouristGuideNotification(int touristId, int guideId, int requestId, DateTime date, NotificationType type, string guideName, DateTime acceptedDate)
        {
            TouristId = touristId;
            GuideId = guideId;
            RequestId = requestId;
            CreationTime = date;
            Type = type;
            Seen = false;
            GuideName = guideName;
            AcceptedTime = acceptedDate;
            AddedPersons = new List<string>();
        }

        public virtual string[] ToCSV()
        {
            string addedPersons = string.Join("|", AddedPersons);
            string[] csvValues = { Id.ToString(), TouristId.ToString(), GuideId.ToString(), TourId.ToString(), CreationTime.ToString("dd.MM.yyyy HH:mm:ss"), Type.ToString(), Seen.ToString(), ActiveKeyPoint, TourName, GuideName,RequestId.ToString(),AcceptedTime.ToString("dd.MM.yyyy HH:mm:ss"),VoucherMessageText, addedPersons };
            return csvValues;
        }

        public virtual void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TouristId = Convert.ToInt32(values[1]);
            GuideId = Convert.ToInt32(values[2]);
            TourId = Convert.ToInt32(values[3]);

            CreationTime = DateTime.ParseExact(values[4], "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);


            Type = (NotificationType)Enum.Parse(typeof(NotificationType), values[5]);
            Seen = bool.Parse(values[6]);
            ActiveKeyPoint = values[7];
            TourName = values[8];
            GuideName = values[9];
            RequestId = Convert.ToInt32(values[10]);
            AcceptedTime = DateTime.ParseExact(values[11], "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            VoucherMessageText = values[12];
            for (int i = 13; i < values.Length; i++)
            {
                AddedPersons.Add(values[i]);
            }
        }
    }
}
