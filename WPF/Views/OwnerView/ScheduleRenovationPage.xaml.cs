using BookingApp.Aplication.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels.OwnerViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.Views.OwnerView
{
    /// <summary>
    /// Interaction logic for ScheduleRenovationPage.xaml
    /// </summary>
    public partial class ScheduleRenovationPage : Page
    {
        User LoggedInUser;
        private readonly PropertyRepository _propertyRepository;
        private readonly PropertyReservationRepository _propertyReservationRepository;
        private readonly RenovationRepository _renovationRepository;
        private readonly ScheduleRenovationViewModel viewModel;
        private DateRange SelectedDateRange;
        public List<string> AllPropertyNames { get; set; }
        private List<DateRange> AvailableDateRanges;


        public ScheduleRenovationPage(User loggedInUser)
        {
            InitializeComponent();
           
            LoggedInUser = loggedInUser;
           // _propertyRepository = new PropertyRepository();
            //_propertyReservationRepository = new PropertyReservationRepository();
           // _renovationRepository = new RenovationRepository();
           // Loaded += ScheduleRenovationPage_Loaded;

            viewModel = new ScheduleRenovationViewModel(LoggedInUser);
            DataContext = viewModel;
        }
        private void ScheduleRenovationPage_Loaded(object sender, RoutedEventArgs e)
        {
            AllPropertyNames = _propertyRepository.GetAllPropertyNames();
            DataContext = this;
        }
        /* private void SearchButton_Click(object sender, RoutedEventArgs e)
         {

             AvailableDateRanges = new List<DateRange>();

             var allProperties = _propertyRepository.GetAllProperties();
             var selectedPropertyName = (string)PropertyComboBox.SelectedItem;
             var selectedProperty = allProperties.FirstOrDefault(property => property.Name == selectedPropertyName);


             DateTime startDate = StartDatePicker.SelectedDate ?? DateTime.MinValue;
             DateTime endDate = EndDatePicker.SelectedDate ?? DateTime.MaxValue;
             int duration;
             int.TryParse(DurationTextBox.Text, out duration);
             AvailableDateRanges = GetAvailableDateRanges(selectedProperty, startDate, endDate, duration);

             if (AvailableDateRanges.Count == 0)
             {
                 MessageBox.Show("There are no available dates for renovation.");
                 return;
             }

             DateDataGrid.ItemsSource = AvailableDateRanges;
         }*/
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedPropertyName = (string)PropertyComboBox.SelectedItem;
            DateTime startDate = StartDatePicker.SelectedDate ?? DateTime.MinValue;
            DateTime endDate = EndDatePicker.SelectedDate ?? DateTime.MaxValue;
            int duration;
            int.TryParse(DurationTextBox.Text, out duration);

            ObservableCollection<DateRange> availableDateRanges = new ObservableCollection<DateRange>();
            availableDateRanges = viewModel.SearchAvailableDateRanges(selectedPropertyName, startDate, endDate, duration);
            DateDataGrid.ItemsSource = availableDateRanges;


        }

        /*private List<DateRange> GetAvailableDateRanges(Property selectedProperty, DateTime startDate, DateTime endDate, int duration)
        {
            var availableDateRanges = new List<DateRange>();

            var reservations = _propertyReservationRepository.GetAllPropertyReservationsByPropertyId(selectedProperty.Id);

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (!IsDateReserved(reservations, date))
                {
                    bool isAvailable = true;
                    for (int i = 1; i < duration; i++)
                    {
                        if (IsDateReserved(reservations, date.AddDays(i)))
                        {
                            isAvailable = false;
                            break;
                        }
                    }

                    if (isAvailable)
                    {
                        DateTime endDateForRange = date.AddDays(duration - 1);
                        availableDateRanges.Add(new DateRange(selectedProperty.Id, date, endDateForRange));
                    }
                }
            }

            return availableDateRanges;
        }

        private bool IsDateReserved(List<PropertyReservation> reservations, DateTime date)
        {
            return reservations.Any(reservation => date >= reservation.StartDate && date <= reservation.EndDate);
        }*/

        private void DateDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedDateRange = (DateRange)DateDataGrid.SelectedItem;
        }
        /* private void SubmitButton_Click(object sender, RoutedEventArgs e)
         {
             int duration;
             int.TryParse(DurationTextBox.Text, out duration);
             if (SelectedDateRange == null)
             {
                 MessageBox.Show("Please select a renovation date.");
                 return;
             }

             if (string.IsNullOrWhiteSpace(RenovationDescriptionTextBox.Text))
             {
                 MessageBox.Show("Please enter a renovation description.");
                 return;
             }

             Renovation newRenovation = new Renovation()
             {
                 OwnerId = LoggedInUser.Id,
                 PropertyId = SelectedDateRange.PropertyId,
                 StartDate = SelectedDateRange.Start,
                 EndDate = SelectedDateRange.End,
                 Description = RenovationDescriptionTextBox.Text,
                 Duration = duration
             };

             _renovationRepository.Save(newRenovation);

             MessageBox.Show("Renovation saved successfully.");
             //DataContext = new ScheduleRenovationViewModel();

         }*/
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            int duration;
            int.TryParse(DurationTextBox.Text, out duration);
            if (SelectedDateRange == null)
            {
                MessageBox.Show("Please select a renovation date.");
                return;
            }

            viewModel.SubmitRenovation(LoggedInUser.Id,SelectedDateRange.PropertyId,SelectedDateRange.Start,SelectedDateRange.End,RenovationDescriptionTextBox.Text,duration);
        }
        private void DateDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectedDateRange = (DateRange)DateDataGrid.SelectedItem;
        }
    }
}
