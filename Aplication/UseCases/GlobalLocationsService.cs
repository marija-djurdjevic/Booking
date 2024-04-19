using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Aplication.UseCases
{
    public class GlobalLocationsService
    {
        private IGlobalLocationsRepository globalLocationsRepository;
        public GlobalLocationsService(IGlobalLocationsRepository globalLocationsRepository) 
        {
            this.globalLocationsRepository = globalLocationsRepository;
        }
        public List<string> GetAllCities()
        {
            return globalLocationsRepository.GetAllCities();
        }

        public List<string> GetAllCountries()
        {
            return globalLocationsRepository.GetAllCountries();
        }

        public List<string> GetCitiesFromCountry(string country)
        {
            return globalLocationsRepository.GetCitiesFromCountry(country);
        }

        public string GetCountryForCity(string city)
        {
            return globalLocationsRepository.GetCountryForCity(city);
        }
    }
}
