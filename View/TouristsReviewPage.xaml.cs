using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for TouristsReviewPage.xaml
    /// </summary>
    public partial class TouristsReviewPage : Page
    {
        private int tourId;

        private readonly TouristRepository touristRepository;
        private readonly TouristExperienceRepository touristExperienceRepository;
        private readonly TourReservationRepository tourReservationRepository;
        public TouristsReviewPage(int tourId)
        {
            InitializeComponent();
            this.tourId = tourId;
            touristRepository = new TouristRepository();
            touristExperienceRepository = new TouristExperienceRepository();
            tourReservationRepository = new TourReservationRepository();

            LoadTouristExperiences();
        }

        private void LoadTouristExperiences()
        {
            var touristExperiences = touristExperienceRepository.GetTouristExperiencesForTour(tourId);

            foreach (var experience in touristExperiences)
            {
                var tourist = touristRepository.GetByUserId(experience.TouristId);
                experience.Tourist = tourist;

                
                var reservations = tourReservationRepository.GetByTourId(tourId);
                var reservation = reservations.FirstOrDefault(r => r.UserId == experience.TouristId);
                if (reservation != null)
                {
                    experience.ArrivalKeyPoint = reservation.JoinedKeyPoint;
                }
            }

            DataContext = touristExperiences;
        }


        private void NavigateToSideMenuPage(object sender, MouseButtonEventArgs e)
        {

        }

        private void ReportReviewClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var experience = button.DataContext as TouristExperience;
                if (experience != null)
                {
                    experience.CommentStatus = "Invalid";

                    // Pronađite listu turističkih iskustava iz DataContext-a
                    var touristExperiences = DataContext as List<TouristExperience>;
                    if (touristExperiences != null)
                    {
                        // Ažurirajte DataContext kako biste osvežili prikaz
                        DataContext = null;
                        DataContext = touristExperiences;
                        touristExperienceRepository.Update(experience);
                    }
                }
            }
        }


    }
}
