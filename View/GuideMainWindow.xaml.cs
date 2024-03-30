using BookingApp.DTO;
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
    /// Interaction logic for GuideMainWindow.xaml
    /// </summary>
    public partial class GuideMainWindow : Window
    {
        private TourDto tourDto;
        private readonly TourRepository tourRepository;
        private Tour selectedTour;
        private readonly KeyPointRepository keyPointRepository;
        private readonly LiveTourRepository liveTourRepository;
        private readonly TourReservationRepository tourReservationRepository;
        public GuideMainWindow()
        {
            InitializeComponent();
            tourRepository = new TourRepository();
            keyPointRepository = new KeyPointRepository();
            tourReservationRepository = new TourReservationRepository();
            liveTourRepository = new LiveTourRepository();
            selectedTour = new Tour();
            tourDto = new TourDto();
            DataContext = tourDto;
            LoadTours();
        }

        private void LoadTours()
        {
            
            var todayTours = tourRepository.GetTodayTours();
            tourListBox.ItemsSource = new ObservableCollection<Tour>(todayTours);


            var upcomingTours = tourRepository.GetUpcomingTours();
            tourListBox1.ItemsSource = new ObservableCollection<Tour>(upcomingTours);
        }
        private void CreateButtonClick(object sender, RoutedEventArgs e)
        {
          
            CreateTourPage createTourPage = new CreateTourPage();
            this.Content = createTourPage;
        }

        private void StartTourButtonClick(object sender, RoutedEventArgs e)
        {
            if (tourListBox.SelectedItem != null)
            {
                selectedTour = (Tour)tourListBox.SelectedItem;
                LiveTourPage liveTourPage = new LiveTourPage(selectedTour);
                this.Content = liveTourPage;
            }
            else
            {
                MessageBox.Show("Please select a tour first.");
            }
        }



        private void NavigateToSideMenuPage(object sender, MouseButtonEventArgs e)
        {
            SideMenuPage sideMenuPage = new SideMenuPage();
            this.Content = sideMenuPage;
        }

        private void CancelTourButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
