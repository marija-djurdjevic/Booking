using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class ReservationDataRepository
    {
        private const string FilePath = "../../../Resources/Data/reservationData.csv";

        private readonly Serializer<ReservationData> _serializer;

        private List<ReservationData> reservationData;

        public ReservationDataRepository()
        {
            _serializer = new Serializer<ReservationData>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            reservationData = _serializer.FromCSV(FilePath);
        }

        public void AddReservationData(ReservationData _reservationData)
        {
            reservationData.Add(_reservationData);
            _serializer.ToCSV(FilePath, reservationData);
        }

        public List<ReservationData> GetAllReservationData()
        {
            return reservationData;
        }

        public List<ReservationData> GetReservationDataByTourId(int tourId)
        {
            return reservationData.FindAll(t => t.TourId == tourId);
        }


        private void SaveChanges()
        {
            _serializer.ToCSV(FilePath, reservationData);
        }

    }
}
