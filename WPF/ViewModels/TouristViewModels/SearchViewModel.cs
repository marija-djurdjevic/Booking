using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using BookingApp.Domain.Models;
using BookingApp.Aplication.UseCases;
using BookingApp.Aplication.Dto;

namespace BookingApp.WPF.ViewModel.TouristViewModel
{
    public class SearchViewModel : INotifyPropertyChanged
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
        private readonly SearchTourService SearchTourService;
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


        public SearchViewModel(ObservableCollection<TourDto> tours)
        {
            Tours = tours;
            SearchParams = new TourDto();
            SelectedLocation = new LocationDto();

            TourRepository = new TourRepository();
            GlobalLanguagesRepository = new GlobalLanguagesRepository();
            GlobalLocationsRepository = new GlobalLocationsRepository();

            Countires = GlobalLocationsRepository.GetAllCountries();
            Languages = GlobalLanguagesRepository.GetAll();
            AllCities = GlobalLocationsRepository.GetAllCities();
            SearchTourService = new SearchTourService();

            IsCancelSearchButtonVisible = false;

            UpdateCitiesFromList(AllCities);
        }

        private void UpdateCitiesFromList(List<string> cities)
        {
            Cities = cities;
        }

        public void Confirm()
        {
            List<Tour> matchingTours = SearchTourService.GetMatchingTours(SearchParams);

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
        }

        private void UpdateCollection(List<Tour> tours)
        {
            Tours.Clear();
            foreach (var tour in tours)
            {
                Tours.Add(new TourDto(tour));
            }
        }

        public void CityComboBoxLostFocus()
        {
            if (AllCities.Contains(SelectedLocation.City) && !string.Equals(SearchParams.LocationDto.City, SelectedLocation.City))
            {
                SelectedLocation.Country = GlobalLocationsRepository.GetCountryForCity(SelectedLocation.City.ToString());
            }
            SearchParams.LocationDto.City = SelectedLocation.City;
        }

        public void CountryComboBoxChanged()
        {
            if (!string.Equals(SearchParams.LocationDto.Country, SelectedLocation.Country))
            {
                if (!string.IsNullOrEmpty(SelectedLocation.Country))
                {
                    List<string> citisInCountry = GlobalLocationsRepository.GetCitiesFromCountry(SelectedLocation.Country.ToString());
                    UpdateCitiesFromList(citisInCountry);
                }
                else if (string.IsNullOrEmpty(SelectedLocation.Country))
                    UpdateCitiesFromList(AllCities);
                SearchParams.LocationDto.Country = SelectedLocation.Country;

            }
        }

        public void OpenDropDownClick(object sender)
        {
            var comboBox = sender as ComboBox;
            comboBox.IsDropDownOpen = Cities.Count() > 100 ? false : true;
        }
    }
}
