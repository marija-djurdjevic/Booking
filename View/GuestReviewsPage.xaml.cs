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
using BookingApp.Model;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for GuestReviewsPage.xaml
    /// </summary>
    public partial class GuestReviewsPage : Page
    {
        public ObservableCollection<KeyValuePair<OwnerReview, PropertyReservation>> OwnerReviews { get; set; }

        private OwnerReviewRepository ownerReviewRepository;
        private PropertyReservationRepository reservationRepository;
        private ReviewRepository reviewRepository;
        public GuestReviewsPage()
        {
            InitializeComponent();
            ownerReviewRepository = new OwnerReviewRepository();
            reservationRepository = new PropertyReservationRepository();
            reviewRepository = new ReviewRepository();

            // Inicijalizacija ObservableCollection-a za recenzije gostiju
            OwnerReviews = new ObservableCollection<KeyValuePair<OwnerReview, PropertyReservation>>();

            // Učitavanje recenzija gostiju iz repozitorija
            LoadOwnerReviewsFromRepository();

            // Postavljanje DataContext-a na ObservableCollection
            DataContext = this;

            // Dodavanje događaja Loaded
            Loaded += GuestReviewsPage_Loaded;
        }
        
        private void LoadOwnerReviewsFromRepository()
        {
            var ownerReviews = ownerReviewRepository.GetAllReviews();
            var guestReviews = reviewRepository.GetAllReviews();
            var guestReviewReservationIds = guestReviews.Select(review => review.ReservationId).ToList();

            // Dodavanje svih recenzija u ObservableCollection
            /*foreach (var review in reviews)
            {
                // Dohvatanje informacija o rezervaciji na osnovu ReservationId
                var reservation = reservationRepository.GetReservationById(review.ReservationId);
                

                // Ako rezervacija nije pronađena, preskačemo ovu recenziju
                if (reservation == null)
                {
                    continue;
                }

                // Privremeno povezivanje recenzije i podataka o rezervaciji
                var reviewWithReservation = new KeyValuePair<OwnerReview, PropertyReservation>(review, reservation);

                // Dodavanje recenzije u ObservableCollection
                OwnerReviews.Add(reviewWithReservation);
            }*/
            foreach (var ownerReview in ownerReviews)
            {
                // Provjera da li se ID rezervacije iz OwnerReview nalazi u listi ID-ova ocijenjenih rezervacija
                if (guestReviewReservationIds.Contains(ownerReview.ReservationId))
                {
                    // Dohvatanje informacija o rezervaciji na osnovu ReservationId
                    var reservation = reservationRepository.GetReservationById(ownerReview.ReservationId);

                    // Ako rezervacija nije pronađena, preskačemo ovu recenziju
                    if (reservation == null)
                    {
                        continue;
                    }

                    // Privremeno povezivanje recenzije i podataka o rezervaciji
                    var reviewWithReservation = new KeyValuePair<OwnerReview, PropertyReservation>(ownerReview, reservation);

                    // Dodavanje recenzije u ObservableCollection
                    OwnerReviews.Add(reviewWithReservation);
                }
            }
        }

        private void GuestReviewsPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Implementacija događaja Loaded
            // Ovdje možete dodati dodatne operacije koje treba izvršiti nakon što se stranica učita
        }
    }
}
