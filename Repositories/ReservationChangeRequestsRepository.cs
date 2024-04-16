using BookingApp.Domain.Models;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repositories
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

        public void UpdateChangeRequestComment(int requestId, string comment)
        {
            reservationChangeRequests = GetAll();
            ReservationChangeRequest changeRequest = reservationChangeRequests.FirstOrDefault(t => t.Id == requestId);
            if (changeRequest != null)
            {
                changeRequest.Comment = comment;
                _serializer.ToCSV(FilePath, reservationChangeRequests);
            }
        }
        public void UpdateChangeRequestStatus(int requestId, RequestStatus newStatus)
        {
            reservationChangeRequests = GetAll();
            ReservationChangeRequest changeRequest = reservationChangeRequests.FirstOrDefault(t => t.Id == requestId);
            if (changeRequest != null)
            {
                changeRequest.RequestStatus = newStatus;
                _serializer.ToCSV(FilePath, reservationChangeRequests);
            }
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
