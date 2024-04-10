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

namespace BookingApp.TouristView.MyTours
{
    /// <summary>
    /// Interaction logic for ActiveToursPage.xaml
    /// </summary>
    public partial class ActiveToursPage : Page
    {
        public static ObservableCollection<Tuple<TourDto,List<KeyPoint>,KeyPoint>> ActiveTours { get; set; }
        public User LoggedInUser { get; set; }

        private readonly TourRepository tourRepository;

        private readonly TourReservationRepository reservationRepository;

        public ActiveToursPage(User loggedInUser)
        {
            InitializeComponent();
            DataContext = this;

            tourRepository = new TourRepository();
            reservationRepository = new TourReservationRepository();
            ActiveTours = new ObservableCollection<Tuple<TourDto, List<KeyPoint>, KeyPoint>>();

            LoggedInUser = loggedInUser;
            GetMyActiveTours();
        }

        public void GetMyActiveTours()
        {
            ActiveTours.Clear();
            foreach (Tour tour in tourRepository.GetMyActiveReserved(LoggedInUser.Id))
            {
                TourDto tourDto = new TourDto(tour);
                List<KeyPoint> keyPoints = tourDto.KeyPoints.Skip(1).Take(tourDto.KeyPoints.Count - 2).ToList();
                KeyPoint endPoint=tour.KeyPoints.Last();
                ActiveTours.Add(new Tuple<TourDto, List<KeyPoint>, KeyPoint>(tourDto,keyPoints,endPoint));
            }
        }

        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void ShowAllReservationsButtonClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MyToursPage(LoggedInUser));
        }

    }
}
