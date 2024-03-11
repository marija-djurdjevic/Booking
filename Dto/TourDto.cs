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
        public int Id { get; set; }
        private string name;
        private string description;
        private string language;
        private int maxTouristsNumber;
        private DateTime startDate;
        private double duration;
        private LocationDto locationDto=new LocationDto();
        private List<string> imagesPaths=new List<string>();
        private string startKeyPoint;
        private List<string> middleKeyPoints=new List<string>();
        private string endKeyPoint;


        public string StartKeyPoint
        {
            get { return startKeyPoint; }
            set
            {
                if (value != startKeyPoint)
                {
                    startKeyPoint = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<string> MiddleKeyPoints
        {
            get { return middleKeyPoints; }
            set
            {
                if (value != middleKeyPoints)
                {
                    middleKeyPoints = value;
                    OnPropertyChanged();
                }
            }
        }

        public string EndKeyPoint
        {
            get { return endKeyPoint; }
            set
            {
                if (value != endKeyPoint)
                {
                    endKeyPoint = value;
                    OnPropertyChanged();
                }
            }
        }


        public TourDto() { }

        public TourDto(string name, string description, string language, int maxTouristsNumber, DateTime tourStartDates, double duration, LocationDto locationDto /*int keyPointId*/, List<string> imagesPaths)
        {
            this.name = name;
            this.description = description;
            this.language = language;
            this.maxTouristsNumber = maxTouristsNumber;
            this.startDate = tourStartDates;
            this.duration = duration;
            this.locationDto = locationDto;
            this.imagesPaths = imagesPaths;

        }


    
        public TourDto(Tour tour)
        {
            Id = tour.Id;
            language = tour.Language;
            maxTouristsNumber = tour.MaxTouristsNumber;
            startDate = tour.StartDate;
            name = tour.Name;
            description = tour.Description;
            duration = tour.Duration;
            imagesPaths = tour.ImagesPaths;
            locationDto = new LocationDto(tour.Location);
           
        }

        public TourDto(TourDto tour)
        {
            Id= tour.Id;
            language = tour.language;
            maxTouristsNumber = tour.maxTouristsNumber;
            startDate = tour.startDate;
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


        
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                if (value != startDate)
                {
                    startDate = value;
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


       /* public int KeyPointId
        {
            get
            {
                return keyPointId;
            }
            set
            {
                if (value != keyPointId)
                    keyPointId = value;
                OnPropertyChanged();
            }
        }*/
        public Tour ToTour()
        {
            Location location = LocationDto != null ? locationDto.ToLocation() : new Location();
            return new Tour(Id,Name, Description, Language, MaxTouristNumber, StartDate, Duration, ImagesPaths, location);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
