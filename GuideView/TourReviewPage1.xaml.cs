using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.View;
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

namespace BookingApp.GuideView
{
    /// <summary>
    /// Interaction logic for TourReviewPage1.xaml
    /// </summary>
    public partial class TourReviewPage1 : Page
    {
        private readonly TouristExperienceRepository touristExperienceRepository;
        public TourReviewPage1(int tourId)
        {
            InitializeComponent();
            touristExperienceRepository = new TouristExperienceRepository();
            DataContext = new TourReviewViewModel(tourId);
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
                    var viewModel = DataContext as TourReviewViewModel;
                    if (viewModel != null)
                    {
                        var touristExperiences = viewModel.TouristExperiences;
                        if (touristExperiences != null)
                        {
                            DataContext = null;
                            DataContext = viewModel;
                           // touristExperienceRepository.Update(experience);
                            // Osvežite prikaz tako što ćete ponovo postaviti DataContext
                          //  viewModel.OnPropertyChanged(nameof(viewModel.TouristExperiences));
                            touristExperienceRepository.Update(experience);
                        }
                    }
                }
            }
        }


        private void NavigateToSideMenuPage(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
