using BookingApp.Dto;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace BookingApp.ViewModel.GuestViewModel
{
    public class ChangeReservationViewModel
    {
        public ReservationChangeRequestsRepository ReservationChangeRequestsRepository { get; set; }
        public PropertyReservation SelectedReservation { get; set; }
        public ObservableCollection<ReservationChangeRequest> GuestsRequests { get; set; }
        public Property SelectedProperty { get; set; }
        public Guest LoggedInGuest { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public ReservationChangeRequestDto ReservationChangeRequest { get; set; }

        public ChangeReservationViewModel(PropertyReservation selectedReservation, Property selectedProperty, Guest guest)
        {
            SelectedReservation = selectedReservation;
            LoggedInGuest = guest;
            SelectedProperty = selectedProperty;
            ReservationChangeRequest = new ReservationChangeRequestDto();
            ReservationChangeRequestsRepository = new ReservationChangeRequestsRepository();
            GuestsRequests = new ObservableCollection<ReservationChangeRequest>(ReservationChangeRequestsRepository.GetAll().FindAll(r => r.GuestId == LoggedInGuest.Id));

        }

        public void ChangeDate1(object sender)
        {
            if (sender is DatePicker datePicker)
            {
                FromDate = datePicker.SelectedDate ?? DateTime.Now;
            }
        }

        public void ChangeDate2(object sender)
        {
            if (sender is DatePicker datePicker)
            {
                ToDate = datePicker.SelectedDate ?? DateTime.Now;
            }

        }

        public void SendRequest()
        {
            ReservationChangeRequest.ReservationId = SelectedReservation.Id;
            ReservationChangeRequest.OldStartDate = SelectedReservation.StartDate;
            ReservationChangeRequest.OldEndDate = SelectedReservation.EndDate;
            ReservationChangeRequest.NewStartDate = FromDate;
            ReservationChangeRequest.NewEndDate = ToDate;
            ReservationChangeRequest.PropertyName = SelectedProperty.Name;
            ReservationChangeRequest.GuestId = LoggedInGuest.Id;
            ReservationChangeRequestsRepository.AddReservationChangeRequest(ReservationChangeRequest.ToReservationChangeRequest());
            GuestsRequests.Clear();
            ReservationChangeRequestsRepository.GetAll().FindAll(r => r.GuestId == LoggedInGuest.Id).ForEach(GuestsRequests.Add);
        }
    }
}
