using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.Service;
using BookingApp.View.GuideView;
using BookingApp.ViewModel.GuidesViewModel;
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
   
    public partial class LiveTourPage : Page
    {
        private int tourId;

        private readonly TourRepository tourRepository;
        private Tour selectedTour;
        private readonly KeyPointRepository keyPointRepository;
        private readonly TouristGuideNotificationRepository touristGuideNotificationRepository;
        private readonly LiveTourRepository liveTourRepository;
        private readonly TourReservationRepository tourReservationRepository;
        public LiveTourPage(int tourId)
        {

            InitializeComponent();
            keyPointRepository = new KeyPointRepository();
            DataContext = new LiveTourViewModel(tourId);
            
        }


        private void NavigateToMainPage(object sender, MouseButtonEventArgs e)
        {
            GuideMainPage1 guideMainPage = new GuideMainPage1();
            this.NavigationService.Navigate(guideMainPage);
        }


        private void NavigateToSideMenuPage(object sender, MouseButtonEventArgs e)
        {
            SideMenuPage sideMenuPage = new SideMenuPage();
            this.NavigationService.Navigate(sideMenuPage);
        }
    }
}
