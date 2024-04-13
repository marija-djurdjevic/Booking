﻿using BookingApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookingApp.Serializer;
using System.Threading.Tasks;

namespace BookingApp.Model
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
        public DateTime Date { get; set; }
        public NotificationType Type { get; set; }
        public bool Seen { get; set; }

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
            Date = date;
            Type = type;
            Seen = false;
            ActiveKeyPoint = activeKeyPoint;
            GuideName = guideName;
            TourName = tourName;
        }

        public virtual string[] ToCSV()
        {
            string addedPersons = string.Join("|", AddedPersons);
            string[] csvValues = { Id.ToString(), TouristId.ToString(), GuideId.ToString(), TourId.ToString(), Date.ToString("dd.MM.yyyy HH:mm:ss"), Type.ToString(), Seen.ToString(), ActiveKeyPoint, TourName, GuideName, addedPersons };
            return csvValues;
        }

        public virtual void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TouristId = Convert.ToInt32(values[1]);
            GuideId = Convert.ToInt32(values[2]);
            TourId = Convert.ToInt32(values[3]);

            Date = DateTime.ParseExact(values[4], "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);


            Type = (NotificationType)Enum.Parse(typeof(NotificationType), values[5]);
            Seen = Boolean.Parse(values[6]);
            ActiveKeyPoint = values[7];
            TourName = values[8];
            GuideName = values[9];

            for (int i = 10; i < values.Length; i++)
            {
                AddedPersons.Add(values[i]);
            }
        }
    }
}
