using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Aplication.UseCases
{
    public class RequestStatisticService : TourRequestService
    {
        public RequestStatisticService(ITourRequestRepository tourRequestRepository, ITourRepository tourRepository) : base(tourRequestRepository, tourRepository)
        {
        }
        public List<string> GetLocations()
        {
            List<string> locations = new List<string>();
            var requests=GetAllSimpleRequests();
            foreach (var request in requests )
            {
                
                string location = $"{request.Location.City}, {request.Location.Country}";
                if (!locations.Contains(location))
                {
                    locations.Add(location) ;
                }
               
            }
            return locations;
        }

        public List<string> GetLanguages()
        {
            List<string> languages = new List<string>();
            var requests = GetAllSimpleRequests();
            foreach (var request in requests)
            {
                string language = request.Language;
                if (!languages.Contains(language))
                {
                    languages.Add(language);
                }
            }
            return languages;
        }

        public List<string> GetYears()
        {
            List<string> years = new List<string>();
            var requests = GetAllSimpleRequests();
            foreach (var request in requests)
            {
                string startYear = request.StartDate.Year.ToString();
                if (!years.Contains(startYear))
                {
                    years.Add(startYear);
                }

                string endYear = request.EndDate.Year.ToString();
                if (!years.Contains(endYear))
                {
                    years.Add(endYear);
                }
            }
            return years;
        }

        public string GetMostRequestedLanguage()
        {
            var allRequests = GetAllSimpleRequests();
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
            var allRequests = GetAllSimpleRequests();
            var locationCounts = allRequests.GroupBy(r => $"{r.Location.City}, {r.Location.Country}")
                                            .Select(g => new { Location = g.Key, Count = g.Count() })
                                            .OrderByDescending(x => x.Count);

            if (locationCounts.Any())
            {
                return locationCounts.First().Location;
            }

            return string.Empty;
        }



        public List<(DateTime, DateTime)> CalculateFreeDates(List<(DateTime, DateTime)> bookedDates, (DateTime, DateTime) touristsDates, List<DateTime> acceptedDates)
        {
            var freeDates = new List<(DateTime, DateTime)>();

            var sortedBookedDates = bookedDates.OrderBy(d => d.Item1).ToList();

            DateTime startFreeDate = touristsDates.Item1;

            freeDates = FindFreeDates(sortedBookedDates, startFreeDate, touristsDates);

            AdjustFreeDates(freeDates, touristsDates);

            RemoveAcceptedDates(freeDates, acceptedDates);

            return freeDates;
        }

        private List<(DateTime, DateTime)> FindFreeDates(List<(DateTime, DateTime)> sortedBookedDates, DateTime startFreeDate, (DateTime, DateTime) touristsDates)
        {
            var freeDates = new List<(DateTime, DateTime)>();

            foreach (var bookedDate in sortedBookedDates)
            {
                if (bookedDate.Item1 > startFreeDate)
                {
                    freeDates.Add((startFreeDate, bookedDate.Item1));
                }

                if (bookedDate.Item2 > startFreeDate)
                {
                    startFreeDate = bookedDate.Item2;
                }

                if (startFreeDate >= touristsDates.Item2)
                {
                    break;
                }
            }

            if (startFreeDate < touristsDates.Item2)
            {
                freeDates.Add((startFreeDate, touristsDates.Item2));
            }

            return freeDates;
        }

        private void AdjustFreeDates(List<(DateTime, DateTime)> freeDates, (DateTime, DateTime) touristsDates)
        {
            foreach (var fd in freeDates.ToList())
            {
                if (fd.Item2 > touristsDates.Item2)
                {
                    var newFd = fd;
                    newFd.Item2 = touristsDates.Item2;
                    freeDates.Remove(fd);
                    freeDates.Add(newFd);
                }
            }
        }

        private void RemoveAcceptedDates(List<(DateTime, DateTime)> freeDates, List<DateTime> acceptedDates)
        {
            foreach (var acceptedDate in acceptedDates)
            {
                freeDates.RemoveAll(d => d.Item1 <= acceptedDate && d.Item2 >= acceptedDate);
            }
        }

    }

}