using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Aplication.Dto;
using System.Windows;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.Domain.Models.Enums;

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
            CheckStatus();
        }

        public List<TourRequest> GetByTouristId(int touristId)
        {
            CheckStatus();
            var allRequests = tourRequestRepository.GetAll();
            return allRequests.FindAll(t=>t.TouristId==touristId);
        }

        private void CheckStatus()
        {
            foreach(var request in tourRequestRepository.GetAll())
            {
                if (request.StartDate < DateTime.Now.AddHours(48) && request.Status==TourRequestStatus.Pending)
                {
                    request.Status = TourRequestStatus.Invalid;
                    tourRequestRepository.Update(request);
                }
            }
        }

        public void SortTours(ObservableCollection<Tuple<TourRequest, string>> unsorted, string sortBy)
        {
            var sorted = new List<Tuple<TourRequest, string>>();
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
