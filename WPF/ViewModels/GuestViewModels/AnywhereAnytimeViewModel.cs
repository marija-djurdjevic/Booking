using BookingApp.Aplication;
using BookingApp.Aplication.Dto;
using BookingApp.Aplication.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;

namespace BookingApp.WPF.ViewModels.GuestViewModels
{
    public class AnywhereAnytimeViewModel : INotifyPropertyChanged
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public PropertyReservationDto PropertyReservation { get; set; }
        public Property SelectedProperty { get; set; }
        public Guest LoggedInGuest { get; set; }
        public List<DateRange> AvailableDateRanges { get; set; }
        private DateRange selectedDateRange;
        public DateRange SelectedDateRange
        {
            get { return selectedDateRange; }
            set
            {
                if (selectedDateRange != value)
                {
                    selectedDateRange = value;
                    OnPropertyChanged(nameof(SelectedDateRange));
                }
            }
        }
        public ReservedDate ReservedDate { get; set; }
        public List<PropertyReservation> GuestsReservations { get; set; }
        public GuestNotification GuestNotification { get; set; }
        public ObservableCollection<KeyValuePair<Property, List<DateRange>>> PropertiesDateRanges { get; set; }
        public AnywhereAnytimeService AnyAnyService;

        public AnywhereAnytimeViewModel(Guest guest) 
        {
            LoggedInGuest = guest;
            AnyAnyService = new AnywhereAnytimeService(Injector.CreateInstance<IPropertyRepository>(), Injector.CreateInstance<IPropertyReservationRepository>(), Injector.CreateInstance<IReservedDateRepository>(), Injector.CreateInstance<IGuestRepository>(), Injector.CreateInstance<IGuestNotificationRepository>(), Injector.CreateInstance<IRenovationRepository>());
            SelectedProperty = new Property();
            ReservedDate = new ReservedDate();
            SelectedDateRange = new DateRange();
            GuestNotification = new GuestNotification();
            AvailableDateRanges = new List<DateRange>();
            GuestsReservations = new List<PropertyReservation>();
            PropertiesDateRanges = new ObservableCollection<KeyValuePair<Property, List<DateRange>>>();
            PropertyReservation = new PropertyReservationDto
            {
                GuestId = LoggedInGuest.Id,
                GuestFirstName = LoggedInGuest.FirstName,
                GuestLastName = LoggedInGuest.LastName
            };
        }

        private List<DateTime> GetRenovationsDates(Property CurrentProperty)
        {
            List<Renovation> Renovations = new List<Renovation>();
            List<DateTime> RenovationDates = new List<DateTime>();
            Renovations = AnyAnyService.GetAllRenovations().FindAll(r => r.PropertyId == CurrentProperty.Id);
            foreach (Renovation renovation in Renovations)
            {
                DateTime Date = renovation.StartDate;
                while (Date <= renovation.EndDate)
                {
                    RenovationDates.Add(Date);
                    Date = Date.AddDays(1);
                }
            }

            return RenovationDates;
        }

        public void SetSelectedItems(DateRange dateRange)
        {
            SelectedDateRange = dateRange;
            SelectedProperty = AnyAnyService.GetById(SelectedDateRange.PropertyId);
        }

        public void SetStartDate(object sender)
        {
            if (sender is DatePicker datePicker)
            {
                StartDate = datePicker.SelectedDate ?? DateTime.Now;

            }
        }

        public void SetEndDate(object sender)
        {
            if (sender is DatePicker datePicker)
            {
                EndDate = datePicker.SelectedDate ?? DateTime.Now;

            }
        }

        public void SearchProperties()
        {
            PropertiesDateRanges.Clear();
            GetPropertyDatesPairs();
        }

        public int ValidateSearchInput()
        {
            if (PropertyReservation.Guests == 0)
            {
                return -1;
            }
            if (PropertyReservation.Days == 0)
            {
                return 0;
            }

            return 1;
        }

