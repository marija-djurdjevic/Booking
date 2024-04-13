using BookingApp.Model;
using BookingApp.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace BookingApp.Service
{
    public class LiveTourService
    {
        private readonly LiveTourRepository liveTourRepository;
        private TourReservationService tourReservationService;
        private KeyPointService keyPointService;
        public LiveTourService()
        {
            liveTourRepository = new LiveTourRepository();
            tourReservationService = new TourReservationService();
            keyPointService = new KeyPointService();
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
            var liveTour = liveTourRepository.GetLiveTourById(tourId);
            if (liveTour != null && !liveTour.IsLive)
            {
                liveTour.IsLive = true;
                liveTourRepository.SaveChanges();
            }
        }


        public void SaveChanges()
        {
            liveTourRepository.SaveChanges();

        }

       

        public void CheckFirstKeyPoint(int tourId)
        {
            var keyPoints = GetTourKeyPoints(tourId);
            LiveTour liveTour = new LiveTour(tourId , keyPoints, true);
            liveTourRepository.AddOrUpdateLiveTour(liveTour);
            if (keyPoints != null && keyPoints.Any())
            {
                keyPoints[0].IsChecked = true;
                keyPointService.SaveChanges();
                liveTourRepository.SaveChanges();
            }
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
                    liveTourRepository.SaveChanges();
                }
                liveTourRepository.SaveChanges();
            }
        }


    }
}










