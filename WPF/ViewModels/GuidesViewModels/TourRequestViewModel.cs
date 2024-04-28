using BookingApp.Aplication;
using BookingApp.Aplication.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.View;
using BookingApp.WPF.ViewModels.GuidesViewModel;
using BookingApp.WPF.Views.GuideView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BookingApp.WPF.ViewModels.GuidesViewModels
{
    public class TourRequestViewModel : BaseViewModel
    {
        private ObservableCollection<TourRequest> tourRequests;
        private TourRequestService tourRequestService;
        private RequestStatisticService requestStatisticService;
        private ObservableCollection<string> locations;
        private ObservableCollection<string> languages;
        private string language;
        private string location;
        private int touristsNumber;
        private RelayCommand searchCommand;
        private RelayCommand acceptCommand;
        private List<TourRequest> allRequests;
        private RelayCommand sideMenuCommand;
        public TourRequestViewModel()
        {
            tourRequestService = new   TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), Injector.CreateInstance<ITourRepository>());
            requestStatisticService = new RequestStatisticService(Injector.CreateInstance<ITourRequestRepository>(), Injector.CreateInstance<ITourRepository>());
            TourRequests = new ObservableCollection<TourRequest>(tourRequestService.GetAllRequests());
            allRequests = new List<TourRequest>(tourRequestService.GetAllRequests());
            Locations = new ObservableCollection<string>(requestStatisticService.GetLocations());
            Languages = new ObservableCollection<string>(requestStatisticService.GetLanguages());
            StartDateTime = null;
            EndDateTime=null;
            searchCommand = new RelayCommand(ExecuteSearchCommand);
            acceptCommand = new RelayCommand(ExecuteAcceptCommand);
            sideMenuCommand = new RelayCommand(ExecuteSideMenuClick);
        }

        public RelayCommand SideManuCommand
        {
            get { return sideMenuCommand; }
            set
            {
                if (sideMenuCommand != value)
                {
                    sideMenuCommand = value;
                    OnPropertyChanged();
                }
            }
        }


        private void ExecuteSideMenuClick()
        {

            var sideMenuPage = new SideMenuPage();
            GuideMainWindow.MainFrame.Navigate(sideMenuPage);

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

        public RelayCommand AcceptTourClickCommand
        {
            get { return acceptCommand; }
            set
            {
                if (acceptCommand != value)
                {
                    acceptCommand = value;
                    OnPropertyChanged();
                }
            }
        }




        
        private void ExecuteSearchCommand()
        {
            
            var filteredRequests = allRequests.Where(request =>
                (string.IsNullOrEmpty(Language) || request.Language == Language) &&
                (string.IsNullOrEmpty(Location) || request.Location.City == Location.Split(',')[0]) &&
                (TouristsNumber == 0 || request.TouristNumber == TouristsNumber) &&
                (!StartDateTime.HasValue || request.StartDate.Date >= StartDateTime.Value.Date) &&
                (!EndDateTime.HasValue || request.EndDate.Date <= EndDateTime.Value.Date));

            TourRequests = new ObservableCollection<TourRequest>(filteredRequests);
        }




        private void ExecuteAcceptCommand(object parameter)
        {
            int id = Convert.ToInt32(parameter);
            var acceptTour = new AcceptTourRequest(id);
            GuideMainWindow.MainFrame.Navigate(acceptTour);

        }



    }
}
