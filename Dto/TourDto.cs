using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class TourDto: INotifyPropertyChanged
    {
        private string name;
        private string description;
        private string language;
        private int maxTouristsNumber;
        private List<DateTime> tourStartDates;
        private double duration;
        private List<string> imagesPaths;
        private LocationDto locationDto;
        public TourDto() { }

        public TourDto(string name, string description, string language, int maxTouristsNumber, List<DateTime> tourStartDates, double duration, List<string> imagesPaths, LocationDto locationDto)
        {
            this.name = name;
            this.description = description;
            this.language = language;
            this.maxTouristsNumber = maxTouristsNumber;
            this.tourStartDates = tourStartDates;
            this.duration = duration;
            this.imagesPaths = imagesPaths;
            this.locationDto = locationDto;
        }


    
        public TourDto(Tour tour)
        {
            
            language = tour.Language;
            maxTouristsNumber = tour.MaxTouristsNumber;
            tourStartDates = tour.TourStartDates;
            name = tour.Name;
            description = tour.Description;
            duration = tour.Duration;
            imagesPaths = tour.ImagesPaths;
            locationDto = new LocationDto(tour.Location);
        }

        public TourDto(TourDto tour)
        {

            language = tour.language;
            maxTouristsNumber = tour.maxTouristsNumber;
            tourStartDates = tour.tourStartDates;
            name = tour.name;
            description = tour.description;
            duration = tour.duration;
            imagesPaths = tour.imagesPaths;
            locationDto = new LocationDto(tour.locationDto);

        }

        public List<string> ImagesPaths
        {
            get { return imagesPaths; }
            set
            {
                if(value!=imagesPaths)
                {
                    imagesPaths = value;
                    OnPropertyChanged();
                }

            }
        }

        public LocationDto LocationDTO
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
                if(value != name)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                if (value != description)
                {
                    description = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Language
        {
            get { return language; }
            set
            {
                if (value != language)
                {
                    language = value;
                    OnPropertyChanged();
                }
            }
        }

        
        public int MaxTouristNumber
        {
            get { return maxTouristsNumber; }
            set
            {
                if (value != maxTouristsNumber)
                {
                    maxTouristsNumber = value;
                    OnPropertyChanged();
                }
            }
        }


        
        public List<DateTime> TourStartDates
        {
            get { return tourStartDates; }
            set
            {
                if (value != tourStartDates)
                {
                    tourStartDates = value;
                    OnPropertyChanged();
                }
            }
        }

       
        public double Duration
        {
            get { return duration; }
            set
            {
                if (value != duration)
                {
                    duration = value;
                    OnPropertyChanged();
                }
            }
        }



        public Tour ToTour()
        {
            Location location = LocationDTO != null ? LocationDTO.ToLocation() : new Location();
            return new Tour(Name, Description, Language, MaxTouristNumber, TourStartDates, Duration, ImagesPaths, location);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
