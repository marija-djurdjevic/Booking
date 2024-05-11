using BookingApp.Aplication.Dto;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Aplication.UseCases
{
    public class ChangeRequestService
    {
        private readonly IReservationChangeRequestRepository reservationChangeRequestsRepository;


        public ChangeRequestService(IReservationChangeRequestRepository reservationChangeRequestsRepository)
        {
           this.reservationChangeRequestsRepository = reservationChangeRequestsRepository;
        }

        public List<ReservationChangeRequest> GetAllRequests()
        {
            return reservationChangeRequestsRepository.GetAll();
        }
        public void UpdateChangeRequestStatus(int requestId, RequestStatus newStatus)
        {
            reservationChangeRequestsRepository.UpdateChangeRequestStatus(requestId, newStatus);
        }

        public ObservableCollection<ReservationChangeRequest> GetAllGuestsRequests(int GuestId)
        {
            return new ObservableCollection<ReservationChangeRequest>(reservationChangeRequestsRepository.GetAll().FindAll(r => r.GuestId == GuestId));
        }

        public void SaveRequest(ReservationChangeRequestDto reservationChangeRequest)
        {
            reservationChangeRequestsRepository.AddReservationChangeRequest(reservationChangeRequest.ToReservationChangeRequest());
        }

        public List<ReservationChangeRequest> UpdateGuestsRequests(int guestId)
        {
            return reservationChangeRequestsRepository.GetAll().FindAll(r => r.GuestId == guestId);
        }

        public void UpdateChangeRequestComment(int requestId, string comment)
        {
            reservationChangeRequestsRepository.UpdateChangeRequestComment(requestId, comment);
        }
        public int GetAcceptedReservationChangeRequestsCount(string propertyName, int year)
        {
            var changeRequestsForProperty = reservationChangeRequestsRepository.GetAll()
                                                .Where(r => r.PropertyName == propertyName && r.RequestStatus == RequestStatus.Approved);

            var changeRequestsForYear = changeRequestsForProperty.Where(r => r.NewStartDate.Year == year || r.NewEndDate.Year == year);

            return changeRequestsForYear.Count();
        }

    }
}
