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
using System.Windows.Input;

namespace BookingApp.View
{
    public partial class GuideMainPage : Page
    {
        private TourDto tourDto;
        private readonly TourRepository tourRepository;
        private Tour selectedTour;
        private readonly KeyPointRepository keyPointRepository;
        private readonly LiveTourRepository liveTourRepository;
        private readonly TourReservationRepository tourReservationRepository;
        private readonly Voucher voucher;
        private readonly TouristRepository touristRepository;
        private readonly VoucherRepository voucherRepository;

        public GuideMainPage()
        {
            InitializeComponent();
            tourRepository = new TourRepository();
            keyPointRepository = new KeyPointRepository();
            tourReservationRepository = new TourReservationRepository();
            liveTourRepository = new LiveTourRepository();
            touristRepository = new TouristRepository();
            voucherRepository = new VoucherRepository();
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
            this.NavigationService.Navigate(createTourPage);
        }

        private void StartTourButtonClick(object sender, RoutedEventArgs e)
        {
            if (tourListBox.SelectedItem != null)
            {
                selectedTour = (Tour)tourListBox.SelectedItem;
                LiveTourPage liveTourPage = new LiveTourPage(selectedTour);
                this.NavigationService.Navigate(liveTourPage);
            }
            else
            {
                MessageBox.Show("Please select a tour first.");
            }
        }

        private void NavigateToSideMenuPage(object sender, MouseButtonEventArgs e)
        {
            SideMenuPage sideMenuPage = new SideMenuPage();
            this.NavigationService.Navigate(sideMenuPage);
        }

        private void CancelTourButtonClick(object sender, RoutedEventArgs e)
        {
            selectedTour = (Tour)tourListBox1.SelectedItem;

            if (tourListBox1.SelectedItem != null)
            {
                Tour tourToCancel = (Tour)tourListBox1.SelectedItem;

                List<KeyPoint> allKeyPoints = keyPointRepository.GetTourKeyPoints(selectedTour.Id);
                List<TourReservation> allTourReservations = tourReservationRepository.GetAll();

                CancelTour(tourToCancel.Id, allKeyPoints, allTourReservations);

                LoadTours();
            }
            else
            {
                MessageBox.Show("Please select an upcoming tour to cancel.");
            }
        }

        public void CancelTour(int tourId, List<KeyPoint> keyPoints, List<TourReservation> tourReservations)
        {
            Tour tour = tourRepository.GetTourById(tourId);
            if (tour != null && (tour.StartDateTime - DateTime.Now).TotalHours > 48)
            {
                tourRepository.Delete(tourId);
                keyPointRepository.DeleteKeyPoints(tourId);

                var usersToReceiveVoucher = new List<int>();
                foreach (var reservation in tourReservations.Where(tr => tr.TourId == tourId).ToList())
                {
                    var user = reservation.UserId;
                    var tourist = touristRepository.GetByUserId(user);
                    if (tourist != null)
                    {
                        var fullName = $"{tourist.FirstName} {tourist.LastName}";
                        var reservationFullName = $"{reservation.TouristFirstName} {reservation.TouristLastName}";
                        if (fullName == reservationFullName && !usersToReceiveVoucher.Contains(user))
                        {
                            usersToReceiveVoucher.Add(user);
                        }
                    }

                }
                tourReservationRepository.DeleteByTourId(tourId);

                foreach (var userId in usersToReceiveVoucher)
                {
                    var newVoucher = new Voucher()
                    {
                        TouristId = userId,
                        Reason = "Otkaz vodica",
                        ExpirationDate = DateTime.Now.AddYears(1),
                        IsUsed = false
                    };
                    voucherRepository.Save(newVoucher);
                }
            }
            else
            {
                MessageBox.Show("The tour cannot be canceled as it starts in less than 48 hours.");
            }
        }

        private void NavigateToTourStatistic(object sender, MouseButtonEventArgs e)
        {
            TourStatistic tourStatistic = new TourStatistic();
            this.NavigationService.Navigate(tourStatistic);
        }
    }
}
