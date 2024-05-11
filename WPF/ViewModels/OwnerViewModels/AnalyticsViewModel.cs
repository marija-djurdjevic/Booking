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

        private string _selectedProperty;
        
        public AnalyticsViewModel(User loggedInUser)
        {
            HistogramData = new SeriesCollection();
            CanceledReservationsHistogramData = new SeriesCollection();
            RenovationRecommendationHistogramData = new SeriesCollection();
            Labels = new List<string> { "2020 2021 2022 2023 2024" };
            
            _loggedInUser = loggedInUser;
            _propertyService = new PropertyService(Injector.CreateInstance<IPropertyRepository>(), Injector.CreateInstance<IPropertyReservationRepository>());
            _propertyReservationService = new PropertyReservationService(Injector.CreateInstance<IPropertyRepository>(),Injector.CreateInstance<IPropertyReservationRepository>(), Injector.CreateInstance<IReservedDateRepository>());
            _changeRequestService = new ChangeRequestService(Injector.CreateInstance<IReservationChangeRequestRepository>());
            _renovationReccomendationService = new RenovationReccomendationService(Injector.CreateInstance<IRenovationReccomendationRepository>(), Injector.CreateInstance<IOwnerReviewRepository>(), Injector.CreateInstance<IPropertyReservationRepository>());
            Initialize();
            _selectedProperty = AllPropertyNames.FirstOrDefault();
           
            LoadHistogramData();
            LoadCanceledReservationsHistogramData();

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
                    
                    LoadHistogramData();
                    LoadCanceledReservationsHistogramData();
                    LoadAcceptedRequestsHistogramData();
                    LoadRenovationRecommendationData();
                }
            }
        }
      
        private void Initialize()
        {
            AllPropertyNames = new ObservableCollection<string>(_propertyService.GetAllPropertyNames());
        }
        
        public Dictionary<int, int> GetYearlyCanceledReservationsCount(string property)
        {
            Dictionary<int, int> yearlyCanceledReservationsCount = new Dictionary<int, int>();

            for (int year = 2020; year <= 2024; year++)
            {
                int canceledReservationsCount = _propertyReservationService.GetCanceledReservationsCount(property, year);
                yearlyCanceledReservationsCount.Add(year, canceledReservationsCount);
            }

            return yearlyCanceledReservationsCount;
        }
        public Dictionary<int, int> GetAcceptedReservationChangeRequestsCountForAllYears(string property)
        {
            Dictionary<int, int> acceptedRequestsCountForAllYears = new Dictionary<int, int>();

            for (int year = 2020; year <= 2024; year++)
            {
                int acceptedRequestsCount = _changeRequestService.GetAcceptedReservationChangeRequestsCount(property, year);
                acceptedRequestsCountForAllYears.Add(year, acceptedRequestsCount);
            }

            return acceptedRequestsCountForAllYears;
        }
        public Dictionary<int, int> GetRenovationRecommendationsCountForProperty(string property)
        {
            Dictionary<int, int> renovationRecommendationsCountForProperty = new Dictionary<int, int>();

            for (int year = 2020; year <= 2024; year++)
            {
                int renovationRecommendationsCount = _renovationReccomendationService.GetRenovationRecommendationsCountForProperty(property, year);
                renovationRecommendationsCountForProperty.Add(year, renovationRecommendationsCount);
            }

            return renovationRecommendationsCountForProperty;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        

    }
}
