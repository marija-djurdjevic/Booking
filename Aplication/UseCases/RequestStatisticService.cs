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
            var requests=GetAllRequests();
            foreach (var request in requests )
            {
                string location = $"{request.Location.City}, {request.Location.Country}";
                locations.Add(location);
            }
            return locations;
        }

        public List<string> GetLanguages()
        {
            List<string> languages = new List<string>();
            var requests = GetAllRequests();
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

        public string GetMostRequestedLanguage()
        {
            var allRequests = GetAllRequests();
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
            var allRequests = GetAllRequests();
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


            foreach (var acceptedDate in acceptedDates)
            {
                freeDates.RemoveAll(d => d.Item1 <= acceptedDate && d.Item2 >= acceptedDate);
            }

            return freeDates;
        }







    }

}