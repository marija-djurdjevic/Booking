using BookingApp.Domain.Models;
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
        private PropertyRepository propertyRepository;
        public GlobalLocationsService(IGlobalLocationsRepository globalLocationsRepository) 
        {
            this.globalLocationsRepository = globalLocationsRepository;
            propertyRepository = new PropertyRepository();
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

        public List<(string, string)> GetAllLocations()
        {
            List<(string, string)> allCitiesAndCountries = new List<(string, string)>();

            List<string> allCities = GetAllCities();
            foreach (var city in allCities)
            {
                string country = GetCountryForCity(city);
                allCitiesAndCountries.Add((city, country));

            }

            return allCitiesAndCountries;
        }

        public List<string> GetPropertiesLocations()
        {
            List<string> Locations = new List<string>();
            List<(string, string)> allPropertiesCitiesAndCountries = new List<(string, string)>();
            List<Property> properties = new List<Property>();
            properties = propertyRepository.GetAllProperties();
            foreach (Property p in properties){
                allPropertiesCitiesAndCountries.Add((p.Location.City, p.Location.Country));
            }

            foreach ((string, string) loc in allPropertiesCitiesAndCountries)
            {
                Locations.Add($"{loc.Item1}, {loc.Item2}");
            }

            return Locations;
        }

        public List<string> GetRandomCitiesAndCountries()
        {
            List<string> randomLocations = new List<string>();
            Random random = new Random();

            List<(string, string)> allCitiesAndCountries = new List<(string, string)>();

            List<string> allCities = GetAllCities();
            foreach (var city in allCities)
            {
                string country = GetCountryForCity(city);
                allCitiesAndCountries.Add((city, country));
            }

            List<int> randomIndexes = new List<int>();
            while (randomIndexes.Count < 100)
            {
                int index = random.Next(0, allCitiesAndCountries.Count);
                if (!randomIndexes.Contains(index))
                {
                    randomIndexes.Add(index);
                }
            }

         
            foreach (int index in randomIndexes)
            {
                randomLocations.Add($"{allCitiesAndCountries[index].Item1}, {allCitiesAndCountries[index].Item2}");
            }

            return randomLocations;
        }






    }
}
