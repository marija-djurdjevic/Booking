using BookingApp.DTO;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using BookingApp.Model;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Collections;

namespace BookingApp.TouristView
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window, INotifyPropertyChanged
    {
        public TourDto SearchParams { get; set; }
        public static ObservableCollection<TourDto> Tours { get; set; }
        public static List<string> Countires { get; set; }
        public static List<string> AllCities { get; set; }
        public static List<string> Languages { get; set; }
        public LocationDto SelectedLocation { get; set; }

        private List<string> cities;

        private readonly TourRepository TourRepository;
        private readonly GlobalLanguagesRepository GlobalLanguagesRepository;
        private readonly GlobalLocationsRepository GlobalLocationsRepository;
        public bool IsCancelSearchButtonVisible { get; set; }

        public List<string> Cities
        {
            get => cities;
            set
            {
                if (value != cities)
                {
                    cities = value;
                    OnPropertyChanged(nameof(cities));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public SearchWindow(ObservableCollection<TourDto> tours)
        {
            InitializeComponent();

            DataContext = this;
            Tours = tours;
            SearchParams = new TourDto();
            SelectedLocation = new LocationDto();

            TourRepository = new TourRepository();
            GlobalLanguagesRepository = new GlobalLanguagesRepository();
            GlobalLocationsRepository = new GlobalLocationsRepository();

            Countires = GlobalLocationsRepository.GetAllCountries();
            Languages = GlobalLanguagesRepository.GetAll();
            AllCities = GlobalLocationsRepository.GetAllCities();

            IsCancelSearchButtonVisible = false;

            UpdateCitiesFromList(AllCities);
        }

        private void UpdateCitiesFromList(List<string> cities)
        {
            Cities = cities;
        }

        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            List<Tour> matchingTours = TourRepository.GetMatchingTours(SearchParams);

            if (matchingTours.Count > 0)
            {
                UpdateCollection(matchingTours);
                IsCancelSearchButtonVisible = true;
            }
            else
            {
                MessageBox.Show("There are no tours with that parameters");
                UpdateCollection(TourRepository.GetAll());
            }

            Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void UpdateCollection(List<Tour> tours)
        {
            Tours.Clear();
            foreach (var tour in tours)
            {
                Tours.Add(new TourDto(tour));
            }
        }

        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void CityComboBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (AllCities.Contains(SelectedLocation.City) && !string.Equals(SearchParams.LocationDto.City, SelectedLocation.City))
            {
                SelectedLocation.Country = GlobalLocationsRepository.GetCountryForCity(SelectedLocation.City.ToString());
            }
            SearchParams.LocationDto.City = SelectedLocation.City;
        }

        private void CountryComboBoxChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SelectedLocation.Country) && !string.Equals(SearchParams.LocationDto.Country, SelectedLocation.Country))
            {
                List<string> citisInCountry = GlobalLocationsRepository.GetCitiesFromCountry(SelectedLocation.Country.ToString());
                UpdateCitiesFromList(citisInCountry);
            }
            else if(string.IsNullOrEmpty(SelectedLocation.Country) && !string.Equals(SearchParams.LocationDto.Country, SelectedLocation.Country))
                UpdateCitiesFromList(AllCities);
            SearchParams.LocationDto.Country = SelectedLocation.Country;
        }

        private void OpenDropDownClick(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            if(Cities.Count()>1000)
                comboBox.IsDropDownOpen = false;
            else
                comboBox.IsDropDownOpen = true;
        }
    }
}
