using BookingApp.Aplication;
using BookingApp.Aplication.Dto;
using BookingApp.Aplication.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.View.TouristView;
using BookingApp.WPF.ViewModels.TouristViewModels;
using BookingApp.WPF.Views.TouristView;
using GalaSoft.MvvmLight.Messaging;
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
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

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

        private TourRequestViewModel tourRequestViewModel;
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

        private DateTime minStartDate;
        public DateTime MinStartDate
        {
            get { return minStartDate; }
            set
            {
                minStartDate = value;
                OnPropertyChanged(nameof(MinStartDate));
            }
        }
        private DateTime minEndDate;
        public DateTime MinEndDate
        {
            get { return minEndDate; }
            set
            {
                minEndDate = value;
                OnPropertyChanged(nameof(MinEndDate));
            }
        }
        public bool IsComplex { get; set; }
        public ComplexTourRequest ComplexTourRequest { get; set; }

        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand HelpCommand { get; set; }
        public RelayCommand CityComboBoxCommand { get; set; }
        public RelayCommand CountryComboBoxCommand { get; set; }
        public RelayCommand<object> OpenDropDownCommand { get; set; }
        public RelayCommand<DatePicker> StartDatePickerOpenedCommand { get; private set; }
        public RelayCommand<DatePicker> EndDatePickerOpenedCommand { get; private set; }

        public CreateTourRequestViewModel(User loggedInUser, bool isComplex, ComplexTourRequest complexTourRequest)
        {
            LoggedInUser = loggedInUser;
            TourRequestViewModel = new TourRequestViewModel();
            TourRequestViewModel.TouristId = LoggedInUser.Id;
            IsComplex = isComplex;
            ComplexTourRequest = complexTourRequest;

            GlobalLanguagesService = new GlobalLanguagesService(Injector.CreateInstance<IGlobalLanguagesRepository>());
            GlobalLocationsService = new GlobalLocationsService(Injector.CreateInstance<IGlobalLocationsRepository>());

            Countires = GlobalLocationsService.GetAllCountries();
            Languages = GlobalLanguagesService.GetAll();
            AllCities = GlobalLocationsService.GetAllCities();
            oldCity = "";
            MinStartDate = DateTime.Today.AddDays(3);
            MinEndDate = DateTime.Today.AddDays(3);

            UpdateCitiesFromList(AllCities);
            ConfirmCommand = new RelayCommand(Confirm);
            CancelCommand = new RelayCommand(CloseWindow);
            HelpCommand = new RelayCommand(Help);
            CityComboBoxCommand = new RelayCommand(CityComboBoxLostFocus);
            CountryComboBoxCommand = new RelayCommand(CountryComboBoxChanged);
            OpenDropDownCommand = new RelayCommand<object>(OpenDropDownClick);
            StartDatePickerOpenedCommand = new RelayCommand<DatePicker>(StartDatePickerClosed);
            EndDatePickerOpenedCommand = new RelayCommand<DatePicker>(EndDatePickerOpened);
        }
        public RelayCommand<object> FocusUpCommand => new RelayCommand<object>(FocusToComboBox);
        public RelayCommand<object> FocusDownCommand => new RelayCommand<object>(FocusToBox);

        private void FocusToComboBox(object o)
        {
            // Assuming MyComboBox is your ComboBox instance
            ComboBox comboBox = o as ComboBox;
            if (comboBox != null)
                comboBox.Focus();
        }
        private void FocusToBox(object o)
        {
            // Assuming MyComboBox is your ComboBox instance
            TextBox comboBox = o as TextBox;
            if (comboBox != null)
                comboBox.Focus();
        }

        private void StartDatePickerClosed(DatePicker datePicker)
        {
            datePicker.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Now.AddDays(2)));
            foreach (var request in ComplexTourRequest.TourRequests)
            {
                datePicker.BlackoutDates.Add(new CalendarDateRange(request.StartDate, request.EndDate));
            }
            List<TourRequest> requestsWithBiggerDatesOdStartDate = ComplexTourRequest.TourRequests.FindAll(t => t.StartDate > TourRequestViewModel.StartDate);
            if (requestsWithBiggerDatesOdStartDate.Count > 0)
                TourRequestViewModel.MaxValidDate = requestsWithBiggerDatesOdStartDate.Min(t => t.StartDate);
            else
                TourRequestViewModel.MaxValidDate = DateTime.MaxValue;
        }
        private void EndDatePickerOpened(DatePicker datePicker)
        {
            foreach (var request in ComplexTourRequest.TourRequests)
            {
                datePicker.BlackoutDates.Add(new CalendarDateRange(request.StartDate, request.EndDate));
            }
            List<TourRequest> requestsWithBiggerDatesOdStartDate = ComplexTourRequest.TourRequests.FindAll(t => t.StartDate > TourRequestViewModel.StartDate);
            if (requestsWithBiggerDatesOdStartDate.Count > 0 && TourRequestViewModel.StartDate > DateTime.Now.AddDays(2))
            {
                //Ima problema
                DateTime minDate = requestsWithBiggerDatesOdStartDate.Min(t => t.StartDate).AddDays(-1);
                datePicker.DisplayDateEnd = minDate;
                if (TourRequestViewModel.EndDate > minDate)
                    TourRequestViewModel.EndDate = DateTime.Now;
            }
            else
            {
                datePicker.DisplayDateEnd = DateTime.MaxValue;
            }
            if (TourRequestViewModel.EndDate < TourRequestViewModel.StartDate)
            {
                datePicker.SelectedDate = TourRequestViewModel.StartDate;
                TourRequestViewModel.EndDate = TourRequestViewModel.StartDate;
            }
            datePicker.DisplayDateStart = TourRequestViewModel.StartDate > DateTime.Now.AddDays(3) ? TourRequestViewModel.StartDate : DateTime.Now.AddDays(3);
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
            new TouristsDataWindow(TourRequestViewModel.TouristNumber, new TourDto(), LoggedInUser.Id, true, TourRequestViewModel, IsComplex, ComplexTourRequest).ShowDialog();
        }

        private void Help()
        {
            new HelpCreateTourRequestWindow().Show();
        }

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
