using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.Repositories
{
    public class PropertyReservationRepository : IPropertyReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/propertyReservations.csv";

        private readonly Serializer<PropertyReservation> _serializer;

        private List<PropertyReservation> propertyReservations;

        public PropertyReservationRepository()
        {
            _serializer = new Serializer<PropertyReservation>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            propertyReservations = _serializer.FromCSV(FilePath);
        }

        public List<PropertyReservation> GetAllPropertyReservationsByPropertyId(int propertyId)
        {
            return propertyReservations.Where(pr => pr.PropertyId == propertyId).ToList();
        }
        public void AddPropertyReservation(PropertyReservation _propertyReservation)
        {
            int nextId = NextId();
            _propertyReservation.Id = nextId;
            propertyReservations.Add(_propertyReservation);
            _serializer.ToCSV(FilePath, propertyReservations);
        }

        public List<PropertyReservation> GetAll()
        {
            return propertyReservations;
        }

        public List<PropertyReservation> GetReservationDataById(int id)
        {
            return propertyReservations.FindAll(t => t.Id == id);
        }

        public void Update(PropertyReservation updatedPropertyReservation)
        {
            PropertyReservation existingPropertyReservation = propertyReservations.FirstOrDefault(t => t.Id == updatedPropertyReservation.Id);
            if (existingPropertyReservation != null)
            {
                int index = propertyReservations.IndexOf(existingPropertyReservation);
                propertyReservations[index] = existingPropertyReservation;
                _serializer.ToCSV(FilePath, propertyReservations);
            }
        }

        public PropertyReservation GetReservationById(int id)
        {
            return propertyReservations.FirstOrDefault(r => r.Id == id);
        }
        public void UpdatePropertyReservation(PropertyReservation updatedReservation)
        {
            propertyReservations = GetAll();
            PropertyReservation existingReservation = propertyReservations.FirstOrDefault(r => r.Id == updatedReservation.Id);

            if (existingReservation != null)
            {
                existingReservation.StartDate = updatedReservation.StartDate;
                existingReservation.EndDate = updatedReservation.EndDate;


                _serializer.ToCSV(FilePath, propertyReservations);
            }
            else
            {
            }
        }

        public int NextId()
        {
            if (propertyReservations.Count < 1)
            {
                return 1;
            }
            return propertyReservations.Max(t => t.Id) + 1;
        }

    }
}
