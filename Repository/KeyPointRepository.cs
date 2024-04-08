using BookingApp.Model.Enums;
using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BookingApp.Repository
{
    public class KeyPointRepository
    {
        private const string FilePath = "../../../Resources/Data/keyPoints.csv";
        private readonly Serializer<KeyPoint> _serializer;

        private List<KeyPoint> keyPoints;

        public KeyPointRepository()
        {
            _serializer = new Serializer<KeyPoint>();

            if (!File.Exists(FilePath))
            {
                File.Create(FilePath).Close(); 
            }

            keyPoints = _serializer.FromCSV(FilePath);
        }

        public void AddKeyPoint(KeyPoint keyPoint)
        {
                keyPoints.Add(keyPoint);
                SaveChanges();
        }

        private void SaveChanges()
        {
            _serializer.ToCSV(FilePath, keyPoints);
        }

        public List<KeyPoint> GetTourKeyPoints(int tourId)
        {
            List<KeyPoint> keyPointsForTour = new List<KeyPoint>();
            foreach (var keyPoint in keyPoints)
            {
                if (keyPoint.TourId == tourId)
                {
                    keyPointsForTour.Add(keyPoint);
                }
            }
            return keyPointsForTour;
        }

        public void DeleteKeyPoints(int tourId)
        {
            keyPoints.RemoveAll(kp => kp.TourId == tourId);
            SaveChanges();
        }


    }
}
