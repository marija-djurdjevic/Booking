using BookingApp.Model.Enums;
using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BookingApp.Repository
{
    public class KeyPointsRepository
    {
        private const string FilePath = "../../../Resources/Data/keyPoints.csv";
        private readonly Serializer<KeyPoint> _serializer;

        private List<KeyPoint> _keyPoints;

        public KeyPointsRepository()
        {
            _serializer = new Serializer<KeyPoint>();

            if (!File.Exists(FilePath))
            {
                File.Create(FilePath).Close(); 
            }

            _keyPoints = _serializer.FromCSV(FilePath);
        }

        public void AddKeyPoint(int tourId, string keyName, KeyPointType keyType, int ordinalNumber, bool isChecked)
        {
            KeyPoint keyPoint = new KeyPoint()
            {
                TourId = tourId,
                Name = keyName,
                Type = keyType,
                OrdinalNumber = ordinalNumber,
                IsChecked = isChecked
            };
            _keyPoints.Add(keyPoint);
            SaveChanges();
        }

        private void SaveChanges()
        {
            _serializer.ToCSV(FilePath, _keyPoints);
        }

        public List<KeyPoint> GetTourKeyPoints(int tourId)
        {
            List<KeyPoint> keyPointsForTour = new List<KeyPoint>();
            foreach (var keyPoint in _keyPoints)
            {
                if (keyPoint.TourId == tourId)
                {
                    keyPointsForTour.Add(keyPoint);
                }
            }
            return keyPointsForTour;
        }

    }
}
