using BookingApp.Aplication;
using BookingApp.Aplication.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
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
        public Dictionary<string, int> RequestsPerMonth { get; }
        public string Language { get; }
        public string Location { get; }

        public MonthStatisticsViewModel(string selectedYear,string language, string location)
        {
            tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), Injector.CreateInstance<ITourRepository>());
            tourRequests = new List<TourRequest>(tourRequestService.GetAllRequests());
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

