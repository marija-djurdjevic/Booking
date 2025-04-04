﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Models.Enums;
using BookingApp.WPF.ViewModels.TouristViewModels;

namespace BookingApp.Aplication.UseCases
{
    public class ComplexTourRequestService
    {
        private readonly ITourRequestRepository tourRequestRepository;
        private readonly IComplexTourRequestRepository complexTourRequestRepository;

        public ComplexTourRequestService(ITourRequestRepository tourRequestRepository, IComplexTourRequestRepository complexTourRequestRepository)
        {
            this.tourRequestRepository = tourRequestRepository;
            this.complexTourRequestRepository = complexTourRequestRepository;
        }

        public void CreateComplexRequest(ComplexTourRequest tourRequest)
        {
            var Id = complexTourRequestRepository.Save(tourRequest);
            foreach (var simpleRequest in tourRequest.TourRequests)
            {
                simpleRequest.ComplexId = Id;
                tourRequestRepository.Save(simpleRequest);
            }
        }

        public List<ComplexTourRequest> GetByTouristId(int touristId)
        {
            var allRequests = GetAllComplexRequests();
            return allRequests.FindAll(t => t.TouristId == touristId);
        }

        private void CheckStatus(List<ComplexTourRequest> complexRequests)
        {
            foreach (var complexRequest in complexRequests)
            {
                foreach (var simpleRequest in complexRequest.TourRequests)
                {
                    if (simpleRequest.StartDate < DateTime.Now.AddHours(48) && simpleRequest.Status == TourRequestStatus.Pending)
                    {
                        simpleRequest.Status = TourRequestStatus.Invalid;
                        tourRequestRepository.Update(simpleRequest);
                    }
                }
                if (complexRequest.TourRequests.All(t => t.Status == TourRequestStatus.Accepted))
                {
                    complexRequest.Status = TourRequestStatus.Accepted;
                }
                else if (complexRequest.TourRequests.Any(t => t.Status == TourRequestStatus.Invalid))
                {
                    complexRequest.Status = TourRequestStatus.Invalid;
                }
                else
                {
                    complexRequest.Status = TourRequestStatus.Pending;
                }
                complexTourRequestRepository.Update(complexRequest);
            }
        }

        public List<ComplexTourRequest> GetAllComplexRequests()
        {
            var allComplexRequests = complexTourRequestRepository.GetAll();
            foreach (var complexRequest in allComplexRequests)
            {
                complexRequest.TourRequests = tourRequestRepository.GetByComplexId(complexRequest.Id);
            }
            CheckStatus(allComplexRequests);

            return allComplexRequests;
        }


        public List<ComplexTourRequest> GetAllPendingComplexRequests()
        {
            var allComplexRequests = complexTourRequestRepository.GetAll();
            foreach (var complexRequest in allComplexRequests)
            {
                complexRequest.TourRequests = tourRequestRepository.GetByComplexId(complexRequest.Id);
            }

            // Ukloni prihvaćene zahteve
            foreach (var complexRequest in allComplexRequests)
            {
                complexRequest.TourRequests = complexRequest.TourRequests.Where(tr => tr.Status != TourRequestStatus.Accepted).ToList();
            }

            // Provera statusa nakon filtriranja
            CheckStatus(allComplexRequests);

            // Filtriraj samo zahteve koji imaju ne-prihvaćene zahteve
            var pendingRequests = allComplexRequests.Where(ct => ct.TourRequests.Any(tr => tr.Status != TourRequestStatus.Accepted)).ToList();

            return pendingRequests;
        }


        public void SortTours(ObservableCollection<Tuple<ComplexTourRequest, string>> unsorted, string sortBy)
        {
            var sorted = new List<Tuple<ComplexTourRequest, string>>();
            switch (sortBy)
            {
                case "System.Windows.Controls.ComboBoxItem: Date - Ascending":
                    sorted = unsorted.OrderBy(t => t.Item1.TourRequests[0].StartDate).ThenBy(t => t.Item1.Status).ToList();
                    break;
                case "System.Windows.Controls.ComboBoxItem: Date - Descending":
                    sorted = unsorted.OrderByDescending(t => t.Item1.TourRequests[0].StartDate).ThenByDescending(t => t.Item1.Status).ToList();
                    break;
                case "System.Windows.Controls.ComboBoxItem: Status - Ascending":
                    sorted = unsorted.OrderBy(t => t.Item1.Status).ThenBy(t => t.Item1.TourRequests[0].StartDate).ToList();
                    break;
                case "System.Windows.Controls.ComboBoxItem: Status - Descending":
                    sorted = unsorted.OrderByDescending(t => t.Item1.Status).ThenByDescending(t => t.Item1.TourRequests[0].StartDate).ToList();
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
