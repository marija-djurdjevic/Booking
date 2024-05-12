using BookingApp.Aplication;
using BookingApp.Aplication.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class AnalyticsViewModel : INotifyPropertyChanged
    {
        private readonly User _loggedInUser;
        private readonly PropertyService _propertyService;
        private readonly PropertyReservationService _propertyReservationService;
        private readonly RenovationService _renovationService;
        private readonly ChangeRequestService _changeRequestService;
        private readonly RenovationReccomendationService _renovationReccomendationService;
        public SeriesCollection HistogramData { get; private set; }
        public SeriesCollection CanceledReservationsHistogramData { get; private set; }
        public SeriesCollection AcceptedRequestsHistogramData { get; private set; }
        public SeriesCollection RenovationRecommendationHistogramData { get; private set; }
        public List<string> Labels { get; set; }
        public ObservableCollection<string> AllPropertyNames { get; set; }
        public ObservableCollection<int> Years { get; set; }


        private string _selectedProperty;
        private int _selectedYear;
        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    OnPropertyChanged(nameof(SelectedYear));
                    
                    if (_selectedYear != 0)
                    {

                        LoadHistogramDataByMonth(_selectedYear);
                        LoadCanceledReservationsHistogramDataByMonth(_selectedYear);
                        LoadAcceptedRequestsHistogramDataByMonth(_selectedYear);
                        LoadRenovationRecommendationHistogramDataByMonth(_selectedYear);
                        UpdateMostOccupiedYearAndMonth();
                    }
                    else
                    {
                        LoadHistogramData();
                        LoadCanceledReservationsHistogramData();
                        LoadRenovationRecommendationData();
                        LoadAcceptedRequestsHistogramData();
                        UpdateMostOccupiedYearAndMonth();
                    }
                }
            }
        }
        public AnalyticsViewModel(User loggedInUser)
        {
            HistogramData = new SeriesCollection();
            CanceledReservationsHistogramData = new SeriesCollection();
            RenovationRecommendationHistogramData = new SeriesCollection();
            Labels = new List<string> { "" };
            Years = new ObservableCollection<int> { 2020, 2021, 2022, 2023, 2024 };

            _loggedInUser = loggedInUser;
            _propertyService = new PropertyService(Injector.CreateInstance<IPropertyRepository>(), Injector.CreateInstance<IPropertyReservationRepository>());
            _propertyReservationService = new PropertyReservationService(Injector.CreateInstance<IPropertyRepository>(),Injector.CreateInstance<IPropertyReservationRepository>(), Injector.CreateInstance<IReservedDateRepository>());
            _changeRequestService = new ChangeRequestService(Injector.CreateInstance<IReservationChangeRequestRepository>());
            _renovationReccomendationService = new RenovationReccomendationService(Injector.CreateInstance<IRenovationReccomendationRepository>(), Injector.CreateInstance<IOwnerReviewRepository>(), Injector.CreateInstance<IPropertyReservationRepository>());
            Initialize();
            _selectedProperty = AllPropertyNames.FirstOrDefault();
           
            //LoadHistogramData();
            //LoadCanceledReservationsHistogramData();

        }
        private void UpdateMostOccupiedYearAndMonth()
        {
            if (_selectedYear == 0)
            {
                
                int mostOccupiedYear = _propertyReservationService.GetMostOccupiedYear(_selectedProperty);
                MostOccupiedText = $"Most occupied year: {mostOccupiedYear}";
                
            }
            else
            {
                
                int mostOccupiedMonth = _propertyReservationService.GetMostOccupiedMonthInYear(_selectedProperty, _selectedYear);
                MostOccupiedText = $"Most occupied month in {_selectedYear}: {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(mostOccupiedMonth)}";

            }
        }

        private void LoadCanceledReservationsHistogramData()
        {
            CanceledReservationsHistogramData.Clear(); 
            for (int year = 2020; year <= 2024; year++)
            {
                CanceledReservationsHistogramData.Add(new ColumnSeries
                {
                    Title = year.ToString(),
                    Values = new ChartValues<int> { GetCanceledReservationsCountForYear(year) }
                });
            }
        }
        private void LoadAcceptedRequestsHistogramData()
        {
            AcceptedRequestsHistogramData = new SeriesCollection();
            for (int year = 2020; year <= 2024; year++)
            {
                AcceptedRequestsHistogramData.Add(new ColumnSeries
                {
                    Title = year.ToString(),
                    Values = new ChartValues<int> { GetAcceptedRequestsCountForYear(year) }
                });
            }
            OnPropertyChanged(nameof(AcceptedRequestsHistogramData));
        }
        private int GetAcceptedRequestsCountForYear(int year)
        {
            return _changeRequestService.GetAcceptedReservationChangeRequestsCount(_selectedProperty, year);
        }

        private int GetCanceledReservationsCountForYear(int year)
        {
            return _propertyReservationService.GetCanceledReservationsCount(_selectedProperty, year);
        }
        private void LoadHistogramData()
        {
            HistogramData.Clear(); 

            for (int year = 2020; year <= 2024; year++)
            {
                HistogramData.Add(new ColumnSeries
                {
                    Title = year.ToString(),
                    Values = new ChartValues<int> { GetReservationCountForYear(_selectedProperty, year) }
                });
            }
        }
        private void LoadRenovationRecommendationData()
        {
            RenovationRecommendationHistogramData.Clear();
            for (int year = 2020; year <= 2024; year++)
            {
                RenovationRecommendationHistogramData.Add(new ColumnSeries
                {
                    Title = year.ToString(),
                    Values = new ChartValues<int> { GetReservationRecommendationCountForYear(_selectedProperty, year) }
                });
            }

        }
        private int GetReservationRecommendationCountForYear(string _selectedProperty, int year)
        {
            return _renovationReccomendationService.GetRenovationRecommendationsCountForProperty(_selectedProperty, year);
        }
        private int GetReservationCountForYear(string _selectedProperty, int year)
        {
            return _propertyReservationService.GetReservationsCountForYear(_selectedProperty, year);
        }

        public string SelectedProperty
        {
            get { return _selectedProperty; }
            set
            {
              
                if (_selectedProperty != value)
                {
                    _selectedProperty = value;
                    OnPropertyChanged(nameof(SelectedProperty));
                    if (_selectedYear != 0)
                    {
                        LoadHistogramDataByMonth(_selectedYear);
                        LoadCanceledReservationsHistogramDataByMonth(_selectedYear);
                        LoadAcceptedRequestsHistogramDataByMonth(_selectedYear);
                        LoadRenovationRecommendationHistogramDataByMonth(_selectedYear);
                        UpdateMostOccupiedYearAndMonth();
                    }
                    else
                    {
                        LoadHistogramData();
                        LoadCanceledReservationsHistogramData();
                        LoadRenovationRecommendationData();
                        LoadAcceptedRequestsHistogramData();
                        UpdateMostOccupiedYearAndMonth();
                    }
                   
                }
            }
        }
        private string _mostOccupiedText;
        public string MostOccupiedText
        {
            get { return _mostOccupiedText; }
            set
            {
                _mostOccupiedText = value;
                OnPropertyChanged(nameof(MostOccupiedText));
            }
        }


        private void LoadHistogramDataByMonth(int year)
        {
            HistogramData.Clear();

            for (int month = 1; month <= 12; month++)
            {
                HistogramData.Add(new ColumnSeries
                {
                    Title = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month),
                    Values = new ChartValues<int> { GetReservationCountForMonth(_selectedProperty, year, month) }
                });
            }
        }

        private void LoadCanceledReservationsHistogramDataByMonth(int year)
        {
            CanceledReservationsHistogramData.Clear();

            for (int month = 1; month <= 12; month++)
            {
                CanceledReservationsHistogramData.Add(new ColumnSeries
                {
                    Title = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month),
                    Values = new ChartValues<int> { GetCanceledReservationsCountForMonth(_selectedProperty, year, month) }
                });
            }
        }
        private void LoadAcceptedRequestsHistogramDataByMonth(int year)
        {
            AcceptedRequestsHistogramData.Clear();

            for (int month = 1; month <= 12; month++)
            {
                AcceptedRequestsHistogramData.Add(new ColumnSeries
                {
                    Title = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month),
                    Values = new ChartValues<int> { GetAcceptedRequestsCountForMonth(_selectedProperty, year, month) }
                });
            }
        }
        private int GetAcceptedRequestsCountForMonth(string _selectedProperty, int year, int month)
        {
            return _changeRequestService.GetAcceptedReservationChangeRequestsCountForMonth(_selectedProperty, year, month);
        }
        private void LoadRenovationRecommendationHistogramDataByMonth(int year)
        {
            RenovationRecommendationHistogramData.Clear();

            for (int month = 1; month <= 12; month++)
            {
                RenovationRecommendationHistogramData.Add(new ColumnSeries
                {
                    Title = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month),
                    Values = new ChartValues<int> { GetRenovationRecommendationCountForMonth(_selectedProperty, year, month) }
                });
            }
        }
        private int GetRenovationRecommendationCountForMonth(string _selectedProperty, int year, int month)
        {
            return _renovationReccomendationService.GetRenovationRecommendationsForMonth(_selectedProperty, year, month);
        }
        private int GetCanceledReservationsCountForMonth(string _selectedProperty, int year, int month)
        {
            return _propertyReservationService.GetCanceledReservationsCountForMonth(_selectedProperty, year, month);
        }
        private int GetReservationCountForMonth(string selectedProperty, int year, int month)
        {
            return _propertyReservationService.GetReservationsCountForMonth(selectedProperty, year, month);
        }
        private void Initialize()
        {
            AllPropertyNames = new ObservableCollection<string>(_propertyService.GetAllPropertyNames());
        }
        
       
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        

    }
}
