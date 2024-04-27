using BookingApp.Domain.Models;
using BookingApp.Repositories;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Domain.RepositoryInterfaces;
using System.Security.Cryptography;
using System.Windows.Controls;

namespace BookingApp.Aplication.UseCases
{
    public class LiveTourService
    {
        private ILiveTourRepository liveTourRepository;
        private TourReservationService tourReservationService;
        private KeyPointService keyPointService;
        private IKeyPointRepository keyPointRepository;
        private ITourReservationRepository tourReservationRepository;
        public LiveTourService(ILiveTourRepository liveTourRepository,IKeyPointRepository keyPointRepository)
        {
            this.liveTourRepository = liveTourRepository;
            tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            tourReservationService = new TourReservationService(tourReservationRepository);
            keyPointService = new KeyPointService(keyPointRepository,liveTourRepository);
            this.keyPointRepository = keyPointRepository;
        }
        public LiveTour GetLiveTourById(int tourId)
        {
            return liveTourRepository.GetById(tourId);
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
            liveTourRepository.Delete(tourId);
        }

        public List<LiveTour> GetAllLiveTours()
        {
            return liveTourRepository.GetAllLiveTours();
        }

        public void ActivateTour(int tourId)
        {
            var liveTours = liveTourRepository.GetAll();
            var liveTour = liveTours.FirstOrDefault(t => t.TourId == tourId && !t.IsLive);
            if (liveTour != null)
            {
                liveTour.IsLive = true;
                liveTourRepository.Update(liveTour);
            }
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
                liveTourRepository.Save(liveTour);
            }
        }

        public void CheckKeyPoint(int tourId, KeyPoint keyPoint)
        {
            var keyPoints = GetTourKeyPoints(tourId);
            int ordinalNumber = keyPoint.OrdinalNumber - 1;
            keyPoints[ordinalNumber].IsChecked = true;
            LiveTour liveTour = new LiveTour(tourId, keyPoints, true);
            liveTourRepository.Save(liveTour);

        }
        public LiveTour FindLiveTourById(int tourId)
        {
            var liveTours = liveTourRepository.GetAll();
            return liveTours.FirstOrDefault(t => t.TourId == tourId && t.IsLive);
        }

        public int GetLiveTourId()
        {
            var liveTours = liveTourRepository.GetAll();
            return liveTours.FirstOrDefault(t => t.IsLive).TourId;
        }


        public bool HasLiveTour()
        {
            var liveTours = liveTourRepository.GetAll();
            if (liveTours.FirstOrDefault(t => t.IsLive) != null)
            {
                return false;
            }
            return true;
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
                liveTourRepository.Update(activeTour);
                var keyPoint = activeTour.KeyPoints;
                for (int i = 0; i < activeTour.KeyPoints.Count; i++)
                {
                    keyPoint[i].IsChecked = true;

                    liveTourRepository.Update(activeTour);
                }
                liveTourRepository.SaveChanges();
            }
        }
    }
}