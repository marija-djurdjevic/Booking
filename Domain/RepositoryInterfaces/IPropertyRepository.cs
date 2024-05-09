using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IPropertyRepository
    {
        List<Property> GetAllProperties();
        public List<string> GetAllPropertyNames();
        void SaveChanges();
        int NextId();
        Property GetPropertyById(int propertyId);
        int GetPropertyId(Property property);
        void DeleteProperty(int propertyId);
        void UpdateProperty(Property updatedProperty);
        Property AddProperty(Property property);

    }
}
