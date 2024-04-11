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
        private readonly KeyPointRepository _keyPointRepository;

        public KeyPointService()
        {
            _keyPointRepository = new KeyPointRepository();
        }

        public List<KeyPoint> GetTourKeyPoints(int tourId)
        {
            return _keyPointRepository.GetTourKeyPoints(tourId);
        }

        public void AddKeyPoint(KeyPoint keyPoint)
        {
            _keyPointRepository.AddKeyPoint(keyPoint);
        }

        public void DeleteKeyPoints(int tourId)
        {
            _keyPointRepository.DeleteKeyPoints(tourId);
        }
    }
}
