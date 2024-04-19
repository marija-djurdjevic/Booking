using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.Repositories
{
    public class TourReservationRepository : ITourReservationRepository
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
        public List<TourReservation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public TourReservation GetById(int reservationId)
        {
            tourReservations = GetAll();
            return tourReservations.FirstOrDefault(t => t.Id == reservationId);
        }

        public void Save(TourReservation _reservationData)
        {
            tourReservations = GetAll();
            _reservationData.Id = NextId();
            tourReservations.Add(_reservationData);
            _serializer.ToCSV(FilePath, tourReservations);

        }

        public void Update(TourReservation updatedReservation)
        {
            tourReservations = GetAll();
            TourReservation existingReservation = tourReservations.FirstOrDefault(t => t.Id == updatedReservation.Id);
            if (existingReservation != null)
            {
                int index = tourReservations.IndexOf(existingReservation);
                tourReservations[index] = updatedReservation;
                _serializer.ToCSV(FilePath, tourReservations);
            }
        }

        public void Delete(int reservationId)
        {
            tourReservations = GetAll();
            TourReservation existingReservation = tourReservations.FirstOrDefault(t => t.Id == reservationId);
            if (existingReservation != null)
            {
                tourReservations.Remove(existingReservation);
                _serializer.ToCSV(FilePath, tourReservations);
            }
        }

        public int NextId()
        {
            tourReservations = _serializer.FromCSV(FilePath);
            if (tourReservations.Count < 1)
            {
                return 1;
            }
            return tourReservations.Max(t => t.Id) + 1;
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

        public void DeleteByTourId(int tourId)
        {
            tourReservations.RemoveAll(tr => tr.TourId == tourId);
            _serializer.ToCSV(FilePath, tourReservations);
        }
    }
}
