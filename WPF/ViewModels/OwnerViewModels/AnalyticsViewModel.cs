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
        
        public List<string> Labels { get; set; }
        public ObservableCollection<string> AllPropertyNames { get; set; }

        private string _selectedProperty;
        private Dictionary<int, int> _histogramData;
        public AnalyticsViewModel(User loggedInUser)
        {
            HistogramData = new SeriesCollection();
            Labels = new List<string> { "2020 2021 2022 2023 2024" };
            
            _loggedInUser = loggedInUser;
            _propertyService = new PropertyService(Injector.CreateInstance<IPropertyRepository>(), Injector.CreateInstance<IPropertyReservationRepository>());
            _propertyReservationService = new PropertyReservationService(Injector.CreateInstance<IPropertyRepository>(),Injector.CreateInstance<IPropertyReservationRepository>(), Injector.CreateInstance<IReservedDateRepository>());
            _changeRequestService = new ChangeRequestService(Injector.CreateInstance<IReservationChangeRequestRepository>());
            _renovationReccomendationService = new RenovationReccomendationService(Injector.CreateInstance<IRenovationReccomendationRepository>(), Injector.CreateInstance<IOwnerReviewRepository>(), Injector.CreateInstance<IPropertyReservationRepository>());
            Initialize();
            _selectedProperty = AllPropertyNames.FirstOrDefault();
            /*for (int year = 2020; year <= 2024; year++)
            {
                HistogramData.Add(new ColumnSeries
                {
                    Title = year.ToString(),
                    Values = new ChartValues<int> { GetReservationCountForYear(_selectedProperty, year)
                    }
                });
                
            }*/
            LoadHistogramData();
            

        }
        private void LoadHistogramData()
        {
            HistogramData.Clear(); // Očisti postojeće podatke prije dodavanja novih

            for (int year = 2020; year <= 2024; year++)
            {
                HistogramData.Add(new ColumnSeries
                {
                    Title = year.ToString(),
                    Values = new ChartValues<int> { GetReservationCountForYear(_selectedProperty, year) }
                });
            }
        }
        /*private int GetReservationCountForYear(int year)
        {
            Random rnd = new Random();
            return rnd.Next(50, 200); // Vraćamo nasumičan broj rezervacija između 50 i 200
        }*/
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
                    // Ovdje pozivamo metodu koja će ažurirati podatke o rezervacijama
                    //UpdateHistogramData();
                    LoadHistogramData();
                }
            }
        }
      

        private Dictionary<int, int> GetReservationDataForProperty(string property)
        {
            var random = new Random();
            var data = new Dictionary<int, int>();
            for (int year = 2020; year <= 2024; year++)
            {
                data.Add(year, random.Next(50, 200)); // Vraćamo nasumičan broj rezervacija između 50 i 200
            }
            return data;
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
