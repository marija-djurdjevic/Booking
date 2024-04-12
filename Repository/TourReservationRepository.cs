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

        private List<TourReservation> tourReservations;

        public TourReservationRepository()
        {
            _serializer = new Serializer<TourReservation>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            tourReservations = _serializer.FromCSV(FilePath);
        }

        public void Save(TourReservation _reservationData)
        {
            tourReservations = GetAll();
            tourReservations.Add(_reservationData);
            _serializer.ToCSV(FilePath, tourReservations);

        }


        public void SaveChanges()
        {
            _serializer.ToCSV(FilePath, tourReservations);
        }

        public List<TourReservation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public List<TourReservation> GetByTourId(int tourId)
        {
            tourReservations = GetAll();
            return tourReservations.FindAll(t => t.TourId == tourId);
        }

        public List<TourReservation> GetByUserId(int userId)
        {
            tourReservations = GetAll();
            return tourReservations.FindAll(t => t.UserId == userId);
        }


        public List<TourReservation> GetUncheckedByTourId(int tourId)
        {
            var reservationData = _serializer.FromCSV(FilePath);
            return reservationData.FindAll(t => t.TourId == tourId && t.IsOnTour==false);
        }


        public void UpdateReservation(TourReservation _reservationData)
        {
            tourReservations = GetAll();
            var existingReservation = tourReservations.FirstOrDefault(r => r.TourId == _reservationData.TourId && r.TouristFirstName == _reservationData.TouristFirstName && r.TouristLastName == _reservationData.TouristLastName);
            if (existingReservation != null)
            {
                existingReservation.IsOnTour = _reservationData.IsOnTour;
                existingReservation.JoinedKeyPoint = _reservationData.JoinedKeyPoint;
            }
            else
            {
                tourReservations.Add(_reservationData);
            }
            _serializer.ToCSV(FilePath, tourReservations);
        }

        public List<TourReservation> GetFinishedReservationsAttendedByUser(int userId)
        {
            tourReservations = GetByUserId(userId);
            return tourReservations.FindAll(t => !t.IsOnTour && !t.JoinedKeyPoint.Name.Equals(""));
        }
        public void DeleteByTourId(int tourId)
        {
            tourReservations.RemoveAll(tr => tr.TourId == tourId);
            SaveChanges();
        }

        public bool IsUserOnTour(int userId,int tourId)
        {
            var userReservations = GetByUserId(userId);
            return userReservations.Any(r => r.TourId == tourId && r.IsOnTour && r.IsUser);
        }
    }
}
