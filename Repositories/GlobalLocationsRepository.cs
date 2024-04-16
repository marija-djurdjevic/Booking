using BookingApp.Domain.Models;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repositories
{
    public class GlobalLocationsRepository
    {
        private const string FilePath = "../../../Resources/Data/globalLocations.csv";

        private List<string> cities;

        private List<string> countries;

        private Dictionary<string, List<string>> countriesAndCities;

        public GlobalLocationsRepository()
        {
            cities = new List<string>();
            countries = new List<string>();
            countriesAndCities = new Dictionary<string, List<string>>();
            using (StreamReader sr = new StreamReader(FilePath))
            {
                while (!sr.EndOfStream)
                {
                    string[] tokens = sr.ReadLine().Split(',');
                    string country = tokens[1];
                    string city = tokens[0];

                    cities.Add(city);

                    if (!countriesAndCities.ContainsKey(country))
                    {
                        countriesAndCities.Add(country, new List<string>());
                        countries.Add(country);
                    }

                    countriesAndCities[country].Add(city);
                }
            }
        }

        public List<string> GetAllCities()
        {
            return cities;
        }
        public List<string> GetAllCountries()
        {
            return countries;
        }

        public List<string> GetCitiesFromCountry(string country)
        {
            List<string> cities = new List<string>();
            if (countries.Contains(country))
            {
                foreach (string city in countriesAndCities[country])
                {
                    cities.Add(city);
                }
            }
            return cities;
        }

        public string GetCountryForCity(string city)
        {
            return countriesAndCities.FirstOrDefault(x => x.Value.Contains(city)).Key;
        }

    }
}
