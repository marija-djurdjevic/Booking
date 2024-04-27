using BookingApp.Domain.Models.Enums;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Aplication.UseCases
{
    public class TourRequestStatisticsService : TourRequestService
    {
        public TourRequestStatisticsService(ITourRequestRepository tourRequestRepository, ITourRepository tourRepository) : base(tourRequestRepository, tourRepository)
        {
        }

        public (int, int, double) GetStatisticsForYear(string year, int id)
        {
            var allRequests = GetTourRequestsForYear(year, id);
            double averagePeopleNumber = 0;
            var accepted = allRequests.Count(r => r.Status == TourRequestStatus.Accepted);
            var rejected = allRequests.Count(r => r.Status != TourRequestStatus.Accepted);

            if (allRequests.FindAll(t => t.Status == TourRequestStatus.Accepted).Count > 0)
                averagePeopleNumber = allRequests.FindAll(t => t.Status == TourRequestStatus.Accepted).Average(r => r.Persons.Count());

            return (accepted, rejected, averagePeopleNumber);
        }

        public Dictionary<string, int> GetLanguageRequestCounts(int id)
        {
            Dictionary<string, int> languageCounts = new Dictionary<string, int>();

            foreach (var tourRequest in GetByTouristId(id))
            {
                string language = tourRequest.Language;
                if (languageCounts.ContainsKey(language))
                {
                    languageCounts[language]++;
                }
                else
                {
                    languageCounts[language] = 1;
                }
            }
            return languageCounts;
        }

        public Dictionary<string, int> GetLocationsRequestCounts(int id)
        {
            Dictionary<string, int> locationsCounts = new Dictionary<string, int>();

            foreach (var tourRequest in GetByTouristId(id))
            {
                string location = tourRequest.Location.City;
                if (locationsCounts.ContainsKey(location))
                {
                    locationsCounts[location]++;
                }
                else
                {
                    locationsCounts[location] = 1;
                }
            }
            return locationsCounts;
        }

        public List<TourRequest> GetTourRequestsForYear(string year, int id)
        {
            var allRequests = GetByTouristId(id);
            int parsedYear;
            if (!int.TryParse(year, out parsedYear))
            {
                return allRequests;
            }
            return allRequests.FindAll(r => r.StartDate.Year == parsedYear);
        }

        public List<string> GetRequestsYears()
        {
            var requestsYears = new HashSet<string>();
            var allRequests = GetAllRequests();
            foreach (var request in allRequests)
            {
                string year = request.StartDate.Year.ToString();
                requestsYears.Add(year); // Dodavanje samo ako element ne postoji u listi
            }
            return requestsYears.ToList<string>();
        }
    }
}
