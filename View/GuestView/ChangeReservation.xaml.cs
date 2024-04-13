using BookingApp.Dto;
using BookingApp.Model;
using BookingApp.View;
using BookingApp.Repository;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Data;

namespace BookingApp.GuestView
{
    /// <summary>
    /// Interaction logic for ChangeReservation.xaml
    /// </summary>
    public partial class ChangeReservation : Page, IValueConverter
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
            MessageBox.Show("Request sent!");

        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                switch (status)
                {
                    case "Processing":
                        return new BitmapImage(new Uri("/Resources/Images/question.png", UriKind.Relative));
                    case "Approved":
                        return new BitmapImage(new Uri("/Resources/Images/correct.png", UriKind.Relative));
                    case "Declined":
                        return new BitmapImage(new Uri("/Resources/Images/decline.png", UriKind.Relative));
                    default:
                        return null;
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
