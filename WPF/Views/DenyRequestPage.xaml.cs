using BookingApp.Aplication.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Repositories;
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
        private ChangeRequestService changeRequestService { get; set; }
        public ObservableCollection<ReservationChangeRequest> ReservationChangeRequests { get; set; }
        public ReservationChangeRequest request;
        public DenyRequestPage(ReservationChangeRequest request)
        {
            InitializeComponent();
            requestId = request.Id;
            this.request = request;
            changeRequestService = new ChangeRequestService();
            reservationChangeRequestsRepository = new ReservationChangeRequestsRepository();
            ReservationChangeRequests = new ObservableCollection<ReservationChangeRequest>();

        }
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
           
            string comment = CommentTextBox.Text.Trim();
            int requestId = this.requestId;

            changeRequestService.UpdateChangeRequestComment(requestId, comment);
            changeRequestService.UpdateChangeRequestStatus(requestId, RequestStatus.Declined);
            MessageBox.Show("Comment saved successfully.");
        }
    }
}
