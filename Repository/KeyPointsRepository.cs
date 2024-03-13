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
        private readonly Serializer<KeyPoints> _serializer;

        private List<KeyPoints> _keyPoints;

        public KeyPointsRepository()
        {
            _serializer = new Serializer<KeyPoints>();

            if (!File.Exists(FilePath))
            {
                File.Create(FilePath).Close(); 
            }

            _keyPoints = _serializer.FromCSV(FilePath);
        }

        public void AddKeyPoint(int tourId, string keyName, KeyPoint keyType, int ordinalNumber, bool isChecked)
        {
            KeyPoints keyPoint = new KeyPoints()
            {
                TourId = tourId,
                KeyName = keyName,
                KeyType = keyType,
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

        public List<KeyPoints> GetKeyPointsForTour(int tourId)
        {
            List<KeyPoints> keyPointsForTour = new List<KeyPoints>();
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
