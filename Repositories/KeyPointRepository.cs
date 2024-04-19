using BookingApp.Domain.Models.Enums;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Repositories
{
    public class KeyPointRepository : IKeyPointRepository
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

        public List<KeyPoint> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public KeyPoint GetById(int keyPointId)
        {
            keyPoints = GetAll();
            return keyPoints.FirstOrDefault(t => t.Id == keyPointId);
        }

        public void Save(KeyPoint keyPoint)
        {
            int nextId = NextId();
            keyPoint.Id = nextId;

            keyPoints.Add(keyPoint);
            SaveChanges();
        }

        public KeyPoint Update(KeyPoint keyPoint)
        {
            keyPoints = _serializer.FromCSV(FilePath);
            KeyPoint current = keyPoints.Find(c => c.Id == keyPoint.Id);
            int index = keyPoints.IndexOf(current);
            keyPoints.Remove(current);
            keyPoints.Insert(index, keyPoint);
            _serializer.ToCSV(FilePath, keyPoints);
            return keyPoint;
        }

        public void Delete(int tourId)
        {
            keyPoints.RemoveAll(kp => kp.TourId == tourId);
            SaveChanges();
        }

        public int NextId()
        {
            keyPoints = _serializer.FromCSV(FilePath);
            if (keyPoints.Count < 1)
            {
                return 1;
            }
            return keyPoints.Max(t => t.Id) + 1;
        }

        public void SaveChanges()
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
    }
}
