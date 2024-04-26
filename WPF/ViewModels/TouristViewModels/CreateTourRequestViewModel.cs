using BookingApp.Aplication;
using BookingApp.Aplication.Dto;
using BookingApp.Aplication.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.View.TouristView;
using BookingApp.WPF.ViewModels.TouristViewModels;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.TouristViewModels
{
    public class CreateTourRequestViewModel : BindableBase
    {
        private readonly GlobalLanguagesService GlobalLanguagesService;
        private readonly GlobalLocationsService GlobalLocationsService;

        public static List<string> Countires { get; set; }
        public static List<string> AllCities { get; set; }
        public static List<string> Languages { get; set; }
        private string oldCity;

        private List<string> cities;
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
        public User LoggedInUser { get; set; }
        private TourRequestViewModel tourRequestViewModel { get; set; }
        public TourRequestViewModel TourRequestViewModel
        {
            get => tourRequestViewModel;
            set
            {
                if (value != tourRequestViewModel)
                {
                    tourRequestViewModel = value;
                    OnPropertyChanged(nameof(TourRequestViewModel));
                }
            }
        }

        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand HelpCommand { get; set; }
        public RelayCommand CityComboBoxCommand { get; set; }
        public RelayCommand CountryComboBoxCommand { get; set; }
        public RelayCommand<object> OpenDropDownCommand { get; set; }

        public CreateTourRequestViewModel(User loggedInUser)
        {
            LoggedInUser = loggedInUser;
            TourRequestViewModel = new TourRequestViewModel();
            TourRequestViewModel.TouristId = LoggedInUser.Id;

            GlobalLanguagesService = new GlobalLanguagesService(Injector.CreateInstance<IGlobalLanguagesRepository>());
            GlobalLocationsService = new GlobalLocationsService(Injector.CreateInstance<IGlobalLocationsRepository>());

            Countires = GlobalLocationsService.GetAllCountries();
            Languages = GlobalLanguagesService.GetAll();
            AllCities = GlobalLocationsService.GetAllCities();
            oldCity = "";

            UpdateCitiesFromList(AllCities);
            ConfirmCommand = new RelayCommand(Confirm);
            CancelCommand = new RelayCommand(CloseWindow);
            HelpCommand = new RelayCommand(Help);
            CityComboBoxCommand = new RelayCommand(CityComboBoxLostFocus);
            CountryComboBoxCommand = new RelayCommand(CountryComboBoxChanged);
            OpenDropDownCommand = new RelayCommand<object>(OpenDropDownClick);
        }

        private void CloseWindow()
        {
            // Slanje poruke za zatvaranje prozora koristeći MVVM Light Messaging
            Style style = Application.Current.FindResource("MessageStyle") as Style;
            MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("Are you sure you want to close window?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning, style);
            if (result == MessageBoxResult.Yes)
                Messenger.Default.Send(new NotificationMessage("CloseCreateTourRequestWindowMessage"));
        }

        private void UpdateCitiesFromList(List<string> cities)
        {
            Cities = cities;
        }

        public void Confirm()
        {
            if (!TourRequestViewModel.IsValid)
            {
                Style style = Application.Current.FindResource("MessageStyle") as Style;
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("All fields must be filled correctly!", "Error", MessageBoxButton.OK, MessageBoxImage.Error, style);
                return;
            }
            new TouristsDataWindow(TourRequestViewModel.TouristNumber, new TourDto(), LoggedInUser.Id, true, TourRequestViewModel).ShowDialog();
        }

        private void Help()
        { }

        public void CityComboBoxLostFocus(object parameter)
        {
            if (AllCities.Contains(TourRequestViewModel.City) && !string.Equals(TourRequestViewModel.City, oldCity))
            {
                TourRequestViewModel.Country = GlobalLocationsService.GetCountryForCity(TourRequestViewModel.City.ToString());
                oldCity = TourRequestViewModel.City;
            }
        }

        public void CountryComboBoxChanged()
        {
            if (!string.IsNullOrEmpty(TourRequestViewModel.Country))
            {
                List<string> citisInCountry = GlobalLocationsService.GetCitiesFromCountry(TourRequestViewModel.Country.ToString());
                UpdateCitiesFromList(citisInCountry);
            }
            else if (string.IsNullOrEmpty(TourRequestViewModel.Country))
                UpdateCitiesFromList(AllCities);
        }

        public void OpenDropDownClick(object sender)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
                comboBox.IsDropDownOpen = Cities.Count() > 500 ? false : true;
        }
    }
}
