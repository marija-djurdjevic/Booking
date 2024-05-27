using BookingApp.Aplication.Dto;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.Views.GuestView;
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
    public partial class PropertyBooking : Page
    {
        public static PropertyRepository PropertyRepository = new PropertyRepository();
        public static PropertyReservationRepository PropertyReservationRepository = new PropertyReservationRepository();
        public static ReservedDateRepository ReservedDateRepository = new ReservedDateRepository();
        public static GuestRepository GuestRepository = new GuestRepository();
        public static GuestNotificationsRepository GuestNotificationRepository = new GuestNotificationsRepository();
        public static RenovationRepository RenovationRepository = new RenovationRepository();
        public static List<Renovation> Renovations {  get; set; }
        public static List<DateTime> RenovationDates {  get; set; }
        public List<PropertyReservation> GuestsReservations { get; set; }
        public PropertyReservationDto PropertyReservation { get; set; }
        public ReservedDate ReservedDate { get; set; }
        public Property SelectedProperty { get; set; }
        public Guest LoggedInGuest { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateRange SelectedDateRange {  get; set; }
        public GuestNotification GuestNotification {  get; set; }
        public List<DateRange> AvailableDateRanges { get; set; }

        public PropertyBooking(Property selectedProperty, Guest guest, PropertyRepository propertyRepository, PropertyReservationRepository propertyReservationRepository)
        {
            InitializeComponent();
            DataContext = this;
            PropertyRepository = propertyRepository;
            PropertyReservationRepository = propertyReservationRepository;
            GuestRepository = new GuestRepository();
            ReservedDateRepository = new ReservedDateRepository();
            GuestNotificationRepository = new GuestNotificationsRepository();
            RenovationRepository = new RenovationRepository();
            GuestsReservations = new List<PropertyReservation>();
            Renovations = new List<Renovation>();
            RenovationDates = new List<DateTime>();
            AvailableDateRanges = new List<DateRange>();
            GuestNotification = new GuestNotification();
            SelectedProperty = selectedProperty;
            Renovations = RenovationRepository.GetAllRenovations().FindAll(r => r.PropertyId == selectedProperty.Id);
            GetRenovationsDates(Renovations);
            SelectedProperty.ReservedDates = ReservedDateRepository.GetReservedDatesByPropertyId(SelectedProperty.Id);
            LoggedInGuest = guest;
            PropertyReservation = new PropertyReservationDto
            {
               GuestId = LoggedInGuest.Id,
               GuestFirstName = LoggedInGuest.FirstName,
               GuestLastName = LoggedInGuest.LastName,
               PropertyId = SelectedProperty.Id,
               OwnerId = SelectedProperty.OwnerId
            };
            DateDataGrid.ItemsSource = this.AvailableDateRanges;
        }

        private void GetRenovationsDates(List<Renovation> Renovations)
        {
            foreach(Renovation renovation in Renovations)
            {
                DateTime Date = renovation.StartDate;
                while (Date <= renovation.EndDate)
                {
                    RenovationDates.Add(Date);
                    Date = Date.AddDays(1);
                }
                
            }
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

            if (!ValidateSearchInput())
                return;

            DateTime BellowStart = StartDate.AddDays(-100);
            DateTime AboveEnd = EndDate;
            FindAvailableDateRanges(StartDate, PropertyReservation.Days - 1, EndDate);

            if (AvailableDateRanges.Count() == 0)
            {
                MessageBox.Show("There are no available dates in this range, so other available dates are shown");

                FindAvailableDateRanges(BellowStart, 0, StartDate);

                FindAvailableDateRanges(AboveEnd, 0, EndDate.AddDays(100));
            }

            DateDataGrid.Items.Refresh();
        }

        private bool ValidateSearchInput()
        {
            if (StartDate.Equals(DateTime.MinValue) || EndDate.Equals(DateTime.MinValue))
            {
                MessageBox.Show("Enter the date range!");
                return false;

            }

            if (PropertyReservation.Days < SelectedProperty.MinReservationDays)
            {
                MessageBox.Show("Minimum number of reservation days is " + SelectedProperty.MinReservationDays);
                return false;
            }
            return true;
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
                if (SelectedProperty.ReservedDates.Find(r => r.Value == currentDate) != null || RenovationDates.Contains(currentDate) == true)
                {
                    found = false;
                    break;
                }
            }

            return found;
        }

        private void ConfirmReservation_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateConfirmationInput())
                return;

            DateTime DateRangeStart = SelectedDateRange.Start;
            DateTime DateRangeEnd = SelectedDateRange.End;
            while (DateRangeStart <= DateRangeEnd)
            {
                ReservedDate = new ReservedDate
                {
                    PropertyId = SelectedProperty.Id,
                    Value = DateRangeStart,
                    ReservationId = PropertyReservationRepository.NextId()
                };
                ReservedDateRepository.AddReservedDate(ReservedDate);
                SelectedProperty.ReservedDates.Add(ReservedDate);
                DateRangeStart = DateRangeStart.AddDays(1);
            }
  
            PropertyRepository.UpdateProperty(SelectedProperty);
            PropertyReservation.StartDate = SelectedDateRange.Start;
            PropertyReservation.EndDate = SelectedDateRange.End;
            PropertyReservation.PropertyName = SelectedProperty.Name;
            if(LoggedInGuest.IsSuperGuest)
            {
                UpdateSuperGuestPoints();
            }
            else
            {
                CheckSuperGuestStatus(LoggedInGuest, GuestsReservations);
            }
            GuestRepository.Update(LoggedInGuest);
            PropertyReservationRepository.AddPropertyReservation(PropertyReservation.ToPropertyReservation());
            MessageBox.Show("Successfully reserved!");
        }

        public void CheckSuperGuestStatus(Guest Guest, List<PropertyReservation> GuestsReservations)
        {
            GuestsReservations = PropertyReservationRepository.GetAll().FindAll(r => r.StartDate >= DateTime.Now.AddDays(-365) && r.GuestId == Guest.Id);
            if (GuestsReservations.Count() >= 10)
            {
                Guest.IsSuperGuest = true;
                Guest.SuperGuestStartDate = DateTime.Now;
                Guest.Points = 5;
                GuestNotification GuestNotification = new GuestNotification()
                {
                    GuestId = Guest.Id,
                    Message = "Congratulations! You have become a Super Guest. You have won 5 points that you can use as a discount on bookings in the next year!",
                    Read = false
                };
                GuestNotificationRepository.AddNotification(GuestNotification);
            }
            else
            {
                Guest.IsSuperGuest = false;
                Guest.Points = 0;
            }
            GuestRepository.Update(Guest);

        }


        public void UpdateSuperGuestPoints()
        {
            if (LoggedInGuest.Points > 0)
            {
                LoggedInGuest.Points -= 1;
                GuestNotification.GuestId = LoggedInGuest.Id;
                GuestNotification.Message = "You used the SuperGuest discount on your reservation, you still have " + LoggedInGuest.Points + " points left.";
                GuestNotification.Read = false;
                GuestNotificationRepository.AddNotification(GuestNotification);
            }
            
        }

        private bool ValidateConfirmationInput()
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
