using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.Repository
{
    public class PropertyReservationRepository
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

        public void Delete(int reservationId)
        {
            propertyReservations = GetAll();
            PropertyReservation propertyReservation = propertyReservations.FirstOrDefault(t => t.Id == reservationId);
            if (propertyReservation != null)
            {
                propertyReservations.Remove(propertyReservation);
                _serializer.ToCSV(FilePath, propertyReservations);
            }
        }

        private void SaveChanges()
        {
            _serializer.ToCSV(FilePath, propertyReservations);
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
