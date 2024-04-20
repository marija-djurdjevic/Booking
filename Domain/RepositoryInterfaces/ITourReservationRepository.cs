using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ITourReservationRepository
    {
        List<TourReservation> GetAll();
        void Save(TourReservation tourReservation);
        int NextId();
        void Delete(int id);
        void Update(TourReservation updatedReservation);
        TourReservation GetById(int id);
        void DeleteByTourId(int tourId);
        public void UpdateReservation(TourReservation _reservationData);
    }
}
