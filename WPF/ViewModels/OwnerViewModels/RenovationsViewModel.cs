using BookingApp.Domain.Models;
using BookingApp.Aplication.UseCases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Aplication;
using BookingApp.Aplication.UseCases;
using System.Windows.Input;
using BookingApp.Command;


namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class RenovationsViewModel
    {
        public User LoggedInUser { get; set; }
        private readonly RenovationService _renovationService; 
        private readonly User _loggedInUser;

        public ObservableCollection<Renovation> Renovations { get; set; }
        private readonly PropertyService _propertyService;

        public RenovationsViewModel() { }

        public RenovationsViewModel(User loggedInUser)
        {
            _loggedInUser = loggedInUser;
            _renovationService = new RenovationService(Injector.CreateInstance<IRenovationRepository>()); 
            _propertyService = new PropertyService(Injector.CreateInstance<IPropertyRepository>(), Injector.CreateInstance<IPropertyReservationRepository>()); 
            Renovations = new ObservableCollection<Renovation>();
            LoadRenovations();
        }

        public void DeleteRenovation(Renovation renovation)
        {
            if (renovation.StartDate >= DateTime.Today.AddDays(5))
            {
                _renovationService.DeleteRenovation(renovation.Id); 
                Renovations.Remove(renovation);
            }
        }

        private void LoadRenovations()
        {
            Renovations.Clear();
            var userRenovations = _renovationService.GetAllRenovations().FindAll(r => r.OwnerId == _loggedInUser.Id);
            var allProperties = _propertyService.GetAllProperties();
            foreach (var renovation in userRenovations)
            {
                var property = allProperties.FirstOrDefault(p => p.Id == renovation.PropertyId);
                if (property != null)
                {
                    renovation.PropertyName = property.Name;
                }

                Renovations.Add(renovation);
            }
        }
    }
}
