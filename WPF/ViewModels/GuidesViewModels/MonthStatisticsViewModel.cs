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
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.GuidesViewModels
{
    public class MonthStatisticsViewModel : BaseViewModel
    {
        public string SelectedYear { get; }

        private List<TourRequest> tourRequests;
        private TourRequestService tourRequestService;
        private RelayCommand sideMenuCommand;
        public Dictionary<string, int> RequestsPerMonth { get; }
        public string Language { get; }
        public string Location { get; }

        public MonthStatisticsViewModel(string selectedYear,string language, string location)
        {
            sideMenuCommand = new RelayCommand(ExecuteSideMenuClick);
            tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), Injector.CreateInstance<ITourRepository>());
            tourRequests = new List<TourRequest>(tourRequestService.GetAllSimpleRequests());
            SelectedYear = selectedYear;
            RequestsPerMonth = new Dictionary<string, int>();
            Location = location;
            Language = language;
            for (int i = 1; i <= 12; i++)
            {
                RequestsPerMonth.Add($"{i}", 0);
            }

            UpdateRequestsForYear();
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
                    (string.IsNullOrEmpty(Location) || request.Location.City == Location.Split(',')[0]) &&
                    (request.StartDate.Year.ToString() == SelectedYear))
                {
                    var month = request.StartDate.Month.ToString();
                    if (RequestsPerMonth.ContainsKey(month))
                    {
                        RequestsPerMonth[month]++;
                    }
                }
            }


            foreach (var month in RequestsPerMonth.Keys)
            {
                OnPropertyChanged(month);
            }
        }
    }

}

