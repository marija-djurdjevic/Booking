using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class KeyPointsRepository
    {
        private const string FilePath = "../../../Resources/Data/key.csv";
        private readonly Serializer<KeyPoints> _serializer;

        private List<KeyPoints> _keyPoints;

        public KeyPointsRepository()
        {
            _serializer = new Serializer<KeyPoints>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            _keyPoints = _serializer.FromCSV(FilePath);
        }


        public void AddKeyPoint(int tourId, string startKeyPoint, List<string> middleKeyPoints, string endKeyPoint)
        {
            KeyPoints keyPoint = new KeyPoints()
            {
                TourId = tourId,
                StartKeyPoint = startKeyPoint,
                MiddleKeyPoints = middleKeyPoints,
                EndKeyPoint = endKeyPoint
            };
            _keyPoints.Add(keyPoint);
            SaveChanges();
        }

        private void SaveChanges()
        {
            _serializer.ToCSV(FilePath, _keyPoints);
        }


    }
}
