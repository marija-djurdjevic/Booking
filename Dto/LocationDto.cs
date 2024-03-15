using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.DTO
{
    public class LocationDto : INotifyPropertyChanged
    {
        private string city;
        private string country;
        public LocationDto() { }

        public LocationDto(string city, string country)
        {
            this.city = city;
            this.country = country;
        }

        public LocationDto(Location location)
        {
            city = location.City;
            country = location.Country;
        }

        public LocationDto(LocationDto location)
        {
            city = location.City;
            country = location.Country;
        }



        public string City
        {
            get { return city; }
            set
            {
                if (value != city)
                {
                    city = value;
                    OnPropertyChanged();
                }
            }
        }


        public string Country
        {
            get { return country; }
            set
            {
                if (value != country)
                {
                    country = value;
                    OnPropertyChanged();
                }
            }
        }

        public Location ToLocation()
        {
            return new Location(city, country);
        }

       
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
