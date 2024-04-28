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

namespace BookingApp.WPF.ViewModels.GuidesViewModels
{
    public class GeneralStatisticsViewModel : BaseViewModel
    {
       
        private List<TourRequest> tourRequests;
        private TourRequestService tourRequestService;
        private RelayCommand sideMenuCommand;
        public Dictionary<string, int> RequestsPerYear { get; }
        public string Language { get; }
        public string Location { get; }
        public GeneralStatisticsViewModel(string language,string location)
        {
            tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), Injector.CreateInstance<ITourRepository>());
            tourRequests = new List<TourRequest>(tourRequestService.GetAllRequests());
            Location = location;
            Language = language;
            RequestsPerYear = new Dictionary<string, int>();

            // Dodajemo sve godine koje su u opticaju
            for (int i = 2020; i <= 2024; i++)
            {
                RequestsPerYear.Add($"{i}", 0);
            }

            UpdateRequestsForYear();
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

            // Obavještavamo o promjeni za sve godine
            foreach (var year in RequestsPerYear.Keys)
            {
                OnPropertyChanged(year);
            }
        }

    }
}
