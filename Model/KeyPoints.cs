using BookingApp.Model.Enums;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class KeyPoints : ISerializable
    {
        public int TourId { get; set; }
        public string StartKeyPoint { get; set; }
        public List<string> MiddleKeyPoints { get; set; }
        public string EndKeyPoint { get; set; }

        public KeyPoints() { }

        public KeyPoints(int id, string startKeyPoint, List<string> middleKeyPoints, string endKeyPoint)
        {
            TourId = id;
            StartKeyPoint = startKeyPoint;
            MiddleKeyPoints = middleKeyPoints;
            EndKeyPoint = endKeyPoint;
        }

        public string[] ToCSV()
        {
            string middleKeyPointsStr = MiddleKeyPoints != null ? string.Join(",", MiddleKeyPoints) : "";
            string[] csvValues = { TourId.ToString(), StartKeyPoint, middleKeyPointsStr, EndKeyPoint };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            TourId = Convert.ToInt32(values[0]);
            StartKeyPoint = values[1];
            MiddleKeyPoints = !string.IsNullOrEmpty(values[2]) ? values[2].Split(',').ToList() : new List<string>();
            EndKeyPoint = values[3];
        }
    }
}