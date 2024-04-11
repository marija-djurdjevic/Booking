using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.View;
using BookingApp.ViewModel;
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


        private void NavigateToSideMenuPage(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
