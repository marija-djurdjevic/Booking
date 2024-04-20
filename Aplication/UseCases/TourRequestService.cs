using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;

namespace BookingApp.Aplication.UseCases
{
    public class TourRequestService
    {
        private readonly ITourRequestRepository tourRequestRepository;

        public TourRequestService(ITourRequestRepository tourRequestRepository)
        {
            this.tourRequestRepository = tourRequestRepository;
        }

        public void CreateRequest(TourRequest tourRequest)
        {
            tourRequestRepository.Save(tourRequest);
        }

        public List<TourRequest> GetByTouristId(int touristId)
        {
            var allRequests = tourRequestRepository.GetAll();
            return allRequests.FindAll(t=>t.TouristId==touristId);
        }

        internal void SortTours(ObservableCollection<TourRequest> tourRequests, string requestsSelectedSort)
        {
            throw new NotImplementedException();
        }
    }
}
