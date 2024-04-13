using BookingApp.Model;
using BookingApp.Repository;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Service
{
    public class LiveTourService
    {
        private readonly LiveTourRepository liveTourRepository;

        public LiveTourService()
        {
            liveTourRepository = new LiveTourRepository();
        }

        public LiveTour GetLiveTourById(int tourId)
        {
            return liveTourRepository.GetLiveTourById(tourId);
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
    }
}
