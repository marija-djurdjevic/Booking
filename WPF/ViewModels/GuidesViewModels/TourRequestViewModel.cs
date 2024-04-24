using BookingApp.Aplication;
using BookingApp.Aplication.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.ViewModels.GuidesViewModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace BookingApp.WPF.ViewModels.GuidesViewModels
{
    public class TourRequestViewModel : BaseViewModel
    {
        private ObservableCollection<TourRequest> tourRequests;
        private TourRequestService tourRequestService;
        private ObservableCollection<string> locations;
        private ObservableCollection<string> languages;
        
        private string language;
        private string location;
        private int touristsNumber;
        private RelayCommand searchCommand;

        public TourRequestViewModel()
        {
            tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>());
            TourRequests = new ObservableCollection<TourRequest>(tourRequestService.GetAllRequests());
            Locations = new ObservableCollection<string>(tourRequestService.GetLocations());
            Languages = new ObservableCollection<string>(tourRequestService.GetLanguages());
            StartDateTime = null;
            EndDateTime=null;
            searchCommand = new RelayCommand(ExecuteSearchCommand);
        }

        public ObservableCollection<TourRequest> TourRequests
        {
            get { return tourRequests; }
            set { tourRequests = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> Locations
        {
            get { return locations; }
            set { locations = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> Languages
        {
            get { return languages; }
            set { languages = value; OnPropertyChanged(); }
        }

       
        public string Language
        {
            get { return language; }
            set { language = value; OnPropertyChanged(); }
        }


        public string Location
        {
            get { return location; }
            set { location = value; OnPropertyChanged(); }
        }


        public int TouristsNumber
        {
            get { return touristsNumber; }
            set { touristsNumber = value; OnPropertyChanged(); }
        }

        private DateTime? endDateTime;
        public DateTime? EndDateTime
        {
            get { return endDateTime; }
            set { endDateTime = value; OnPropertyChanged(); }
        }


        private DateTime? startDateTime;
        public DateTime? StartDateTime
        {
            get { return startDateTime; }
            set { startDateTime = value; OnPropertyChanged(); }
        }


        public RelayCommand SearchCommand
        {
            get { return searchCommand; }
            set
            {
                if (searchCommand != value)
                {
                    searchCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private void ExecuteSearchCommand()
        {
            
            var filteredRequests = tourRequests.Where(request =>
                (string.IsNullOrEmpty(Language) || request.Language == Language) &&
                (string.IsNullOrEmpty(Location) || request.Location.City == Location.Split(',')[0]) &&
                (TouristsNumber == 0 || request.TouristNumber == TouristsNumber) &&
                (!StartDateTime.HasValue || request.StartDate >= StartDateTime) &&
                (!EndDateTime.HasValue || request.EndDate <= EndDateTime));

            TourRequests = new ObservableCollection<TourRequest>(filteredRequests);
        }




    }
}
