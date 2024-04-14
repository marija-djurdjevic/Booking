using BookingApp.Model;
using BookingApp.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Controls;

namespace BookingApp.Service
{
    public class LiveTourService
    {
        private readonly LiveTourRepository liveTourRepository;
        private TourReservationService tourReservationService;
        private KeyPointService keyPointService;
        private KeyPointRepository keyPointRepository;

        public LiveTourService()
        {
            liveTourRepository = new LiveTourRepository();
            tourReservationService = new TourReservationService();
            keyPointService = new KeyPointService();
            keyPointRepository = new KeyPointRepository();
        }
        public LiveTour GetLiveTourById(int tourId)
        {
            return liveTourRepository.GetLiveTourById(tourId);
        }

        public List<TourReservation> GetTouristsByTourId(int tourId)
        {
            return tourReservationService.GetByTourId(tourId);
        }

        public List<KeyPoint> GetTourKeyPoints(int tourId)
        {
            return keyPointService.GetTourKeyPoints(tourId);
        }
        public void RemoveLiveTour(int tourId)
        {
            liveTourRepository.RemoveLiveTour(tourId);
        }

        public List<LiveTour> GetAllLiveTours()
        {
            return liveTourRepository.GetAllLiveTours();
        }

        public void ActivateTour(int tourId)
        {
            liveTourRepository.ActivateTour(tourId);
        }
        public void SaveChanges()
        {
            liveTourRepository.SaveChanges();

        }

        public List<LiveTour> GetFinishedTours()
        {
            return liveTourRepository.GetFinishedTours();
        }

        public void CheckFirstKeyPoint(int tourId)
        {
            var keyPoints = GetTourKeyPoints(tourId);
            var keyPoint = keyPoints[0];
            if (keyPoints != null && keyPoints.Any())
            {
                keyPoints[0].IsChecked = true;
                keyPointRepository.Update(keyPoint);
                LiveTour liveTour = new LiveTour(tourId, keyPoints, true);
                liveTourRepository.AddOrUpdateLiveTour(liveTour);
            }
        }

        public void CheckKeyPoint(int tourId,KeyPoint keyPoint)
        {
            var keyPoints = GetTourKeyPoints(tourId);
            int ordinalNumber = keyPoint.OrdinalNumber-1;
            keyPoints[ordinalNumber].IsChecked = true;
            LiveTour liveTour = new LiveTour(tourId, keyPoints, true);
            liveTourRepository.AddOrUpdateLiveTour(liveTour);
            
        }
        public LiveTour FindLiveTourById(int tourId)
        {
            return liveTourRepository.FindLiveTourById(tourId);
        }

        public void FinishTourIfAllChecked(int tourId)
        {
            var keyPoints = GetTourKeyPoints(tourId);
            if (keyPoints != null && keyPoints.All(kp => kp.IsChecked))
            {
                FinishTour(tourId);
            }
        }
        public void FinishTour(int tourId)
        {
            var activeTour = GetLiveTourById(tourId);
            if (activeTour != null)
            {
                activeTour.IsLive = false;
                var keyPoint = activeTour.KeyPoints;
                for (int i = 0; i < activeTour.KeyPoints.Count; i++)
                {
                    keyPoint[i].IsChecked = false;
                    
                    liveTourRepository.Update(activeTour);
                }
                liveTourRepository.SaveChanges();
            }
        }


    }
}