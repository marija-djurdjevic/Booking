using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class KeyPointService
    {
        private readonly KeyPointRepository keyPointRepository;

        public KeyPointService()
        {
            keyPointRepository = new KeyPointRepository();
        }

        public List<KeyPoint> GetTourKeyPoints(int tourId)
        {
            return keyPointRepository.GetTourKeyPoints(tourId);
        }

        public void AddKeyPoint(KeyPoint keyPoint)
        {
            keyPointRepository.AddKeyPoint(keyPoint);
        }

        public void DeleteKeyPoints(int tourId)
        {
            keyPointRepository.DeleteKeyPoints(tourId);
        }
    }
}