        private void GetPropertyDatesPairs()
        {
            foreach (Property property in AnyAnyService.GetAllProperties())
            {
                List<DateRange> dateRanges = new List<DateRange>();
                if (StartDate < DateTime.Now)
                {
                    StartDate = DateTime.Today;
                }
                if (EndDate < DateTime.Now)
                {
                    EndDate = DateTime.Today.AddDays(100);
                }
                dateRanges = FindAvailableDateRanges(StartDate, PropertyReservation.Days - 1, EndDate, GetRenovationsDates(property), property);
                if (dateRanges.Count > 0)
                {
                    var propertyDates = new KeyValuePair<Property, List<DateRange>>(property, dateRanges);
                    PropertiesDateRanges.Add(propertyDates);
                }
            }
        }

        private List<DateRange> FindAvailableDateRanges(DateTime start, int days, DateTime end, List<DateTime> RenovationDates, Property property)
        {
            AvailableDateRanges.Clear();
            while (start.AddDays(days) <= end)
            {

                if (!FindAvailableRange(start, RenovationDates, property))
                {
                    start = start.AddDays(1);
                    continue;
                }

                DateRange dateRange = new DateRange();
                dateRange.Start = start;
                dateRange.End = start.AddDays(PropertyReservation.Days - 1);
                dateRange.PropertyId = property.Id;
                AvailableDateRanges.Add(dateRange);
                start = start.AddDays(1);
            }

            return AvailableDateRanges;
        }

        private bool FindAvailableRange(DateTime start, List<DateTime> RenovationDates, Property CurrentProperty)
        {
            bool found = true;
            for (int i = 0; i < PropertyReservation.Days; i++)
            {
                DateTime currentDate = start.AddDays(i);
                CurrentProperty.ReservedDates = AnyAnyService.GetReservedDateById(CurrentProperty.Id);
                if (CurrentProperty.ReservedDates.Find(r => r.Value == currentDate) != null || RenovationDates.Contains(currentDate) == true)
                {
                    found = false;
                    break;
                }
            }

            return found;
        }

        public void CheckSuperGuestStatus(Guest Guest, List<PropertyReservation> GuestsReservations)
        {
            GuestsReservations = AnyAnyService.GetAllPropertyReservations().FindAll(r => r.StartDate >= DateTime.Now.AddDays(-365) && r.GuestId == Guest.Id);
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
                AnyAnyService.AddNotification(GuestNotification);
            }
            else
            {
                Guest.IsSuperGuest = false;
                Guest.Points = 0;
            }
            AnyAnyService.UpdateGuest(Guest);
        }

        public bool Confirm_Click()
        {
            if(SelectedDateRange.Start < DateTime.Today)
            {
                return false;
            }
            DateTime DateRangeStart = SelectedDateRange.Start;
            DateTime DateRangeEnd = SelectedDateRange.End;
            while (DateRangeStart <= DateRangeEnd)
            {
                ReservedDate = new ReservedDate
                {
                    PropertyId = SelectedProperty.Id,
                    Value = DateRangeStart,
                    ReservationId = AnyAnyService.GetNextIdPR()
                };
                AnyAnyService.AddReservedDate(ReservedDate);
                SelectedProperty.ReservedDates.Add(ReservedDate);
                DateRangeStart = DateRangeStart.AddDays(1);
            }

            AnyAnyService.UpdateProperty(SelectedProperty);
            PropertyReservation.StartDate = SelectedDateRange.Start;
            PropertyReservation.EndDate = SelectedDateRange.End;
            PropertyReservation.PropertyName = SelectedProperty.Name;
            PropertyReservation.PropertyId = SelectedProperty.Id;
            PropertyReservation.OwnerId = SelectedProperty.OwnerId;
            if (LoggedInGuest.IsSuperGuest)
            {
                UpdateSuperGuestPoints();
            }
            else
            {
                CheckSuperGuestStatus(LoggedInGuest, GuestsReservations);
            }
            AnyAnyService.UpdateGuest(LoggedInGuest);
            AnyAnyService.AddPropertyReservation(PropertyReservation.ToPropertyReservation());
            return true;
        }

        public void UpdateSuperGuestPoints()
        {
            if (LoggedInGuest.Points > 0)
            {
                LoggedInGuest.Points -= 1;
                GuestNotification.GuestId = LoggedInGuest.Id;
                GuestNotification.Message = "You used the SuperGuest discount on your reservation, you still have " + LoggedInGuest.Points + " points left.";
                GuestNotification.Read = false;
                AnyAnyService.AddNotification(GuestNotification);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
