using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Aplication.Dto
{
    public class PropertyDto : INotifyPropertyChanged
    {
        private string name;
        private int ownerId;
        private int maxGuests;
        private int minReservationDays;
        private string city;
        private string country;
        private LocationDto locationDto = new LocationDto();
        private List<string> imagesPaths = new List<string>();
        private int cancellationDeadline;
        private PropertyType type;
        public List<ReservedDate> reservedDates;


        public PropertyDto()
        {
            CancellationDeadline = 1;
        }

        public PropertyDto(int ownerId, string name, LocationDto locationDto, PropertyType type, int maxGuests, int minReservationDays, int cancellationDeadline, List<string> imagesPaths)
        {
            this.ownerId = ownerId;
            this.name = name;
            this.type = type;
            this.maxGuests = maxGuests;
            this.minReservationDays = minReservationDays;
            this.cancellationDeadline = cancellationDeadline;
            this.imagesPaths = imagesPaths;
            this.locationDto = locationDto;
            city = locationDto.City;
            country = locationDto.Country;
        }

        public PropertyDto(Property property)
        {
            ownerId = property.OwnerId;
            name = property.Name;
            maxGuests = property.MaxGuests;
            minReservationDays = property.MinReservationDays;
            cancellationDeadline = property.CancellationDeadline;
            type = property.Type;
            imagesPaths = property.ImagesPaths;
            locationDto = new LocationDto(property.Location);
            city = property.Location.City;
            country = property.Location.Country;

        }

        public int Id { get; set; }

        public List<string> ImagesPaths
        {
            get { return imagesPaths; }
            set
            {
                if (value != imagesPaths)
                {
                    imagesPaths = value;
                    OnPropertyChanged();
                }

            }
        }
        public int OwnerId
        {
            get { return ownerId; }
            set
            {
                if (value != ownerId)
                {
                    ownerId = value;
                    OnPropertyChanged();
                }
            }
        }

        public LocationDto LocationDto
        {
            get { return locationDto; }
            set
            {
                if (value != locationDto)
                {
                    locationDto = value;
                    city = locationDto.City;
                    country = locationDto.Country;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(City));
                    OnPropertyChanged(nameof(Country));
                }
            }
        }

        public string City
        {
            get { return city; }
            set
            {
                if (value != city)
                {
                    city = value;
                    locationDto.City = value;
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
                    locationDto.Country = value;
                    OnPropertyChanged();
                }
            }
        }


        public string Name
        {

            get { return name; }
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }
        public PropertyType Type
        {

            get { return type; }
            set
            {
                if (value != type)
                {
                    type = value;
                    OnPropertyChanged();
                }
            }
        }

        public int MaxGuests
        {
            get { return maxGuests; }
            set
            {
                if (value != maxGuests)
                {
                    maxGuests = value;
                    OnPropertyChanged();
                }
            }
        }
        public int MinReservationDays
        {
            get { return minReservationDays; }
            set
            {
                if (value != minReservationDays)
                {
                    minReservationDays = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<ReservedDate> ReservedDates
        {
            get { return reservedDates; }
            set
            {
                if (value != reservedDates)
                {
                    reservedDates = value;
                    OnPropertyChanged();
                }
            }
        }


        public int CancellationDeadline
        {
            get { return cancellationDeadline; }
            set
            {
                if (value != cancellationDeadline)
                {
                    cancellationDeadline = value;
                    OnPropertyChanged();
                }
            }
        }


        public Property ToProperty()
        {
            Location location = LocationDto != null ? locationDto.ToLocation() : new Location();
            return new Property(OwnerId, Name, location, Type, MaxGuests, MinReservationDays, CancellationDeadline, ImagesPaths);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}

