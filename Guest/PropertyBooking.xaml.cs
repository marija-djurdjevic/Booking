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
        public static PropertyReservationRepository PropertyReservationRepository = new PropertyReservationRepository();
        public static ReservedDateRepository ReservedDateRepository = new ReservedDateRepository();

        public PropertyReservationDto PropertyReservation { get; set; }
        public ReservedDate ReservedDate { get; set; }
        public Property SelectedProperty { get; set; }
        public User LoggedInUser { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateRange SelectedDateRange {  get; set; }
        public List<DateRange> AvailableDateRanges { get; set; }

        public PropertyBooking(Property selectedProperty, User loggedInUser, PropertyRepository propertyRepository, PropertyReservationRepository propertyReservationRepository)
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
            AvailableDateRanges = new List<DateRange>();
            DateDataGrid.ItemsSource = this.AvailableDateRanges;
            if (PropertyReservation.Days < SelectedProperty.MinReservationDays)
            {
                MessageBox.Show("Minimum number of reservation days is " + SelectedProperty.MinReservationDays);
                return;
            }

            if(StartDate.Equals(DateTime.MinValue) || EndDate.Equals(DateTime.MinValue))
            {
                MessageBox.Show("Enter the date range!");
                return;

            }

            DateTime BellowStart = StartDate.AddDays(-10);
            DateTime AboveEnd = EndDate;
            while (StartDate.AddDays(PropertyReservation.Days -1) <= EndDate)
            {
                bool found = true;
                for (int i = 0; i < PropertyReservation.Days; i++)
                {
                    DateTime currentDate = StartDate.AddDays(i);
                    if (SelectedProperty.ReservedDates.Find(r =>  r.Value == currentDate) != null)
                    {
                        found = false;
                        break;
                    }
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

            if(AvailableDateRanges.Count() == 0)
            {
                MessageBox.Show("There are no available dates in this range, so other available dates are shown");
                //10 days bellow lower border
                while (BellowStart <= StartDate)
                {
                    bool found = true;
                    for (int i = 0; i < PropertyReservation.Days; i++)
                    {
                        DateTime currentDate = BellowStart.AddDays(i);
                        if (SelectedProperty.ReservedDates.Find(r => r.Value == currentDate) != null)
                        {
                            found = false;
                            break;
                        }
                    }

                    if (!found)
                    {
                        BellowStart = BellowStart.AddDays(1);
                        continue;
                    }

                    DateRange dateRange = new DateRange();
                    dateRange.Start = BellowStart;
                    dateRange.End = BellowStart.AddDays(PropertyReservation.Days - 1);
                    AvailableDateRanges.Add(dateRange);
                    BellowStart = BellowStart.AddDays(1);
                }

                //10 days above higher border
                while (AboveEnd <= EndDate.AddDays(10))
                {
                    bool found = true;
                    for (int i = 0; i < PropertyReservation.Days; i++)
                    {
                        DateTime currentDate = AboveEnd.AddDays(i);
                        if (SelectedProperty.ReservedDates.Find(r => r.Value == currentDate) != null)
                        {
                            found = false;
                            break;
                        }
                    }

                    if (!found)
                    {
                        AboveEnd = AboveEnd.AddDays(1);
                        continue;
                    }

                    DateRange dateRange = new DateRange();
                    dateRange.Start = AboveEnd;
                    dateRange.End = AboveEnd.AddDays(PropertyReservation.Days - 1);
                    AvailableDateRanges.Add(dateRange);
                    AboveEnd = AboveEnd.AddDays(1);
                }

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
                ReservedDate = new ReservedDate();
                ReservedDate.PropertyId = SelectedProperty.Id;
                ReservedDate.Value = StartDateRange;
                ReservedDateRepository.AddReservedDate(ReservedDate);
                SelectedProperty.ReservedDates.Add(ReservedDate);
                StartDateRange = StartDateRange.AddDays(1);
            }

            PropertyRepository.UpdateProperty(SelectedProperty);
            PropertyReservation.StartDate = SelectedDateRange.Start;
            PropertyReservation.EndDate = SelectedDateRange.End;
            PropertyReservation.PropertyId = SelectedProperty.Id;
            PropertyReservationRepository.AddPropertyReservation(PropertyReservation.ToPropertyReservation());
            MessageBox.Show("Successfully reserved!");
            Close();
        }
    }
}
