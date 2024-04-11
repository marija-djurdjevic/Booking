using BookingApp.Model;
using BookingApp.Repository;
using System.Collections.Generic;

namespace BookingApp.Service
{
    internal class TouristService
    {
        private readonly TouristRepository _touristRepository;

        public TouristService()
        {
            _touristRepository = new TouristRepository();
        }



        public Tourist GetByUserId(int Id)
        {
            return _touristRepository.GetByUserId(Id);
        }

        public int GetAgeById(int touristId)
        {
            var tourist = GetByUserId(touristId);
            return tourist != null ? tourist.Age : -1;
        }
    }
}
