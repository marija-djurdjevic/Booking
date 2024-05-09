using BookingApp.Aplication;
using BookingApp.Aplication.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class ScheduleRenovationViewModel
    {
        private readonly PropertyService _propertyService;
        private readonly PropertyReservationService _propertyReservationService;
        private readonly RenovationService _renovationService;
        private readonly User _loggedInUser;

        public ObservableCollection<string> AllPropertyNames { get; set; }
        public ObservableCollection<DateRange> AvailableDateRanges { get; set; }
        public DateRange SelectedDateRange { get; set; }
        public ScheduleRenovationViewModel(User loggedInUser)
        {
            _loggedInUser = loggedInUser;
            _propertyService = new PropertyService(Injector.CreateInstance<IPropertyRepository>(), Injector.CreateInstance<IPropertyReservationRepository>());
            _propertyReservationService = new PropertyReservationService(Injector.CreateInstance<IPropertyRepository>(), Injector.CreateInstance<IPropertyReservationRepository>(), Injector.CreateInstance<IReservedDateRepository>());
            _renovationService = new RenovationService(Injector.CreateInstance<IRenovationRepository>());
            Initialize();
        }
        private void Initialize()
        {
            AllPropertyNames = new ObservableCollection<string>(_propertyService.GetAllPropertyNames());
        }

        public ObservableCollection<DateRange> SearchAvailableDateRanges(string selectedPropertyName, DateTime startDate, DateTime endDate, int duration)
        {
            ObservableCollection<DateRange> availableDateRanges = new ObservableCollection<DateRange>();

            Property selectedProperty = _propertyService.GetAllProperties().FirstOrDefault(property => property.Name == selectedPropertyName);
          


            availableDateRanges = new ObservableCollection<DateRange>(_propertyService.GetAvailableDateRanges(selectedProperty, startDate, endDate, duration));
            if (availableDateRanges.Count == 0)
            {
                MessageBox.Show("There are no available dates for renovation.");
                
            }
            return availableDateRanges;

           //DateDataGrid.ItemsSource = AvailableDateRanges;
        }
        public void SubmitRenovation(int ownerId, int propertyId, DateTime startDate, DateTime endDate, string description, int duration)
        {
            if (startDate == null || endDate == null || string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            Renovation newRenovation = new Renovation()
            {
                OwnerId = ownerId,
                PropertyId = propertyId,
                StartDate = startDate,
                EndDate = endDate,
                Description = description,
                Duration = duration
            };

            // Pozovite odgovarajuću metodu u servisu za čuvanje renovacije
            _renovationService.Save(newRenovation);

            MessageBox.Show("Renovation saved successfully.");
        }

    }
}
