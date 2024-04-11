using BookingApp.Dto;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.GuestView
{
    /// <summary>
    /// Interaction logic for ChangeReservation.xaml
    /// </summary>
    public partial class ChangeReservation : Page
    {
        public ReservationChangeRequestsRepository ReservationChangeRequestsRepository { get; set; }
        public PropertyReservation SelectedReservation { get; set; }
        public ObservableCollection<ReservationChangeRequest> GuestsRequests {  get; set; }
        public Property SelectedProperty { get; set; }
        public Guest LoggedInGuest { get; set; }
        public DateTime FromDate {  get; set; }
        public DateTime ToDate { get; set; }
        public ReservationChangeRequestDto ReservationChangeRequest { get; set; }
        public ChangeReservation(PropertyReservation selectedReservation, Property selectedProperty, Guest guest)
        {
            InitializeComponent();
            DataContext = this;
            SelectedReservation = selectedReservation;
            LoggedInGuest = guest;
            SelectedProperty = selectedProperty;
            ReservationChangeRequest = new ReservationChangeRequestDto();
            ReservationChangeRequestsRepository = new ReservationChangeRequestsRepository();
            GuestsRequests = new ObservableCollection<ReservationChangeRequest>(ReservationChangeRequestsRepository.GetAll().FindAll(r => r.GuestId == LoggedInGuest.Id));

        }

        private void DatePicker_SelectedDate1Changed(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DatePicker datePicker)
            {
                FromDate = datePicker.SelectedDate ?? DateTime.Now;
            }
        }

        private void DatePicker_SelectedDate2Changed(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DatePicker datePicker)
            {
                ToDate = datePicker.SelectedDate ?? DateTime.Now;
            }
        }

        private void SendRequest_Click(object sender, RoutedEventArgs e)
        {
            ReservationChangeRequest.OldStartDate = SelectedReservation.StartDate;
            ReservationChangeRequest.OldEndDate = SelectedReservation.EndDate;
            ReservationChangeRequest.NewStartDate = FromDate;
            ReservationChangeRequest.NewEndDate = ToDate;
            ReservationChangeRequest.PropertyName = SelectedProperty.Name;
            ReservationChangeRequest.GuestId = LoggedInGuest.Id;
            ReservationChangeRequestsRepository.AddReservationChangeRequest(ReservationChangeRequest.ToReservationChangeRequest());
            GuestsRequests.Clear();
            ReservationChangeRequestsRepository.GetAll().FindAll(r => r.GuestId == LoggedInGuest.Id).ForEach(GuestsRequests.Add);
            MessageBox.Show("Request sent!");

        }
    }
}
