using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _propertyReservation.PropertyReservationId = nextId;
            propertyReservations.Add(_propertyReservation);
            _serializer.ToCSV(FilePath, propertyReservations);
        }

        public List<PropertyReservation> GetAllPropertyReservation()
        {
            return propertyReservations;
        }

        public List<PropertyReservation> GetReservationDataByTourId(int propertyReservationId)
        {
            return propertyReservations.FindAll(t => t.PropertyReservationId == propertyReservationId);
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
            return propertyReservations.Max(t => t.PropertyReservationId) + 1;
        }

    }
}
