using BookingApp.Dto;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.GuestView
{
    /// <summary>
    /// Interaction logic for PropertyBooking.xaml
    /// </summary>
    public partial class PropertyBooking : Window
    {
        public static PropertyRepository PropertyRepository = new PropertyRepository();
        public static PropertyReservationRepository PropertyReservationRepository = new PropertyReservationRepository();
        public static ReservedDateRepository ReservedDateRepository = new ReservedDateRepository();

        public PropertyReservationDto PropertyReservation { get; set; }
        public ReservedDate ReservedDate { get; set; }
        public Property SelectedProperty { get; set; }
        public Guest LoggedInGuest { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateRange SelectedDateRange {  get; set; }
        public List<DateRange> AvailableDateRanges { get; set; }

        public PropertyBooking(Property selectedProperty, Guest guest, PropertyRepository propertyRepository, PropertyReservationRepository propertyReservationRepository)
        {
            InitializeComponent();
            DataContext = this;
            PropertyReservation = new PropertyReservationDto();
            PropertyRepository = propertyRepository;
            PropertyReservationRepository = propertyReservationRepository;
            ReservedDateRepository = new ReservedDateRepository();
            AvailableDateRanges = new List<DateRange>();
            SelectedProperty = selectedProperty;
            SelectedProperty.ReservedDates = ReservedDateRepository.GetReservedDatesByPropertyId(SelectedProperty.Id);
            LoggedInGuest = guest;
            DateDataGrid.ItemsSource = this.AvailableDateRanges;
        }

        private void DatePicker_SelectedDate1Changed(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DatePicker datePicker)
            {
                StartDate = datePicker.SelectedDate ?? DateTime.Now;
            }
        }

        private void DatePicker_SelectedDate2Changed(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DatePicker datePicker)
            {
                EndDate = datePicker.SelectedDate ?? DateTime.Now;
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            AvailableDateRanges = new List<DateRange>();
            DateDataGrid.ItemsSource = this.AvailableDateRanges;
            if (PropertyReservation.Days < SelectedProperty.MinReservationDays)
            {
                MessageBox.Show("Minimum number of reservation days is " + SelectedProperty.MinReservationDays);
                return;
            }

            if (StartDate.Equals(DateTime.MinValue) || EndDate.Equals(DateTime.MinValue))
            {
                MessageBox.Show("Enter the date range!");
                return;

            }

            DateTime BellowStart = StartDate.AddDays(-10);
            DateTime AboveEnd = EndDate;
            FindAvailableDateRanges(StartDate, PropertyReservation.Days - 1, EndDate);

            if (AvailableDateRanges.Count() == 0)
            {
                MessageBox.Show("There are no available dates in this range, so other available dates are shown");

                //10 days bellow lower border
                FindAvailableDateRanges(BellowStart, 0, StartDate);

                //10 days above higher border
                FindAvailableDateRanges(AboveEnd, 0, EndDate.AddDays(10));
            }

            DateDataGrid.Items.Refresh();
        }

        private void FindAvailableDateRanges(DateTime start, int days, DateTime end)
        {
            while (start.AddDays(days) <= end)
            {
 
                if (!FindAvailableRange(start))
                {
                    start = start.AddDays(1);
                    continue;
                }

                DateRange dateRange = new DateRange();
                dateRange.Start = start;
                dateRange.End = start.AddDays(PropertyReservation.Days - 1);
                AvailableDateRanges.Add(dateRange);
                start = start.AddDays(1);
            }
        }

        private bool FindAvailableRange(DateTime start)
        {
            bool found = true;
            for (int i = 0; i < PropertyReservation.Days; i++)
            {
                DateTime currentDate = start.AddDays(i);
                if (SelectedProperty.ReservedDates.Find(r => r.Value == currentDate) != null)
                {
                    found = false;
                    break;
                }
            }

            return found;
        }

        private void ConfirmReservation_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            DateTime DateRangeStart = SelectedDateRange.Start;
            DateTime DateRangeEnd = SelectedDateRange.End;
            while (DateRangeStart <= DateRangeEnd)
            {
                ReservedDate = new ReservedDate
                {
                    PropertyId = SelectedProperty.Id,
                    Value = DateRangeStart
                };
                ReservedDateRepository.AddReservedDate(ReservedDate);
                SelectedProperty.ReservedDates.Add(ReservedDate);
                DateRangeStart = DateRangeStart.AddDays(1);
            }

            PropertyRepository.UpdateProperty(SelectedProperty);
            PropertyReservation.GuestId = LoggedInGuest.Id;
            PropertyReservation.GuestFirstName = LoggedInGuest.FirstName;
            PropertyReservation.GuestLastName = LoggedInGuest.LastName;
            PropertyReservation.StartDate = SelectedDateRange.Start;
            PropertyReservation.EndDate = SelectedDateRange.End;
            PropertyReservation.PropertyId = SelectedProperty.Id;
            PropertyReservationRepository.AddPropertyReservation(PropertyReservation.ToPropertyReservation());
            MessageBox.Show("Successfully reserved!");
            Close();
        }

        private bool ValidateInput()
        {
            if (PropertyReservation.Guests == 0)
            {
                MessageBox.Show("Enter the number of guests!");
                return false;
            }

            if (PropertyReservation.Guests > SelectedProperty.MaxGuests)
            {
                MessageBox.Show("Maximum number of guests is " + SelectedProperty.MaxGuests);
                return false;
            }

            if (SelectedDateRange == null)
            {
                MessageBox.Show("Select one date range");
                return false;
            }
            return true;
        }
    }
}
