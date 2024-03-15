using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class TourReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/tourReservation.csv";

        private readonly Serializer<TourReservation> _serializer;

        private List<TourReservation> reservationData;

        public TourReservationRepository()
        {
            _serializer = new Serializer<TourReservation>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            reservationData = _serializer.FromCSV(FilePath);
        }

        public void Save(TourReservation _reservationData)
        {
            reservationData = GetAll();
            reservationData.Add(_reservationData);
            _serializer.ToCSV(FilePath, reservationData);

        }


        public void SaveChanges()
        {
            _serializer.ToCSV(FilePath, reservationData);
        }

        public List<TourReservation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public List<TourReservation> GetByTourId(int tourId)
        {
            reservationData = _serializer.FromCSV(FilePath);
            return reservationData.FindAll(t => t.TourId == tourId);
        }

        public List<TourReservation> GetByUserId(int userId)
        {
            reservationData = _serializer.FromCSV(FilePath);
            return reservationData.FindAll(t => t.UserId == userId);
        }


        public List<TourReservation> GetUncheckedByTourId(int tourId)
        {
            var reservationData = _serializer.FromCSV(FilePath);
            return reservationData.FindAll(t => t.TourId == tourId && t.IsOnTour==false);
        }


        public void Saveee(TourReservation _reservationData)
        {
            reservationData = GetAll();
            var existingReservation = reservationData.FirstOrDefault(r => r.TourId == _reservationData.TourId && r.TouristFirstName == _reservationData.TouristFirstName && r.TouristLastName == _reservationData.TouristLastName);
            if (existingReservation != null)
            {
                existingReservation.IsOnTour = _reservationData.IsOnTour;
                existingReservation.JoinedKeyPoint = _reservationData.JoinedKeyPoint;
            }
            else
            {
                reservationData.Add(_reservationData);
            }
            _serializer.ToCSV(FilePath, reservationData);
        }



    }
}
