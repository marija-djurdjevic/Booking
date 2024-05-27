using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Aplication.Dto
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
        private LocationDto locationDto = new LocationDto();
        private List<string> imagesPaths = new List<string>();
        private List<KeyPoint> keyPoints = new List<KeyPoint>();

        public TourDto()
        {
            locationDto = new LocationDto();
            imagesPaths = new List<string>();
            keyPoints = new List<KeyPoint>();
        }

        public TourDto(string name, string description, string language, int maxTouristsNumber, DateTime startTime, double duration, LocationDto locationDto, List<string> imagesPaths)
        {
            this.name = name;
            this.description = description;
            this.language = language;
            this.maxTouristsNumber = maxTouristsNumber;
            startDateTime = startTime;
            this.duration = duration;
            this.locationDto = locationDto;
            this.imagesPaths = imagesPaths;
            keyPoints = new List<KeyPoint>();
            //GuideId = guideId; // Dodajemo GuideId u konstruktoru
        }

        public TourDto(Tour tour/*, int guideId*/)
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
            keyPoints = tour.KeyPoints;
            GuideId= tour.GuideId; 
           // GuideId = guideId; // Dodajemo GuideId prilikom kreiranja TourDto iz Tour objekta
        }


        private int guideId;

        public int GuideId
        {
            get { return guideId; }
            set
            {
                if (value != guideId)
                {
                    guideId = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<KeyPoint> KeyPoints
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
            return new Tour(Id, Name, Description, Language, MaxTouristNumber, StartDateTime, Duration, ImagesPaths, location, GuideId); // Dodajte GuideId prilikom konverzije u Tour
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
