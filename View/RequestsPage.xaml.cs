using BookingApp.Dto;
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
using BookingApp.Repository;
using BookingApp.Model;
using BookingApp.Serializer;
using BookingApp.ViewModel.OwnerViewModel;
using BookingApp.ViewModel;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for RequestsPage.xaml
    /// </summary>
    public partial class RequestsPage : Page
    {
        /*public ObservableCollection<ReservationChangeRequest> ReservationChangeRequests { get; set; }

        PropertyReservationRepository reservationRepository;
        ReservationChangeRequestsRepository requestsRepository;*/

        public RequestsPage()
        {
            InitializeComponent();
            DataContext = new RequestsViewModel();
            /*reservationRepository = new PropertyReservationRepository();
            requestsRepository = new ReservationChangeRequestsRepository();
            ReservationChangeRequests = new ObservableCollection<ReservationChangeRequest>();

            ReservationChangeRequests = new ObservableCollection<ReservationChangeRequest>();

            // Učitavanje zahtjeva iz Repository-ja
            LoadReservationChangeRequestsFromRepository();

            // Postavljanje DataContext-a na ObservableCollection
            DataContext = this;
            Loaded += RequestsPage_Loaded;*/
        }
        /*private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            // Dobijanje DataContext-a dugmeta koje je kliknuto (to bi trebalo biti instanca ReservationChangeRequest)
            if (sender is FrameworkElement element && element.DataContext is ReservationChangeRequest request)
            {
                // Ažurirajte rezervaciju
                UpdateReservation(request);
                ReservationChangeRequests.Remove(request);
               // requestsRepository.Delete(request.Id);
                
            }
        }*/
        private void DenyButton_Click(object sender, RoutedEventArgs e) 
        {

            if (sender is FrameworkElement element && element.DataContext is ReservationChangeRequest request)
            {
                
                //int requestId = request.Id;
                DenyRequestPage denyRequestPage = new DenyRequestPage(request);
                NavigationService.Navigate(denyRequestPage);
            }
        }
        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is RequestsViewModel viewModel && sender is FrameworkElement element && element.DataContext is ReservationChangeRequest request)
            {
                viewModel.Accept(request);
            }
        }
        /*private void DenyButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is RequestsViewModel viewModel && sender is FrameworkElement element && element.DataContext is ReservationChangeRequest request)
            {
                viewModel.Deny(request);
            }
        }*/


        /*private void UpdateReservation(ReservationChangeRequest request)
        {
            // Dohvati rezervacije za ovaj zahtjev
            var reservationsForProperty = reservationRepository.GetReservationDataById(request.ReservationId);

            // Ako postoje rezervacije za ovaj zahtjev
            if (reservationsForProperty != null && reservationsForProperty.Count > 0)
            {
                // Ažurirajte prvu rezervaciju sa novim datumima
                var updatedReservation = reservationsForProperty.First();
                updatedReservation.StartDate = request.NewStartDate;
                updatedReservation.EndDate = request.NewEndDate;

                // Pozovite metod za ažuriranje rezervacije u repozitoriju
                reservationRepository.UpdatePropertyReservation(updatedReservation);
                
            }
            else
            {
                // Obavijestite korisnika da rezervacija nije pronađena
                MessageBox.Show("Reservation not found for ReservationId: " + request.ReservationId);
            }
        }
        
        private void RequestsPage_Loaded(object sender, RoutedEventArgs e)
        {
            CheckAvailabilityForAllRequests(); // Poziv metode za provjeru dostupnosti smještaja
        }
        
        private void CheckAvailabilityForAllRequests()
        {
            //if(reservationRepository != null) {
                var allReservations = reservationRepository.GetAll();

                foreach (var item in ReservationChangeRequestsListView.Items)
                {
                    if (item is ReservationChangeRequest request)
                    {
                        int reservationId = request.ReservationId;

                        var reservationsForProperty = allReservations.Where(r => r.Id == reservationId).ToList();

                        bool isAvailable = CheckAvailability(request.NewStartDate, request.NewEndDate, reservationsForProperty);
                        request.Status = isAvailable ? "Free" : "Occupied";
                    ReservationChangeRequestsListView.Items.Refresh();
                    }
                }
           
         }



        private void LoadReservationChangeRequestsFromRepository()
        {
            ReservationChangeRequestsRepository repository = new ReservationChangeRequestsRepository();
            var requests = repository.GetAll();

            // Ažuriranje ObservableCollection sa učitanim zahtjevima
            foreach (var request in requests)
            {
                if (request.RequestStatus == RequestStatus.Processing)
                {
                    ReservationChangeRequests.Add(request);
                }
                   
            }
        }
       
        private bool CheckAvailability(DateTime newStartDate, DateTime newEndDate, List<PropertyReservation> reservations)
        {
            // Implementirati logiku za provjeru dostupnosti smještaja
            foreach (var reservation in reservations)
            {
                // Provjeravamo postoji li preklapanje s novim datumima
                if (newStartDate >= reservation.StartDate && newStartDate <= reservation.EndDate)
                {
                    return false; // Smještaj nije dostupan
                }
                if (newEndDate >= reservation.StartDate && newEndDate <= reservation.EndDate)
                {
                    return false; // Smještaj nije dostupan
                }
            }
            return true; // Smještaj je dostupan
        }*/


    }


}
