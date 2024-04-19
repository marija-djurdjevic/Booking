using BookingApp.Domain.Models;
using BookingApp.Domain.Models.Enums;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using BookingApp.Domain.RepositoryInterfaces;
using System.Threading.Tasks;

namespace BookingApp.Aplication.UseCases
{
    public class KeyPointService
    {
        private readonly IKeyPointRepository keyPointRepository;
        private readonly ILiveTourRepository liveTourService;
        public KeyPointService(IKeyPointRepository keyPointRepository,ILiveTourRepository liveTourRepository)
        {
            this.keyPointRepository = keyPointRepository;
            this.liveTourService = liveTourRepository;
        }

        public KeyPoint Update(KeyPoint keyPoint)
        {
            return keyPointRepository.Update(keyPoint);
        }

        public List<KeyPoint> GetTourKeyPoints(int tourId)
        {
            return keyPointRepository.GetTourKeyPoints(tourId);
        }

        public void AddKeyPoint(KeyPoint keyPoint)
        {
            keyPointRepository.Save(keyPoint);
        }

        public void DeleteKeyPoints(int tourId)
        {
            keyPointRepository.Delete(tourId);
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

        public bool SetKeyPoints(int tourId, ObservableCollection<string> keyPointNames)
        {
            string[] keyPointsArray = keyPointNames.ToArray();
            if (keyPointsArray.Length < 2 || string.IsNullOrEmpty(keyPointsArray[1]))
            {
                return false;
            }

            for (int i = 0; i < keyPointsArray.Length; i++)
            {
                string keyPointName = keyPointsArray[i];
                KeyPointType keyType = i == 0 ? KeyPointType.Begining : (i == keyPointsArray.Length - 1 ? KeyPointType.End : KeyPointType.Middle);
                int ordinalNumber = i + 1;
                bool isChecked = false;
                KeyPoint keyPoint = new KeyPoint(tourId, keyPointName, keyType, ordinalNumber, isChecked);
                keyPointRepository.Save(keyPoint);

            }
            return true;
        }
    }
}
