using BookingApp.Aplication;
using BookingApp.Aplication.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.View.TouristView;
using BookingApp.WPF.Views.TouristView;
using GalaSoft.MvvmLight.Messaging;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BookingApp.WPF.ViewModels.TouristViewModels
{
    public class TourRequestsViewModel : BindableBase
    {
        private readonly TourRequestStatisticsService tourRequestService;

        private ObservableCollection<Tuple<TourRequestViewModel, string>> tourRequests;
        public ObservableCollection<Tuple<TourRequestViewModel, string>> TourRequests
        {
            get { return tourRequests; }
            set
            {
                tourRequests = value;
                OnPropertyChanged(nameof(TourRequests));
            }
        }
        private string requestsSelectedSort;
        public string RequestsSelectedSort
        {
            get { return requestsSelectedSort; }
            set
            {
                requestsSelectedSort = value;
                OnPropertyChanged(nameof(RequestsSelectedSort));
            }
        }
        private string selectedYear;
        public string SelectedYear
        {
            get { return selectedYear; }
            set
            {
                selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
            }
        }
        private ObservableCollection<string> years;
        public ObservableCollection<string> Years
        {
            get { return years; }
            set
            {
                years = value;
                OnPropertyChanged(nameof(Years));
            }
        }
        private int unreadNotificationCount;
        public int UnreadNotificationCount
        {
            get { return unreadNotificationCount; }
            set
            {
                unreadNotificationCount = value;
                OnPropertyChanged(nameof(UnreadNotificationCount));
            }
        }
        private double averagePeopleNumber;
        public double AveragePeopleNumber
        {
            get { return averagePeopleNumber; }
            set
            {
                averagePeopleNumber = value;
                OnPropertyChanged(nameof(AveragePeopleNumber));
            }
        }
        private SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get { return _seriesCollection; }
            set
            {
                _seriesCollection = value;
                OnPropertyChanged("SeriesCollection");
            }
        }

        private SeriesCollection seriesCollectionLanguageChart;
        private string[] languagesLabels;
        private SeriesCollection seriesCollectionLocationChart;
        private string[] locationLabels;
        private Func<double, string> languageFormatter;
        public SeriesCollection SeriesCollectionLanguageChart
        {
            get => seriesCollectionLanguageChart;
            set
            {
                if (value != seriesCollectionLanguageChart)
                {
                    seriesCollectionLanguageChart = value;
                    OnPropertyChanged(nameof(SeriesCollectionLanguageChart));
                }
            }
        }
        public string[] LanguagesLabels
        {
            get => languagesLabels;
            set
            {
                if (value != languagesLabels)
                {
                    languagesLabels = value;
                    OnPropertyChanged(nameof(LanguagesLabels));
                }
            }
        }
        public SeriesCollection SeriesCollectionLocationChart
        {
            get => seriesCollectionLocationChart;
            set
            {
                if (value != seriesCollectionLocationChart)
                {
                    seriesCollectionLocationChart = value;
                    OnPropertyChanged(nameof(SeriesCollectionLocationChart));
                }
            }
        }
        public string[] LocationLabels
        {
            get => locationLabels;
            set
            {
                if (value != locationLabels)
                {
                    locationLabels = value;
                    OnPropertyChanged(nameof(LocationLabels));
                }
            }
        }
        public Func<double, string> LanguageFormatter
        {
            get => languageFormatter;
            set
            {
                if (value != languageFormatter)
                {
                    languageFormatter = value;
                    OnPropertyChanged(nameof(LanguageFormatter));
                }
            }
        }
        public TouristPDFExportService touristPDFExportService;
        public User LoggedInUser { get; set; }
        private readonly TouristGuideNotificationService notificationService;
        public RelayCommand InboxCommand { get; set; }
        public RelayCommand HelpCommand { get; set; }
        public RelayCommand SortingRequeststCommand { get; set; }
        public RelayCommand SelectedStatisticYearCommand { get; set; }
        public RelayCommand CreateCommand { get; set; }
        public RelayCommand ExportCommand { get; set; }
        public RelayCommand ScrollToTopCommand { get; private set; }
        public RelayCommand ScrollToBottomCommand { get; private set; }
        public RelayCommand ScrollDownCommand { get; private set; }
        public RelayCommand ScrollUpCommand { get; private set; }
        public RelayCommand ChangeTabRightCommand { get; private set; }
        public RelayCommand ChangeTabLeftCommand { get; private set; }

        public TourRequestsViewModel(User loggedInUser)
        {
            LoggedInUser = loggedInUser;
            TourRequests = new ObservableCollection<Tuple<TourRequestViewModel, string>>();
            Years = new ObservableCollection<string>();
            tourRequestService = new TourRequestStatisticsService(Injector.CreateInstance<ITourRequestRepository>(), Injector.CreateInstance<ITourRepository>());
            notificationService = new TouristGuideNotificationService(Injector.CreateInstance<ITouristGuideNotificationRepository>());

            GetMyRequests();
            UnreadNotificationCount = notificationService.GetUnreadNotificationCount(LoggedInUser.Id);

            InboxCommand = new RelayCommand(OpenInbox);
            CreateCommand = new RelayCommand(CreateTourRequest);
            HelpCommand = new RelayCommand(Help);
            ExportCommand = new RelayCommand(Export);
            SortingRequeststCommand = new RelayCommand(SortingSelectionChanged);
            SelectedStatisticYearCommand = new RelayCommand(SelectedStatisticYear);
            ScrollToTopCommand = new RelayCommand(ScrollToTop);
            ScrollToBottomCommand = new RelayCommand(ScrollToBottom);
            ScrollDownCommand = new RelayCommand(ScrollDown);
            ScrollUpCommand = new RelayCommand(ScrollUp);
            ChangeTabRightCommand = new RelayCommand(ChangeTabRight);
            ChangeTabLeftCommand = new RelayCommand(ChangeTabLeft);

            FetchStatistics();
        }
        private void ChangeTabLeft()
        {
            Messenger.Default.Send(new NotificationMessage("ChangeTabLeftRequests"));
        }

        private void ChangeTabRight()
        {
            Messenger.Default.Send(new NotificationMessage("ChangeTabRightRequests"));
        }

        private void ScrollUp()
        {
            Messenger.Default.Send(new NotificationMessage("ScrollRequestsUp"));
        }

        private void ScrollDown()
        {
            Messenger.Default.Send(new NotificationMessage("ScrollRequestsDown"));
        }

        private void ScrollToBottom()
        {
            Messenger.Default.Send(new NotificationMessage("ScrollRequestsToBottom"));
        }

        private void ScrollToTop()
        {
            Messenger.Default.Send(new NotificationMessage("ScrollRequestsToTop"));
        }

        private void Export()
        {
            Messenger.Default.Send(new NotificationMessage("SaveCharts"));
            touristPDFExportService = new TouristPDFExportService(LoggedInUser.Id, SelectedYear, AveragePeopleNumber);
        }

        private void FillRequestYears()
        {
            Years.Clear();
            Years.Add("All years");
            foreach (var year in tourRequestService.GetRequestsYears(LoggedInUser.Id).OrderBy(t => t))
            {
                Years.Add(year);
            }
            SelectedYear = "All years";
        }

        private void FetchStatistics()
        {
            FillRequestYears();
            FillLanguageChart();
            FillLocationChart();
            SelectedStatisticYear();
            Messenger.Default.Send(new NotificationMessage("SaveCharts"));
        }

        private void FillLanguageChart()
        {
            Dictionary<string, int> languageCounts = tourRequestService.GetLanguageRequestCounts(LoggedInUser.Id);

            SeriesCollectionLanguageChart = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Requests",
                    Values = new ChartValues<int>(languageCounts.Values)
                }
            };
            LanguagesLabels = languageCounts.Keys.ToArray();
            LanguageFormatter = value => value.ToString("N");
        }

        private void FillLocationChart()
        {
            Dictionary<string, int> locationsCounts = tourRequestService.GetLocationsRequestCounts(LoggedInUser.Id);

            SeriesCollectionLocationChart = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Requests",
                    Values = new ChartValues<int>(locationsCounts.Values)
                }
            };
            LocationLabels = locationsCounts.Keys.ToArray();
            LanguageFormatter = value => value.ToString("N");
        }

        private void SelectedStatisticYear()
        {
            (var accepted, var notAccepted, AveragePeopleNumber) = tourRequestService.GetStatisticsForYear(SelectedYear, LoggedInUser.Id);
            SeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = $"Accepted requests: {((float)accepted/(accepted+notAccepted))*100:N2}%",
                    Values = new ChartValues<double> {accepted},
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = $"Not accepted requests: {((float)notAccepted/(accepted+notAccepted))*100:N2}% ",
                    Values = new ChartValues<double> {notAccepted},
                    DataLabels = true
                }
            };
            Messenger.Default.Send(new NotificationMessage("SaveCharts"));
        }

        private void Help()
        {

        }
        private void GetMyRequests()
        {
            TourRequests.Clear();
            int i = 0;
            string title = "Request ";
            foreach (var request in tourRequestService.GetByTouristId(LoggedInUser.Id))
            {
                TourRequests.Add(new Tuple<TourRequestViewModel, string>(new TourRequestViewModel(request), title + request.Id));
            }
        }

        public void OpenInbox()
        {
            new NotificationsWindow(LoggedInUser).ShowDialog();
            UnreadNotificationCount = notificationService.GetUnreadNotificationCount(LoggedInUser.Id);
        }

        public void SortingSelectionChanged()
        {
            tourRequestService.SortTours(TourRequests, RequestsSelectedSort);
        }
        public void CreateTourRequest()
        {
            new CreateTourRequestWindow(LoggedInUser,false,new ComplexTourRequest()).ShowDialog();
            GetMyRequests();
            FetchStatistics();
        }
    }
}
