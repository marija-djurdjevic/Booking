using BookingApp.Aplication;
using BookingApp.Aplication.Dto;
using BookingApp.Aplication.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.View.TouristView;
using BookingApp.WPF.ViewModel.TouristViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.TouristViewModels
{
    public class CreateTourRequestViewModel: INotifyPropertyChanged
    {
        private readonly GlobalLanguagesService GlobalLanguagesService;
        private readonly GlobalLocationsService GlobalLocationsService;

        public static List<string> Countires { get; set; }
        public static List<string> AllCities { get; set; }
        public static List<string> Languages { get; set; }

        private List<string> cities;
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
        public LocationDto SelectedLocation { get; set; }
        public User LoggedInUser { get; set; }
        private TourRequest tourRequest { get; set; }
        public TourRequest TourRequest
        {
            get => tourRequest;
            set
            {
                if (value != tourRequest)
                {
                    tourRequest = value;
                    OnPropertyChanged(nameof(tourRequest));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CreateTourRequestViewModel(User loggedInUser)
        {
            LoggedInUser = loggedInUser;
            TourRequest = new TourRequest();
            TourRequest.TouristId = LoggedInUser.Id;
            SelectedLocation = new LocationDto();

            GlobalLanguagesService = new GlobalLanguagesService(Injector.CreateInstance<IGlobalLanguagesRepository>());
            GlobalLocationsService = new GlobalLocationsService(Injector.CreateInstance<IGlobalLocationsRepository>());

            Countires = GlobalLocationsService.GetAllCountries();
            Languages = GlobalLanguagesService.GetAll();
            AllCities = GlobalLocationsService.GetAllCities();

            UpdateCitiesFromList(AllCities);
        }

        private void UpdateCitiesFromList(List<string> cities)
        {
            Cities = cities;
        }

        public void Confirm()
        {
            new TouristsDataWindow(TourRequest.TouristNumber, new TourDto(), LoggedInUser.Id, true, TourRequest).ShowDialog();
        }

        

        public void CityComboBoxLostFocus()
        {
            if (AllCities.Contains(SelectedLocation.City) && !string.Equals(TourRequest.Location.City, SelectedLocation.City))
            {
                SelectedLocation.Country = GlobalLocationsService.GetCountryForCity(SelectedLocation.City.ToString());
            }
            TourRequest.Location.City = SelectedLocation.City;
        }

        public void CountryComboBoxChanged()
        {
            if (!string.Equals(TourRequest.Location.Country, SelectedLocation.Country))
            {
                if (!string.IsNullOrEmpty(SelectedLocation.Country))
                {
                    List<string> citisInCountry = GlobalLocationsService.GetCitiesFromCountry(SelectedLocation.Country.ToString());
                    UpdateCitiesFromList(citisInCountry);
                }
                else if (string.IsNullOrEmpty(SelectedLocation.Country))
                    UpdateCitiesFromList(AllCities);
                TourRequest.Location.Country = SelectedLocation.Country;

            }
        }

        public void OpenDropDownClick(object sender)
        {
            var comboBox = sender as ComboBox;
            comboBox.IsDropDownOpen = Cities.Count() > 500 ? false : true;
        }
    }
}
