using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookingApp.Repositories;
using System.Threading.Tasks;
using BookingApp.Domain.Models;
using BookingApp.Aplication.Dto;

namespace BookingApp.Aplication.UseCases
{
    public class SearchTourService
    {
        private readonly TourRepository tourRepository;

        public SearchTourService()
        {
            tourRepository = new TourRepository();
        }

        public List<Tour> GetMatchingTours(TourDto searchParams)
        {
            var tours = tourRepository.GetAll();
            List<Tour> matchingTours = tours.Where(t =>
            IsCityMatch(t, searchParams) &&
            IsCountryMatch(t, searchParams) &&
            IsDurationMatch(t, searchParams) &&
            IsLanguageMatch(t, searchParams) &&
            IsMaxTouristNumberMatch(t, searchParams)).ToList();
            return matchingTours;
        }

        public bool IsCityMatch(Tour t, TourDto searchParams)
        {
            return string.IsNullOrEmpty(searchParams.LocationDto.City) || t.Location.City.ToLower().Contains(searchParams.LocationDto.City.ToLower());
        }

        public bool IsCountryMatch(Tour t, TourDto searchParams)
        {
            return string.IsNullOrEmpty(searchParams.LocationDto.Country) || t.Location.Country.ToLower().Contains(searchParams.LocationDto.Country.ToLower());
        }

        public bool IsDurationMatch(Tour t, TourDto searchParams)
        {
            return searchParams.Duration == 0 || t.Duration == searchParams.Duration;
        }

        public bool IsLanguageMatch(Tour t, TourDto searchParams)
        {
            return string.IsNullOrEmpty(searchParams.Language) || t.Language.ToLower().Contains(searchParams.Language.ToLower());
        }

        public bool IsMaxTouristNumberMatch(Tour t, TourDto searchParams)
        {
            return searchParams.MaxTouristNumber == 0 || (t.MaxTouristsNumber >= searchParams.MaxTouristNumber && searchParams.MaxTouristNumber > 0);
        }
    }
}
