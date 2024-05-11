using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using BookingApp.Domain.Models;
using BookingApp.Aplication.UseCases;
using BookingApp.Aplication.Dto;
using BookingApp.Aplication;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Command;
using GalaSoft.MvvmLight.Messaging;
using Xceed.Wpf.Toolkit;

namespace BookingApp.WPF.ViewModels.TouristViewModels
{
    public class SearchViewModel : BindableBase
    {
        public TourDto SearchParams { get; set; }
        public static ObservableCollection<TourDto> Tours { get; set; }
        public static List<string> Countires { get; set; }
        public static List<string> AllCities { get; set; }
        public static List<string> Languages { get; set; }
        public LocationDto SelectedLocation { get; set; }

        private List<string> cities;

        private readonly TourService TourService;
        private readonly GlobalLanguagesService GlobalLanguagesService;
        private readonly SearchTourService SearchTourService;
        private readonly GlobalLocationsService GlobalLocationsService;

        public List<string> Cities
        {
            get => cities;
            set
            {
                if (value != cities)
                {
                    cities = value;
                    OnPropertyChanged(nameof(Cities));
                }
            }
        }

        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand HelpCommand { get; set; }
        public RelayCommand CityComboBoxCommand { get; set; }
        public RelayCommand CountryComboBoxCommand { get; set; }
        public RelayCommand<object> OpenDropDownCommand { get; set; }

        public SearchViewModel(ObservableCollection<TourDto> tours)
        {
            Tours = tours;
            SearchParams = new TourDto();
            SelectedLocation = new LocationDto();

            TourService = new TourService(Injector.CreateInstance<ITourRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            GlobalLanguagesService = new GlobalLanguagesService(Injector.CreateInstance<IGlobalLanguagesRepository>());
            GlobalLocationsService = new GlobalLocationsService(Injector.CreateInstance<IGlobalLocationsRepository>());

            Countires = GlobalLocationsService.GetAllCountries();
            Languages = GlobalLanguagesService.GetAll();
            AllCities = GlobalLocationsService.GetAllCities();
            SearchTourService = new SearchTourService(Injector.CreateInstance<ITourRepository>());

            UpdateCitiesFromList(AllCities);
            ConfirmCommand = new RelayCommand(Confirm);
            CancelCommand = new RelayCommand(CloseWindow);
            HelpCommand = new RelayCommand(Help);
            CityComboBoxCommand = new RelayCommand(CityComboBoxLostFocus);
            CountryComboBoxCommand = new RelayCommand(CountryComboBoxChanged);
            OpenDropDownCommand = new RelayCommand<object>(OpenDropDownClick);
        }
        public RelayCommand<object> FocusUpCommand => new RelayCommand<object>(FocusToComboBox);
        public RelayCommand<object> FocusDownCommand => new RelayCommand<object>(FocusToInteger);

        private void FocusToComboBox(object o)
        {
            // Assuming MyComboBox is your ComboBox instance
            ComboBox comboBox = o as ComboBox;
            if(comboBox != null)
                comboBox.Focus();
        }
        private void FocusToInteger(object o)
        {
        // Assuming MyComboBox is your ComboBox instance
            IntegerUpDown comboBox = o as IntegerUpDown;
            if (comboBox != null)
                comboBox.Focus();
        }
        private void CloseWindow()
        {
            // Slanje poruke za zatvaranje prozora koristeći MVVM Light Messaging
            Style style = Application.Current.FindResource("MessageStyle") as Style;
            MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("Are you sure you want to close window?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning, style);
            if (result == MessageBoxResult.Yes)
                Messenger.Default.Send(new NotificationMessage("CloseSearchWindowMessage"));
        }
        private void Help()
        {

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
                Messenger.Default.Send(new NotificationMessage("ShowAllButtonMessage"));
            }
            else
            {
                Style style = Application.Current.FindResource("MessageStyle") as Style;
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("There are no tours with that parameters!", "Search", MessageBoxButton.OK, MessageBoxImage.Information, style);
                UpdateCollection(TourService.GetAll());
            }
            Messenger.Default.Send(new NotificationMessage("CloseSearchWindowMessage"));
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
                SelectedLocation.Country = GlobalLocationsService.GetCountryForCity(SelectedLocation.City.ToString());
            }
            SearchParams.LocationDto.City = SelectedLocation.City;
        }

        public void CountryComboBoxChanged()
        {
            if (!string.Equals(SearchParams.LocationDto.Country, SelectedLocation.Country))
            {
                if (!string.IsNullOrEmpty(SelectedLocation.Country))
                {
                    List<string> citisInCountry = GlobalLocationsService.GetCitiesFromCountry(SelectedLocation.Country.ToString());
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
            comboBox.IsDropDownOpen = Cities.Count() > 500 ? false : true;
        }
    }
}
