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
    public class TourDto : INotifyPropertyChanged
    {
        public int Id { get; set; }
        private string name;
        private string description;
        private string language;
        private int maxTouristsNumber;
        private DateTime startDateTime;
        private double duration;
        private LocationDto locationDto=new LocationDto();
        private List<string> imagesPaths=new List<string>();
        private List<KeyPoints> keyPoints = new List<KeyPoints>();



        public TourDto() { }

        public TourDto(string name, string description, string language, int maxTouristsNumber, DateTime startTime, double duration, LocationDto locationDto /*int keyPointId*/, List<string> imagesPaths)
        {
            this.name = name;
            this.description = description;
            this.language = language;
            this.maxTouristsNumber = maxTouristsNumber;
            this.startDateTime = startTime;
            this.duration = duration;
            this.locationDto = locationDto;
            this.imagesPaths = imagesPaths;

        }



        public TourDto(Tour tour)
        {
            Id = tour.Id;
            language = tour.Language;
            maxTouristsNumber = tour.MaxTouristsNumber;
            startDateTime = tour.StartDateTime;
            name = tour.Name;
            description = tour.Description;
            duration = tour.Duration;
            imagesPaths = tour.ImagesPaths;
            locationDto = new LocationDto(tour.Location);

        }

        public TourDto(TourDto tour)
        {
            Id = tour.Id;
            language = tour.language;
            maxTouristsNumber = tour.maxTouristsNumber;
            startDateTime = tour.startDateTime;
            name = tour.name;
            description = tour.description;
            duration = tour.duration;
            imagesPaths = tour.imagesPaths;
            locationDto = new LocationDto(tour.locationDto);

        }



        public List<KeyPoints> KeyPoints
        {
            get { return keyPoints; }
            set
            {
                if (value != keyPoints)
                {
                    keyPoints = value;
                    OnPropertyChanged();
                }
            }
        }




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



       
        public DateTime StartDateTime
        {
            get { return startDateTime; }
            set
            {
                if (value != startDateTime)
                {
                    startDateTime = value;
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
            Location location = LocationDto != null ? locationDto.ToLocation() : new Location();
            return new Tour( Name, Description, Language, MaxTouristNumber, StartDateTime, Duration, ImagesPaths, location);
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
