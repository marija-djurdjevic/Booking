using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for DenyRequestPage.xaml
    /// </summary>
    public partial class DenyRequestPage : Page
    {
        private readonly int requestId;

        private readonly ReservationChangeRequestsRepository reservationChangeRequestsRepository;
        public ObservableCollection<ReservationChangeRequest> ReservationChangeRequests { get; set; }
        public ReservationChangeRequest request;
        public DenyRequestPage(ReservationChangeRequest request)
        {
            InitializeComponent();
            requestId = request.Id;
            this.request = request;
            reservationChangeRequestsRepository = new ReservationChangeRequestsRepository();
            ReservationChangeRequests = new ObservableCollection<ReservationChangeRequest>();

            // Možete izvršiti dodatne radnje ovdje, ako je potrebno
        }
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            // Dobijte uneseni komentar
            string comment = CommentTextBox.Text.Trim();

            // Dobijte ID zahtjeva za promjenu rezervacije iz konstruktora
            int requestId = this.requestId;

            // Ažurirajte komentar u zahtjevu za pomijeranje rezervacije
            reservationChangeRequestsRepository.UpdateChangeRequestComment(requestId, comment);
            reservationChangeRequestsRepository.UpdateChangeRequestStatus(requestId, RequestStatus.Declined);

            // ReservationChangeRequests.Remove(request);
            //reservationChangeRequestsRepository.Delete(request.Id);

            //NavigationService.GoBack();

            MessageBox.Show("Comment saved successfully.");
        }
    }
}
