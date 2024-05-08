using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
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
        private DateRange SelectedDateRange;
        public List<string> AllPropertyNames { get; set; }
        private List<DateRange> AvailableDateRanges;


        public ScheduleRenovationPage(User loggedInUser)
        {
            InitializeComponent();
            LoggedInUser = loggedInUser;
            _propertyRepository = new PropertyRepository();
            _propertyReservationRepository = new PropertyReservationRepository();
            _renovationRepository = new RenovationRepository();
            Loaded += ScheduleRenovationPage_Loaded;
        }
        private void ScheduleRenovationPage_Loaded(object sender, RoutedEventArgs e)
        {
            AllPropertyNames = _propertyRepository.GetAllPropertyNames();
            DataContext = this;
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
           
            AvailableDateRanges = new List<DateRange>();

            //var selectedProperty = (Property)PropertyComboBox.SelectedItem;
            var allProperties = _propertyRepository.GetAllProperties();
            var selectedPropertyName = (string)PropertyComboBox.SelectedItem;
            var selectedProperty = allProperties.FirstOrDefault(property => property.Name == selectedPropertyName);


            DateTime startDate = StartDatePicker.SelectedDate ?? DateTime.MinValue;
            DateTime endDate = EndDatePicker.SelectedDate ?? DateTime.MaxValue;
            int duration;
            int.TryParse(DurationTextBox.Text, out duration);
            AvailableDateRanges = GetAvailableDateRanges(selectedProperty, startDate, endDate, duration);
            //AvailableDateRanges = GetAvailableDateRanges(selectedProperty, startDate, endDate);

            if (AvailableDateRanges.Count == 0)
            {
                MessageBox.Show("There are no available dates for renovation.");
                return;
            }

            DateDataGrid.ItemsSource = AvailableDateRanges;
        }
        /*private List<DateRange> GetAvailableDateRanges(Property selectedProperty, DateTime startDate, DateTime endDate)
        {
            var availableDateRanges = new List<DateRange>();

            var reservations = _propertyReservationRepository.GetAllPropertyReservationsByPropertyId(selectedProperty.Id);

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (!IsDateReserved(reservations, date))
                {
                    availableDateRanges.Add(new DateRange(selectedProperty.Id, date, date));
                }
            }

            return availableDateRanges;
        }*/
        private List<DateRange> GetAvailableDateRanges(Property selectedProperty, DateTime startDate, DateTime endDate, int duration)
        {
            var availableDateRanges = new List<DateRange>();

            var reservations = _propertyReservationRepository.GetAllPropertyReservationsByPropertyId(selectedProperty.Id);

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (!IsDateReserved(reservations, date))
                {
                    // Provjeravamo da li su i sljedeći 'duration' dana slobodni za renoviranje
                    bool isAvailable = true;
                    for (int i = 1; i < duration; i++)
                    {
                        if (IsDateReserved(reservations, date.AddDays(i)))
                        {
                            isAvailable = false;
                            break;
                        }
                    }

                    // Ako su svi dani u 'duration' slobodni, dodajemo ih u listu dostupnih datuma
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
        }

        private void DateDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedDateRange = (DateRange)DateDataGrid.SelectedItem;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
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

            // Kreiramo novi objekat Renovation sa odabranim podacima
            Renovation newRenovation = new Renovation()
            {
                OwnerId = LoggedInUser.Id,
                PropertyId = SelectedDateRange.PropertyId,
                StartDate = SelectedDateRange.Start,
                EndDate = SelectedDateRange.End,
                Description = RenovationDescriptionTextBox.Text,
                Duration = duration
            };

            // Čuvamo renoviranje u bazi podataka
            _renovationRepository.Save(newRenovation);

            MessageBox.Show("Renovation saved successfully.");
        }
        private void DateDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectedDateRange = (DateRange)DateDataGrid.SelectedItem;
        }
    }
}
