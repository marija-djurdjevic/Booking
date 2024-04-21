using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IReservationChangeRequestRepository
    {
        void AddReservationChangeRequest(ReservationChangeRequest _reservationChangeRequest);
        List<ReservationChangeRequest> GetAll();
        List<ReservationChangeRequest> GetReservationChangeRequestDataById(int id);
        void UpdateChangeRequestComment(int requestId, string comment);
        void UpdateChangeRequestStatus(int requestId, RequestStatus newStatus);
        int NextId();
    }
}
