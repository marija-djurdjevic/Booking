using BookingApp.Model;
using BookingApp.Repository;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Service
{
    public class LiveTourService
    {
        private readonly LiveTourRepository _liveTourRepository;

        public LiveTourService()
        {
            _liveTourRepository = new LiveTourRepository();
        }

        public LiveTour GetLiveTourById(int tourId)
        {
            return _liveTourRepository.GetLiveTourById(tourId);
        }


        public List<LiveTour> GetAllLiveTours()
        {
            return _liveTourRepository.GetAllLiveTours();
        }

        public void ActivateTour(int tourId)
        {
            var liveTour = _liveTourRepository.GetLiveTourById(tourId);
            if (liveTour != null && !liveTour.IsLive)
            {
                liveTour.IsLive = true;
                _liveTourRepository.SaveChanges();
            }
        }
    }
}
