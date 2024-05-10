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
using System.Windows.Controls;
using System.Windows.Navigation;
using BookingApp.WPF.Views.GuideView;
using BookingApp.GuestView;
using BookingApp.View;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;

namespace BookingApp.WPF.ViewModels.GuidesViewModels
{
    public class TourRequestsStatisticViewModel : BaseViewModel
    {
        private ObservableCollection<TourRequest> tourRequests;
        private TourRequestService tourRequestService;
        private RequestStatisticService requestStatisticService;
        private ObservableCollection<string> locations;
        private ObservableCollection<string> languages;
        private string language;
        private ObservableCollection<string> years;
        private string location;
        private int requestsNumber;
        //private int selectedYear;
        private RelayCommand searchCommand;
        private RelayCommand generalStatisticsCommand;
        private RelayCommand monthStatisticsCommand;
        private RelayCommand sideMenuCommand;
        private RelayCommand resetSearchCommand;

        public TourRequestsStatisticViewModel()
        {
            tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(),Injector.CreateInstance<ITourRepository>());
            requestStatisticService= new RequestStatisticService(Injector.CreateInstance<ITourRequestRepository>(), Injector.CreateInstance<ITourRepository>());
            TourRequests = new ObservableCollection<TourRequest>(tourRequestService.GetAllSimpleRequests());
            Locations = new ObservableCollection<string>(requestStatisticService.GetLocations());
            Languages = new ObservableCollection<string>(requestStatisticService.GetLanguages());
            Years = new ObservableCollection<string>(requestStatisticService.GetYears().OrderByDescending(year => year));
            Years.Insert(0, "General");
            IsYear = false;
            IsGeneral = false;
            searchCommand = new RelayCommand(ExecuteSearchCommand);
            generalStatisticsCommand = new RelayCommand(ExecuteGeneralStatisticsCommand);
            monthStatisticsCommand = new RelayCommand(ExecuteMonthStatisticsCommand);
            sideMenuCommand = new RelayCommand(ExecuteSideMenuClick);
            resetSearchCommand = new RelayCommand(ExecuteResetSearchCommand);
        }

        public ObservableCollection<TourRequest> TourRequests
        {
            get { return tourRequests; }
            set { tourRequests = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> Years
        {
            get { return years; }
            set { years = value; OnPropertyChanged(); }
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

        private string selectedYear;


        public string SelectedYear
        {
            get { return selectedYear; }
            set
            {
                selectedYear = value;

                OnPropertyChanged();
            }
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


        public RelayCommand ResetSearchCommand
        {
            get { return resetSearchCommand; }
            set
            {
                if (resetSearchCommand != value)
                {
                    resetSearchCommand = value;
                    OnPropertyChanged();
                }
            }
        }


        public RelayCommand GeneralStatisticsCommand
        {
            get { return generalStatisticsCommand; }
            set
            {
                if (generalStatisticsCommand != value)
                {
                    generalStatisticsCommand = value;
                    OnPropertyChanged();
                }
            }
        }



        public RelayCommand MonthStatisticsCommand
        {
            get { return monthStatisticsCommand; }
            set
            {
                if (monthStatisticsCommand != value)
                {
                    monthStatisticsCommand = value;
                    OnPropertyChanged();
                }
            }
        }


        private void ExecuteGeneralStatisticsCommand()
        {

            var generalStat = new GeneralStatistics(Language, Location);
            GuideMainWindow.MainFrame.Navigate(generalStat);


        }



        private void ExecuteMonthStatisticsCommand(object parameter)
        {
            if (parameter is string selectedYearItem)
            {
                
                var montStat = new MonthStatistics(selectedYearItem, Language, Location);
                GuideMainWindow.MainFrame.Navigate(montStat);

            }
        }



        private void ExecuteSearchCommand()
        {
            if (SelectedYear == null)
            {
                MessageBox.Show("nisi izabrao period");
                return;
            }

            var filteredRequests = tourRequests.Where(request =>
                (string.IsNullOrEmpty(Language) || request.Language == Language) &&
                (string.IsNullOrEmpty(Location) || request.Location.City == Location.Split(',')[0]));

            if (SelectedYear != "General")
            {

                filteredRequests = filteredRequests.Where(request =>
                    request.StartDate.Year.ToString() == SelectedYear);
            }


            RequestsNumber = filteredRequests.Count();
            AllRequestsNumber = tourRequests.Count();


            IsYear = SelectedYear != "General";
            IsGeneral = SelectedYear == "General";
            UpdateChart();
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


        private void ExecuteResetSearchCommand()
        {
            Language = null;
            Location = null;
            RequestsNumber = 0;
            AllRequestsNumber = 0;
            SelectedYear = null;
            IsYear = false;
            IsGeneral = false;
            TourRequests = new ObservableCollection<TourRequest>(tourRequestService.GetAllSimpleRequests());
            UpdateChart();
        }





        private bool isYear;

        public bool IsYear
        {
            get { return isYear; }
            set { isYear = value; OnPropertyChanged(); }
        }


        private bool isGeneral;


        public bool IsGeneral
        {
            get { return isGeneral; }
            set { isGeneral = value; OnPropertyChanged(); }
        }


        private int allRequestsNumber;
        public int AllRequestsNumber
        {
            get { return allRequestsNumber; }
            set { allRequestsNumber = value; OnPropertyChanged(); }
        }
        private SeriesCollection seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get { return seriesCollection; }
            set { seriesCollection = value; OnPropertyChanged(nameof(SeriesCollection)); }
        }

        private void UpdateChart()
        {
               double totalRequests = AllRequestsNumber;
               double searchedRequests = RequestsNumber;

               SeriesCollection = new SeriesCollection
          {
             new PieSeries
             {
               Title = "Total Requests",
               Values = new ChartValues<double> { totalRequests },
               DataLabels = true,
               LabelPoint = chartPoint => string.Format("{0:P}", chartPoint.Participation),
               FontSize=24
             },
             new PieSeries
             {
               Title = "Searched Requests",
               Values = new ChartValues<double> { searchedRequests },
               DataLabels = true,
               LabelPoint = chartPoint => string.Format("{0:P}", chartPoint.Participation),
               FontSize=24
             }
          };

        }

    }
}
