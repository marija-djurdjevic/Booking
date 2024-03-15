using BookingApp.Dto;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookingApp.Guest
{
    /// <summary>
    /// Interaction logic for PropertyBooking.xaml
    /// </summary>
    public partial class PropertyBooking : Window
    {
        public static PropertyRepository PropertyRepository = new PropertyRepository();
        //public static ReservedDateRepository ReservedDateRepository = new ReservedDateRepository();
        public static PropertyReservationRepository PropertyReservationRepository = new PropertyReservationRepository();
        public PropertyReservationDto PropertyReservation { get; set; }
        public Property SelectedProperty { get; set; }
        public User LoggedInUser { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateRange SelectedDateRange {  get; set; }
        public List<DateRange> AvailableDateRanges { get; set; }

        public PropertyBooking(Property selectedProperty, User loggedInUser)
        {
            InitializeComponent();
            DataContext = this;
            PropertyReservation = new PropertyReservationDto();
            AvailableDateRanges = new List<DateRange>();
            SelectedProperty = selectedProperty;
            LoggedInUser = loggedInUser;
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
            if(PropertyReservation.Days < SelectedProperty.MinReservationDays)
            {
                MessageBox.Show("Minimum number of reservation days is " + SelectedProperty.MinReservationDays);
                return;
            }

            if(StartDate.Equals(DateTime.MinValue) || EndDate.Equals(DateTime.MinValue))
            {
                MessageBox.Show("Enter the date range!");
                return;

            }

            SelectedProperty.ReservedDates.Add(StartDate);
            while (StartDate.AddDays(PropertyReservation.Days) <= EndDate)
            {
                bool found = true;
                for (int i = 0; i < PropertyReservation.Days; i++)
                {
                    DateTime currentDate = StartDate.AddDays(i);
                    if (SelectedProperty.ReservedDates.Contains(currentDate))
                        found = false;
                    break;
                }

                if (!found)
                {
                    StartDate = StartDate.AddDays(1);
                    continue;
                }
            
                DateRange dateRange = new DateRange(); 
                dateRange.Start = StartDate;
                dateRange.End = StartDate.AddDays(PropertyReservation.Days -1);
                AvailableDateRanges.Add(dateRange);
                StartDate = StartDate.AddDays(1);

            }

            DateDataGrid.Items.Refresh();
        }

        private void ConfirmReservation_Click(object sender, RoutedEventArgs e)
        {
            if(PropertyReservation.Guests == 0)
            {
                MessageBox.Show("Enter the number of guests!");
                return;
            }

            if(PropertyReservation.Guests > SelectedProperty.MaxGuests)
            {
                MessageBox.Show("Maximum number of guests is " + SelectedProperty.MaxGuests);
                return;
            }

            if(SelectedDateRange == null)
            {
                MessageBox.Show("Select one date range");
                return;
            }

            DateTime StartDateRange = SelectedDateRange.Start;
            DateTime EndDateRange = SelectedDateRange.End;
            while (StartDateRange <= EndDateRange)
            {
                SelectedProperty.ReservedDates.Add(StartDateRange);
                StartDateRange = StartDateRange.AddDays(1);

            }

            PropertyRepository.UpdateProperty(SelectedProperty);
            PropertyReservation.StartDate = SelectedDateRange.Start;
            PropertyReservation.EndDate = SelectedDateRange.End;
            PropertyReservationRepository.AddPropertyReservation(PropertyReservation.ToPropertyReservation());

        }
    }

    
}
