using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class PropertyRepository
    {
        private const string FilePath = "../../../Resources/Data/properties.csv";

        private readonly Serializer<Property> _serializer;

        private List<Property> _properties;

        public int idd;

        public PropertyRepository()
        {
            _serializer = new Serializer<Property>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            _properties = _serializer.FromCSV(FilePath);
        }

        public Property AddProperty(Property property)
        {
            int nextId = NextId();
            property.Id = nextId;
            _properties.Add(property);
            _serializer.ToCSV(FilePath, _properties);
            return property;
        }

        public void UpdateProperty(Property updatedProperty)
        {
            Property existingProperty = _properties.FirstOrDefault(t => t.Id == updatedProperty.Id);
            if (existingProperty != null)
            {
                int index = _properties.IndexOf(existingProperty);
                _properties[index] = updatedProperty;
                _serializer.ToCSV(FilePath, _properties);
            }
        }

        public void DeleteProperty(int propertyId)
        {
            Property existingProperty = _properties.FirstOrDefault(t => t.Id == propertyId);
            if (existingProperty != null)
            {
                _properties.Remove(existingProperty);
                _serializer.ToCSV(FilePath, _properties);
            }
        }

        public List<Property> GetAllProperties()
        {
            return _properties;
        }

        public Property GetPropertyById(int propertyId)
        {
            return _properties.FirstOrDefault(t => t.Id == propertyId);
        }


        private void SaveChanges()
        {
            _serializer.ToCSV(FilePath, _properties);
        }

        public int NextId()
        {
            if (_properties.Count < 1)
            {
                return 1;
            }
            return _properties.Max(t => t.Id) + 1;
        }


        public int GetPropertyId(Property property)
        {
            return property.Id;
        }
    }
}

