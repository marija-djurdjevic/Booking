using BookingApp.Domain.Models;
using BookingApp.Domain.Models.Enums;
using BookingApp.Repositories;
using BookingApp.View.GuideView;
using BookingApp.WPF.ViewModels.GuidesViewModel;
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
            GuideMainPage guideMainPage = new GuideMainPage();
            this.NavigationService.Navigate(guideMainPage);
        }


        private void NavigateToSideMenuPage(object sender, MouseButtonEventArgs e)
        {
            SideMenuPage sideMenuPage = new SideMenuPage();
            this.NavigationService.Navigate(sideMenuPage);
        }
    }
}
