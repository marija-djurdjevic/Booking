using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Models.Enums;
using BookingApp.WPF.ViewModels.TouristViewModels;
using System.Net.WebSockets;
using System.CodeDom;

namespace BookingApp.Aplication.UseCases
{
    public class TourRequestService
    {
        private readonly ITourRequestRepository tourRequestRepository;
        private readonly ITourRepository tourRepository;

        public TourRequestService(ITourRequestRepository tourRequestRepository, ITourRepository tourRepository)
        {
            this.tourRequestRepository = tourRequestRepository;
            this.tourRepository = tourRepository;
        }

        public void CreateRequest(TourRequest tourRequest)
        {
            tourRequestRepository.Save(tourRequest);
            CheckStatus();
        }

        public List<TourRequest> GetNotAcceptedRequests()
        {
            CheckStatus();
            return GetAllSimpleRequests().FindAll(t => t.Status != TourRequestStatus.Accepted);
        }

        public HashSet<int> GetTouristIdsInterestedForTour(string language,string location)
        {
            HashSet<int> touristIds = new HashSet<int>();
            foreach(var request in GetNotAcceptedRequests())
            {
                if(request.Language.ToLower() == language.ToLower() || request.Location.City.ToLower() == location.ToLower())
                {
                    touristIds.Add(request.TouristId);
                }
            }
            return touristIds;
        }

        public List<TourRequest> GetByTouristId(int touristId)
        {
            CheckStatus();
            var allRequests = GetAllSimpleRequests();
            return allRequests.FindAll(t => t.TouristId == touristId);
        }

        private void CheckStatus()
        {
            foreach (var request in tourRequestRepository.GetAll())
            {
                if (request.StartDate < DateTime.Now.AddHours(48) && request.Status == TourRequestStatus.Pending)
                {
                    request.Status = TourRequestStatus.Invalid;
                    tourRequestRepository.Update(request);
                }
            }
        }

        public List<TourRequest> GetAllSimpleRequests()
        {
            return tourRequestRepository.GetAll().FindAll(r=>r.ComplexId==-1);
        }


        public List<TourRequest> GetAllComplexRequests()
        {
            return tourRequestRepository.GetAll().FindAll(r => r.ComplexId != -1);
        }


        public List<TourRequest>GetAllRequests()
        {
            return tourRequestRepository.GetAll();
        }
        
        public List<(DateTime StartDate, DateTime EndDate)> GetUpcomingToursDates()
        {
            var upcomingTours = tourRepository.GetAll().Where(t => t.StartDateTime >= DateTime.Today).ToList();
            var tourDates = upcomingTours.Select(t => (t.StartDateTime, t.StartDateTime.AddHours(t.Duration))).ToList();

            return tourDates;
        }


        public void UpdateRequestById(int requestId, DateTime newAcceptedDate)
        {
            var request = tourRequestRepository.GetById(requestId);
            if (request != null)
            {
                request.AcceptedDate = newAcceptedDate;
                request.Status= TourRequestStatus.Accepted;
                tourRequestRepository.Update(request);
            }
        }

        public TourRequest GetRequestById(int requestId)
        {
            return tourRequestRepository.GetById(requestId);
        }


        public List<DateTime> GetAllAcceptedDates()
        {
            var allrequests=tourRequestRepository.GetAll();
            var acceptedDates = new List<DateTime>();
            foreach(var request in allrequests)
            {
                if (request.Status == TourRequestStatus.Accepted)
                {
                    acceptedDates.Add(request.AcceptedDate);
                }
            }
            return acceptedDates;
        }


        public (DateTime StartDate, DateTime EndDate) GetDateSlotById(int requestId)
        {
            var request = tourRequestRepository.GetById(requestId);
            return (request.StartDate, request.EndDate);
        }

        public void SortTours(ObservableCollection<Tuple<TourRequestViewModel, string>> unsorted, string sortBy)
        {
            var sorted = new List<Tuple<TourRequestViewModel, string>>();
            switch (sortBy)
            {
                case "System.Windows.Controls.ComboBoxItem: Date - Ascending":
                    sorted = unsorted.OrderBy(t => t.Item1.StartDate).ThenBy(t => t.Item1.Status).ToList();
                    break;
                case "System.Windows.Controls.ComboBoxItem: Date - Descending":
                    sorted = unsorted.OrderByDescending(t => t.Item1.StartDate).ThenByDescending(t => t.Item1.Status).ToList();
                    break;
                case "System.Windows.Controls.ComboBoxItem: Status - Ascending":
                    sorted = unsorted.OrderBy(t => t.Item1.Status).ThenBy(t => t.Item1.StartDate).ToList();
                    break;
                case "System.Windows.Controls.ComboBoxItem: Status - Descending":
                    sorted = unsorted.OrderByDescending(t => t.Item1.Status).ThenByDescending(t => t.Item1.StartDate).ToList();
                    break;
                default:
                    return;
            }
            unsorted.Clear();
            foreach (var sortedTour in sorted)
            {
                unsorted.Add(sortedTour);
            }
            return;
        }
       
    }
}
