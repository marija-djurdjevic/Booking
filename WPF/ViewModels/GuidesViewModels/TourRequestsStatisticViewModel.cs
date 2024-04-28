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
        private string location;
        private int requestsNumber;
        //private int selectedYear;
        private RelayCommand searchCommand;
        private RelayCommand generalStatisticsCommand;
        private RelayCommand monthStatisticsCommand;
        private RelayCommand sideMenuCommand;

        public TourRequestsStatisticViewModel()
        {
            tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(),Injector.CreateInstance<ITourRepository>());
            requestStatisticService= new RequestStatisticService(Injector.CreateInstance<ITourRequestRepository>(), Injector.CreateInstance<ITourRepository>());
            TourRequests = new ObservableCollection<TourRequest>(tourRequestService.GetAllRequests());
            Locations = new ObservableCollection<string>(requestStatisticService.GetLocations());
            Languages = new ObservableCollection<string>(requestStatisticService.GetLanguages());
            IsYear = false;
            IsGeneral = false;
            searchCommand = new RelayCommand(ExecuteSearchCommand);
            generalStatisticsCommand = new RelayCommand(ExecuteGeneralStatisticsCommand);
            monthStatisticsCommand = new RelayCommand(ExecuteMonthStatisticsCommand);
            sideMenuCommand = new RelayCommand(ExecuteSideMenuClick);
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

        private ComboBoxItem selectedYear;


        public ComboBoxItem SelectedYear
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
            if (parameter is ComboBoxItem selectedYearItem)
            {
                string year = selectedYearItem.Content.ToString();
                var montStat = new MonthStatistics(year, Language, Location);
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

            if (SelectedYear.Content.ToString() != "General")
            {
                
                filteredRequests = filteredRequests.Where(request =>
                    request.StartDate.Year.ToString() == SelectedYear.Content.ToString());
            }

            
            RequestsNumber = filteredRequests.Count();

           
            IsYear = SelectedYear.Content.ToString() != "General";
            IsGeneral = SelectedYear.Content.ToString() == "General";
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
    }
}
