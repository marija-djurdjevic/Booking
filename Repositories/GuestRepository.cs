using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repositories
{
    public class GuestRepository : IGuestRepository
    {
        private const string FilePath = "../../../Resources/Data/guests.csv";

        private readonly Serializer<Guest> _serializer;

        private List<Guest> guests;

        public GuestRepository()
        {
            _serializer = new Serializer<Guest>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            guests = _serializer.FromCSV(FilePath);
        }

        public void Save(Guest guest)
        {
            guests = GetAll();
            guests.Add(guest);
            _serializer.ToCSV(FilePath, guests);
        }

        public List<Guest> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Update(Guest updatedguest)
        {
            Guest existingGuest = guests.FirstOrDefault(t => t.Id == updatedguest.Id);
            if (existingGuest != null)
            {
                int index = guests.IndexOf(existingGuest);
                guests[index] = updatedguest;
                _serializer.ToCSV(FilePath, guests);
            }
        }

        public Guest GetByUserId(int Id)
        {
            guests = _serializer.FromCSV(FilePath);
            return guests.Find(t => t.Id == Id);
        }
    }
}
