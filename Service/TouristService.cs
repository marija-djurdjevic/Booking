﻿using BookingApp.Model;
using BookingApp.Repository;
using System.Collections.Generic;

namespace BookingApp.Service
{
    public class TouristService
    {
        private readonly TouristRepository touristRepository;

        public TouristService()
        {
            touristRepository = new TouristRepository();
        }



        public Tourist GetByUserId(int Id)
        {
            return touristRepository.GetByUserId(Id);
        }

        public int GetAgeById(int touristId)
        {
            var tourist = GetByUserId(touristId);
            return tourist != null ? tourist.Age : -1;
        }
    }
}
