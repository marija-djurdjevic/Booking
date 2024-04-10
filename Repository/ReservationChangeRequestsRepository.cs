using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class ReservationChangeRequestsRepository
    {
        private const string FilePath = "../../../Resources/Data/reservationChangeRequests.csv";

        private readonly Serializer<ReservationChangeRequest> _serializer;

        private List<ReservationChangeRequest> reservationChangeRequests;

        public ReservationChangeRequestsRepository()
        {
            _serializer = new Serializer<ReservationChangeRequest>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            reservationChangeRequests = _serializer.FromCSV(FilePath);
        }

        public void AddReservationChangeRequest(ReservationChangeRequest _reservationChangeRequest)
        {
            int nextId = NextId();
            _reservationChangeRequest.Id = nextId;
            reservationChangeRequests.Add(_reservationChangeRequest);
            _serializer.ToCSV(FilePath, reservationChangeRequests);
        }

        public List<ReservationChangeRequest> GetAll()
        {
            return reservationChangeRequests;
        }

        public List<ReservationChangeRequest> GetReservationChangeRequestDataById(int id)
        {
            return reservationChangeRequests.FindAll(t => t.Id == id);
        }

        public void Delete(int Id)
        {
            reservationChangeRequests = GetAll();
            ReservationChangeRequest reservationChangeRequest = reservationChangeRequests.FirstOrDefault(t => t.Id == Id);
            if (reservationChangeRequest != null)
            {
                reservationChangeRequests.Remove(reservationChangeRequest);
                _serializer.ToCSV(FilePath, reservationChangeRequests);
            }
        }

        private void SaveChanges()
        {
            _serializer.ToCSV(FilePath, reservationChangeRequests);
        }

        public int NextId()
        {
            if (reservationChangeRequests.Count < 1)
            {
                return 1;
            }
            return reservationChangeRequests.Max(t => t.Id) + 1;
        }

    }
}
