using BookingApp.Domain.Models;
using BookingApp.Repositories;
using BookingApp.Serializer;
using System.Collections.Generic;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Aplication.UseCases
{
    public class TouristService
    {
        private readonly ITouristRepository touristRepository;

        public TouristService(ITouristRepository touristRepository)
        {
            this.touristRepository = touristRepository;
        }

        public Tourist GetByUserId(int Id)
        {
            var tourists = touristRepository.GetAll();
            return tourists.Find(t => t.Id == Id);
        }

        public int GetAgeById(int touristId)
        {
            var tourist = GetByUserId(touristId);
            return tourist != null ? tourist.Age : -1;
        }
    }
}
