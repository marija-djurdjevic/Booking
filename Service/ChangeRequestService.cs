using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class ChangeRequestService
    {
        private readonly ReservationChangeRequestsRepository _repository;

        public ChangeRequestService()
        {
            _repository = new ReservationChangeRequestsRepository();
        }

        public List<ReservationChangeRequest> GetAllRequests()
        {
            return _repository.GetAll();
        }
        public void UpdateChangeRequestStatus(int requestId, RequestStatus newStatus)
        {
            _repository.UpdateChangeRequestStatus(requestId, newStatus);
        }
        public void UpdateChangeRequestComment(int requestId, string comment)
        {
            _repository.UpdateChangeRequestComment(requestId, comment);
        }
    }
}
