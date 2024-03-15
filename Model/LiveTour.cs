using BookingApp.Model.Enums;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace BookingApp.Model
{
    public class LiveTour: ISerializable
    {
        public int TourId { get; set; }
        public List<KeyPoint> KeyPoints { get; set; }
        public bool IsLive {  get; set; }

        public LiveTour() 
        { 
            KeyPoints = new List<KeyPoint>();
        }

        public LiveTour(int tourId, List<KeyPoint> keyPoints,bool isLive)
        {
            TourId = tourId;
            KeyPoints= keyPoints;
            IsLive = isLive;
        }

        public string[] ToCSV()
        {
            List<string> csvValues = new List<string>();
            csvValues.Add(TourId.ToString());
            csvValues.Add(IsLive.ToString());
            csvValues.AddRange(KeyPoints.Select(kp => $"{kp.TourId}|{kp.Name}|{kp.Type}|{kp.OrdinalNumber}|{kp.IsChecked}"));

            return csvValues.ToArray();
        }

        public void FromCSV(string[] values)
        {
            TourId = int.Parse(values[0]);
            IsLive = bool.Parse(values[1]);
            KeyPoints = new List<KeyPoint>();

            for (int i = 2; i < values.Length; i += 5) 
            {
                KeyPoints.Add(new KeyPoint
                {
                    TourId = int.Parse(values[i]),
                    Name = values[i + 1],
                    Type = (KeyPointType)Enum.Parse(typeof(KeyPointType), values[i + 2]),
                    OrdinalNumber = int.Parse(values[i + 3]),
                    IsChecked = bool.Parse(values[i + 4])
                });
            }
        }

    }
}

