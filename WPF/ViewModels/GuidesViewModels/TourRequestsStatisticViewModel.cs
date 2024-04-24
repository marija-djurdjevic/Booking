using BookingApp.Aplication.UseCases;
using BookingApp.Aplication;
using BookingApp.Command;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.ViewModels.GuidesViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.GuidesViewModels
{
    public class TourRequestsStatisticViewModel : BaseViewModel
    {
        private ObservableCollection<TourRequest> tourRequests;
        private TourRequestService tourRequestService;
        private ObservableCollection<string> locations;
        private ObservableCollection<string> languages;
        private string language;
        private string location;
        private int requestsNumber;
        private int selectedYear;
        private RelayCommand searchCommand;

        public TourRequestsStatisticViewModel()
        {
            tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>());
            TourRequests = new ObservableCollection<TourRequest>(tourRequestService.GetAllRequests());
            Locations = new ObservableCollection<string>(tourRequestService.GetLocations());
            Languages = new ObservableCollection<string>(tourRequestService.GetLanguages());

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

        public int RequestsNumber
        {
            get { return requestsNumber; }
            set { requestsNumber = value; OnPropertyChanged(); }
        }

        public int SelectedYear
        {
            get { return selectedYear; }
            set { selectedYear = value; OnPropertyChanged(); }
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
                (SelectedYear.ToString() == "General" || request.StartDate.Year.ToString() == SelectedYear.ToString() ||
                    (SelectedYear.ToString() == DateTime.Now.Year.ToString() &&
                    request.StartDate.Month <= DateTime.Now.Month)));

            if (SelectedYear.ToString() != "General" )
            {
                filteredRequests = filteredRequests.Where(request =>
                    request.StartDate.Year == SelectedYear);
            }

            RequestsNumber = filteredRequests.Count();
           
        }


    }
}
