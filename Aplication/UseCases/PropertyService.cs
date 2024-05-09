using BookingApp.Domain.Models;
using BookingApp.Repositories;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Aplication.UseCases
{
    public class PropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyReservationRepository _propertyReservationRepository;

        public PropertyService(IPropertyRepository _propertyRepository, IPropertyReservationRepository _propertyReservationRepository)
        {
            //_propertyRepository = new PropertyRepository();
            this._propertyRepository = _propertyRepository;
            this._propertyReservationRepository = _propertyReservationRepository;
        }
        public List<DateRange> GetAvailableDateRanges(Property selectedProperty, DateTime startDate, DateTime endDate, int duration)
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
        }

        public Property AddProperty(Property property)
        {
            return _propertyRepository.AddProperty(property);
        }

        public void DeleteProperty(int propertyId)
        {
            _propertyRepository.DeleteProperty(propertyId);
        }

        public List<Property> GetAllProperties()
        {
            return _propertyRepository.GetAllProperties();
        }

        public Property GetPropertyById(int propertyId)
        {
            return _propertyRepository.GetPropertyById(propertyId);
        }

        public int GetPropertyId(Property property)
        {
            return _propertyRepository.GetPropertyId(property);
        }

        public List<string> GetAllPropertyNames()
        {
            return _propertyRepository.GetAllPropertyNames();
        }

        public void SaveChanges()
        {
            _propertyRepository.SaveChanges();
        }

        public void UpdateProperty(Property updatedProperty)
        {
            _propertyRepository.UpdateProperty(updatedProperty);
        }
    }
}
