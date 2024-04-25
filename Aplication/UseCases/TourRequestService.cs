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

        public List<TourRequest> GetAllRequests()
        {
            return tourRequestRepository.GetAll();
        }

        
      


        public List<string> GetLocations()
        {
            List<string> locations = new List<string>();
            foreach (var request in tourRequestRepository.GetAll())
            {
                string location = $"{request.Location.City}, {request.Location.Country}";
                locations.Add(location);
            }
            return locations;
        }



        public List<string> GetLanguages()
        {
            List<string> languages = new List<string>();
            foreach (var request in tourRequestRepository.GetAll())
            {
                string language = request.Language;
                if (!languages.Contains(language))
                {
                    languages.Add(language);
                }
            }
            return languages;
        }


        public string GetMostRequestedLanguage()
        {
            var allRequests = tourRequestRepository.GetAll();
            var languageCounts = allRequests.GroupBy(r => r.Language)
                                             .Select(g => new { Language = g.Key, Count = g.Count() })
                                             .OrderByDescending(x => x.Count);

            if (languageCounts.Any())
            {
                return languageCounts.First().Language;
            }

            return string.Empty;
        }


        public string GetMostRequestedLocation()
        {
            var allRequests = tourRequestRepository.GetAll();
            var locationCounts = allRequests.GroupBy(r => $"{r.Location.City}, {r.Location.Country}")
                                            .Select(g => new { Location = g.Key, Count = g.Count() })
                                            .OrderByDescending(x => x.Count);

            if (locationCounts.Any())
            {
                return locationCounts.First().Location;
            }

            return string.Empty;
        }


        public List<(DateTime StartDate, DateTime EndDate)> GetUpcomingToursDates()
        {
            var upcomingTours = tourRepository.GetAll().Where(t => t.StartDateTime >= DateTime.Today).ToList();
            var tourDates = upcomingTours.Select(t => (t.StartDateTime, t.StartDateTime.AddHours(t.Duration))).ToList();

            return tourDates;
        }

        public (DateTime StartDate, DateTime EndDate) GetDateSlotById(int requestId)
        {
            var request = tourRequestRepository.GetById(requestId);
            return (request.StartDate, request.EndDate);
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



        public List<(DateTime, DateTime)> CalculateFreeDates(List<(DateTime, DateTime)> bookedDates, (DateTime, DateTime) touristsDates)
        {
            var freeDates = new List<(DateTime, DateTime)>();

            // Sortiraj bookedDates po početnom datumu
            var sortedBookedDates = bookedDates.OrderBy(d => d.Item1).ToList();

            // Početni slobodni datum je početak turističkog perioda
            DateTime startFreeDate = touristsDates.Item1;

            foreach (var bookedDate in sortedBookedDates)
            {
                // Ako je početak zauzetog datuma nakon početnog slobodnog datuma, dodaj slobodni opseg
                if (bookedDate.Item1 > startFreeDate)
                {
                    freeDates.Add((startFreeDate, bookedDate.Item1));
                }

                // Ažuriraj početni slobodni datum ako je kraj zauzetog datuma veći od početnog slobodnog datuma
                if (bookedDate.Item2 > startFreeDate)
                {
                    startFreeDate = bookedDate.Item2;
                }

                // Ako je početni slobodni datum veći ili jednak završnom datumu turističkog perioda, prekini petlju
                if (startFreeDate >= touristsDates.Item2)
                {
                    break;
                }
            }

            // Dodaj poslednji slobodni opseg ako je kraj zauzetog datuma manji od završnog datuma turističkog perioda
            if (startFreeDate < touristsDates.Item2)
            {
                freeDates.Add((startFreeDate, touristsDates.Item2));
            }

            return freeDates;
        }
    }
}
