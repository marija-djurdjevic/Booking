using BookingApp.Aplication.UseCases;
using BookingApp.Aplication;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.ViewModels.GuidesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Command;
using BookingApp.View;
using System.Collections.ObjectModel;

namespace BookingApp.WPF.ViewModels.GuidesViewModels
{
    public class GeneralStatisticsViewModel : BaseViewModel
    {
        
        private List<TourRequest> tourRequests;
        private TourRequestService tourRequestService;
        private RequestStatisticService requestStatisticService;
        private RelayCommand sideMenuCommand;
        private ObservableCollection<string> years;
        public Dictionary<string, int> RequestsPerYear { get; }
        public string Language { get; }
        public string Location { get; }
        public User LoggedInUser { get; set; }
        public GeneralStatisticsViewModel(string language,string location, User loggedInUser)
        {
            requestStatisticService = new RequestStatisticService(Injector.CreateInstance<ITourRequestRepository>(), Injector.CreateInstance<ITourRepository>());
            tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), Injector.CreateInstance<ITourRepository>());
            tourRequests = new List<TourRequest>(tourRequestService.GetAllSimpleRequests());
            Location = location;
            Language = language;
            Years = new ObservableCollection<string>(requestStatisticService.GetYears().OrderByDescending(year => year));
            RequestsPerYear = new Dictionary<string, int>();


            foreach (var year in Years)
            {
                RequestsPerYear.Add(year, 0);
            }


            UpdateRequestsForYear();
            sideMenuCommand = new RelayCommand(ExecuteSideMenuClick);
            LoggedInUser = loggedInUser;
        }

        public ObservableCollection<string> Years
        {
            get { return years; }
            set { years = value; OnPropertyChanged(); }
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

            var sideMenuPage = new SideMenuPage(LoggedInUser);
            GuideMainWindow.MainFrame.Navigate(sideMenuPage);

        }


        private void UpdateRequestsForYear()
        {
            foreach (var request in tourRequests)
            {
                if ((string.IsNullOrEmpty(Language) || request.Language == Language) &&
                    (string.IsNullOrEmpty(Location) || request.Location.City == Location.Split(',')[0]))
                {
                    var year = request.StartDate.Year.ToString();
                    if (RequestsPerYear.ContainsKey(year))
                    {
                        RequestsPerYear[year]++;
                    }
                }
            }

           
            foreach (var year in RequestsPerYear.Keys)
            {
                OnPropertyChanged(year);
            }
        }

    }
}
