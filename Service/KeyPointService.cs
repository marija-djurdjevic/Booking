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
        private readonly LiveTourRepository liveTourService;
        public KeyPointService()
        {
            keyPointRepository = new KeyPointRepository();
            liveTourService = new LiveTourRepository();    
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

        public void SaveChanges()
        {
             keyPointRepository.SaveChanges();
        }


        public KeyPoint GetLastActiveKeyPoint()
        {
            var liveTour = liveTourService.GetAllLiveTours().FirstOrDefault(t => t.IsLive);
            if (liveTour != null)
            {
                var checkedKeyPoints = liveTour.KeyPoints.Where(k => k.IsChecked).ToList();
                if (checkedKeyPoints.Any())
                {
                    return checkedKeyPoints.Last();
                }
            }
            return null;
        }

    }
}
