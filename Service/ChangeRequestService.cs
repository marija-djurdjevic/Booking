using BookingApp.Dto;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class ChangeRequestService
    {
        private readonly ReservationChangeRequestsRepository _repository;
        private ReservationChangeRequestsRepository ReservationChangeRequestsRepository;


        public ChangeRequestService()
        {
            _repository = new ReservationChangeRequestsRepository();
            ReservationChangeRequestsRepository = new ReservationChangeRequestsRepository();
        }

        public List<ReservationChangeRequest> GetAllRequests()
        {
            return _repository.GetAll();
        }
        public void UpdateChangeRequestStatus(int requestId, RequestStatus newStatus)
        {
            _repository.UpdateChangeRequestStatus(requestId, newStatus);
        }

        public ObservableCollection<ReservationChangeRequest> GetAllGuestsRequests(int GuestId)
        {
            return new ObservableCollection<ReservationChangeRequest>(ReservationChangeRequestsRepository.GetAll().FindAll(r => r.GuestId == GuestId));
        }

        public void SaveRequest(ReservationChangeRequestDto reservationChangeRequest)
        {
            ReservationChangeRequestsRepository.AddReservationChangeRequest(reservationChangeRequest.ToReservationChangeRequest());
        }

        public List<ReservationChangeRequest> UpdateGuestsRequests(int guestId)
        {
            return ReservationChangeRequestsRepository.GetAll().FindAll(r => r.GuestId == guestId);
        }

        public void UpdateChangeRequestComment(int requestId, string comment)
        {
            _repository.UpdateChangeRequestComment(requestId, comment);
        }

    }
}
