using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IGlobalLocationsRepository
    {
        public List<string> GetAllCities();
        public List<string> GetAllCountries();
        public List<string> GetCitiesFromCountry(string country);
        public string GetCountryForCity(string city);
    }
}
