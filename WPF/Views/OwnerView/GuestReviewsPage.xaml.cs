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
using BookingApp.Domain.Models;
using System.IO;
using BookingApp.WPF.ViewModel.GuestViewModel;
using BookingApp.WPF.ViewModel.OwnerViewModel;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for GuestReviewsPage.xaml
    /// </summary>
    public partial class GuestReviewsPage : Page
    {
       /* public ObservableCollection<KeyValuePair<OwnerReview, PropertyReservation>> OwnerReviews { get; set; }

        private OwnerReviewRepository ownerReviewRepository;
        private PropertyReservationRepository reservationRepository;
        private ReviewRepository reviewRepository;
        private int currentImageIndex = 0;

        private void PreviousImage_Click(object sender, RoutedEventArgs e)
        {
            // Provjera da li postoje prethodne slike
            if (currentImageIndex > 0)
            {
                currentImageIndex--; // Smanjivanje indeksa za prikaz prethodne slike
                UpdateImage(); // Ažuriranje prikazane slike
            }
        }*/

        /* private void NextImage_Click(object sender, RoutedEventArgs e)
         {
             // Provjera da li postoje sljedeće slike
             if (currentImageIndex < ImagesPaths.Count - 1)
             {
                 currentImageIndex++; // Povećanje indeksa za prikaz sljedeće slike
                 UpdateImage(); // Ažuriranje prikazane slike
             }
         }

         private void UpdateImage()
         {
             // Postavljanje izvora slike na temelju trenutnog indeksa
             string imagePath = ImagesPaths[currentImageIndex];
             // Postavljanje slike na Image element
             CurrentImageView.Source = new BitmapImage(new Uri(imagePath));
         }*/
       /* private void NextImage_Click(object sender, RoutedEventArgs e)
        {
            // Provjera da li postoje sljedeće slike
            if (currentImageIndex < OwnerReviews.Count - 1)
            {
                currentImageIndex++; // Povećanje indeksa za prikaz sljedeće slike
                UpdateImage(); // Ažuriranje prikazane slike
            }
        }

        private void UpdateImage()
        {
            // Dohvaćanje OwnerReview objekta na temelju trenutnog indeksa
            var ownerReview = OwnerReviews[currentImageIndex].Key;

            // Provjera da li postoje slike za ovu recenziju
            if (ownerReview.ImagesPaths != null && ownerReview.ImagesPaths.Any())
            {
                // Postavljanje izvora slike na temelju trenutnog indeksa
                string imagePath = ownerReview.ImagesPaths[currentImageIndex];
                // Postavljanje slike na Image element
                CurrentImageView.Source = new BitmapImage(new Uri(imagePath));
            }
            else
            {
                // Ako nema slika za ovu recenziju, postaviti izvor slike na null ili neku defaultnu sliku
                CurrentImageView.Source = null;
            }
        }*/
       



        public GuestReviewsPage()
        {
            InitializeComponent();
            DataContext = new GuestReviewsViewModel();
            /*ownerReviewRepository = new OwnerReviewRepository();
            reservationRepository = new PropertyReservationRepository();
            reviewRepository = new ReviewRepository();

            // Inicijalizacija ObservableCollection-a za recenzije gostiju
            OwnerReviews = new ObservableCollection<KeyValuePair<OwnerReview, PropertyReservation>>();

            // Učitavanje recenzija gostiju iz repozitorija
            LoadOwnerReviewsFromRepository();

            // Postavljanje DataContext-a na ObservableCollection
            DataContext = this;

            // Dodavanje događaja Loaded
            Loaded += GuestReviewsPage_Loaded;*/
        }
        
       /* private void LoadOwnerReviewsFromRepository()
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
           /* foreach (var ownerReview in ownerReviews)
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
            foreach (var ownerReview in OwnerReviews)
            {
                var guestImages = new List<string>();

                // Pretraživanje foldera za slike s ekstenzijom .jpg
                var directoryPath = "Resources/Images/GuestExperienceImages";
                if (Directory.Exists(directoryPath))
                {
                    var imageFiles = Directory.GetFiles(directoryPath, "*.jpg");

                    foreach (var imagePath in imageFiles)
                    {
                        // Dodavanje putanja do slika
                        guestImages.Add(imagePath);
                    }
                }

                // Dodavanje listu slika za ovu recenziju
                ownerReview.Key.ImagesPaths = guestImages;
            }
        }*/

    }
}
