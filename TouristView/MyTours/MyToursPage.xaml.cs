using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.TouristView.TourBooking;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace BookingApp.TouristView.MyTours
{
    /// <summary>
    /// Interaction logic for MyToursPaige.xaml
    /// </summary>
    public partial class MyToursPage : Page
    {
        public static ObservableCollection<Tuple<TourDto, List<KeyPoint>, KeyPoint>> ActiveTours { get; set; }
        public static ObservableCollection<Tuple<TourDto, Visibility>> Tours { get; set; }
        public User LoggedInUser { get; set; }
        public TourDto SelectedTour { get; set; }

        private readonly TourRepository tourRepository;

        private readonly TouristExperienceRepository touristExperienceRepository;

        private readonly TourReservationRepository reservationRepository;

        public MyToursPage(User loggedInUser)
        {
            InitializeComponent();
            DataContext = this;

            tourRepository = new TourRepository();
            reservationRepository = new TourReservationRepository();
            touristExperienceRepository= new TouristExperienceRepository();
            Tours = new ObservableCollection<Tuple<TourDto, Visibility>>();
            SelectedTour = new TourDto();
            ActiveTours = new ObservableCollection<Tuple<TourDto, List<KeyPoint>, KeyPoint>>();

            LoggedInUser = loggedInUser;
            GetMyTours();
            GetMyActiveTours();
        }

        public void GetMyTours()
        {
            Tours.Clear();
            foreach (var tour in tourRepository.GetMyReserved(LoggedInUser.Id))
            {
                Tours.Add(new Tuple<TourDto, Visibility>(new TourDto(tour), IsRateButtonVisible(tour.Id, LoggedInUser.Id)));
            }
            if (Tours.Count() < 1)
            {
                NoMyTours.Visibility = Visibility.Visible;
            }
        }
        public void GetMyActiveTours()
        {
            ActiveTours.Clear();
            foreach (Tour tour in tourRepository.GetMyActiveReserved(LoggedInUser.Id))
            {
                TourDto tourDto = new TourDto(tour);
                List<KeyPoint> keyPoints = tourDto.KeyPoints.Skip(1).Take(tourDto.KeyPoints.Count - 2).ToList();
                KeyPoint endPoint = tour.KeyPoints.Last();
                ActiveTours.Add(new Tuple<TourDto, List<KeyPoint>, KeyPoint>(tourDto, keyPoints, endPoint));
            }
            if(ActiveTours.Count() < 1)
            {
                NoActiveTours.Visibility = Visibility.Visible;
            }
        }
        private Visibility IsRateButtonVisible(int tourId, int userId)
        {
            Visibility visibility = Visibility.Hidden;
            List<TourReservation> finishedReservationsAttendedByUser = reservationRepository.GetFinishedReservationsAttendedByUser(userId);
            
            if (finishedReservationsAttendedByUser.Find(t=>t.TourId==tourId)!=null && !touristExperienceRepository.IsTourRatedByUser(tourId,userId)) 
            { 
                visibility = Visibility.Visible; 
            }
            return visibility;
        }

        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void ShowActiveToursButtonClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ActiveToursPage(LoggedInUser));
            
        }

        private void RateButtonClick(object sender, RoutedEventArgs e)
        {
            Button rateButton = (Button)sender;
            Tuple<TourDto,Visibility> tupl = (Tuple<TourDto, Visibility>)rateButton.DataContext;
            SelectedTour= tupl.Item1;
            RateTourWindow rateTourWindow = new RateTourWindow(SelectedTour,LoggedInUser);
            rateTourWindow.ShowDialog();
            GetMyTours();
        }
    }
}
