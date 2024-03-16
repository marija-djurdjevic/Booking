using BookingApp.DTO;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Dto
{
    public class PropertyDto: INotifyPropertyChanged
    {
        private string name;
        private int maxGuests;
        private int minReservationDays;
        private LocationDto locationDto = new LocationDto();
        private List<string> imagesPaths = new List<string>();
        private int cancellationDeadline;
        private PropertyType type;
        public List<ReservedDate> reservedDates;


        public PropertyDto() {
            CancellationDeadline = 1;
        }

        public PropertyDto(string name, LocationDto locationDto, PropertyType type, int maxGuests, int minReservationDays, int cancellationDeadline, List<string> imagesPaths)
        {
            this.name = name;
            this.type = type;
            this.maxGuests = maxGuests;
            this.minReservationDays = minReservationDays;
            this.cancellationDeadline = cancellationDeadline;
            this.imagesPaths = imagesPaths;
            this.locationDto = locationDto;
        }
       
        public PropertyDto(Property property)
        {
            name = property.Name;
            maxGuests = property.MaxGuests;
            minReservationDays = property.MinReservationDays;
            cancellationDeadline = property.CancellationDeadline;
            type = property.Type;
            imagesPaths = property.ImagesPaths;
            locationDto = new LocationDto(property.Location);

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

        public LocationDto LocationDto
        {
            get { return locationDto; }
            set
            {
                if (value != locationDto)
                {
                    locationDto = value;
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
            return new Property(Name, location, Type, MaxGuests, MinReservationDays, CancellationDeadline, ImagesPaths);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}

