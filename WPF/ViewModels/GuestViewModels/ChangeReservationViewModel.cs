using BookingApp.Aplication;
using BookingApp.Aplication.Dto;
using BookingApp.Aplication.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace BookingApp.WPF.ViewModel.GuestViewModel
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
        public ChangeRequestService changeRequestService;

        public ChangeReservationViewModel(PropertyReservation selectedReservation, Property selectedProperty, Guest guest)
        {
            SelectedReservation = selectedReservation;
            LoggedInGuest = guest;
            SelectedProperty = selectedProperty;
            ReservationChangeRequestsRepository = new ReservationChangeRequestsRepository();
            changeRequestService = new ChangeRequestService(Injector.CreateInstance<IReservationChangeRequestRepository>());
            GuestsRequests = changeRequestService.GetAllGuestsRequests(LoggedInGuest.Id);

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
            ReservationChangeRequest = new ReservationChangeRequestDto
            {
                ReservationId = SelectedReservation.Id,
                OldStartDate = SelectedReservation.StartDate,
                OldEndDate = SelectedReservation.EndDate,
                NewStartDate = FromDate,
                NewEndDate = ToDate,
                PropertyName = SelectedProperty.Name,
                GuestId = LoggedInGuest.Id
            };

            changeRequestService.SaveRequest(ReservationChangeRequest);
            GuestsRequests.Clear();
            changeRequestService.UpdateGuestsRequests(LoggedInGuest.Id).ForEach(GuestsRequests.Add);
        }
    }
}
