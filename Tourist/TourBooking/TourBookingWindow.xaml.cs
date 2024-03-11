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
using System.Windows.Shapes;

namespace BookingApp.Tourist.TourBooking
{
    /// <summary>
    /// Interaction logic for TourBookingWindow.xaml
    /// </summary>
    public partial class TourBookingWindow : Window
    {
        public static TourRepository TourRepository = new TourRepository();
        public TourDto SelectedTour { get; set; }
        public int NumberOfReservations { get; set; }
        public User LoggedInUser { get; set; }

        public TourBookingWindow(TourDto selectedTour,User loggedInUser)
        {
            InitializeComponent();
            DataContext = this;
            SelectedTour = selectedTour;
            NumberOfReservations = 1;
            LoggedInUser = loggedInUser;
        }

        private void ConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            if(NumberOfReservations == 1)
            {
                ReservationDataRepository reservationDataRepository=new ReservationDataRepository();
                reservationDataRepository.AddReservationData(new ReservationData(SelectedTour.Id,LoggedInUser.FirstName,LoggedInUser.LastName,LoggedInUser.Age));
                SelectedTour.MaxTouristNumber--;
                TourRepository.UpdateTour(SelectedTour.ToTour());
                Close();
            }
            if(NumberOfReservations > 1 && NumberOfReservations <= SelectedTour.MaxTouristNumber)
            {
                TouristsDataWindow touristsDataWindow = new TouristsDataWindow(NumberOfReservations,SelectedTour,LoggedInUser);
                touristsDataWindow.ShowDialog();
                Close();
            }
            if (NumberOfReservations > SelectedTour.MaxTouristNumber)
            {
                MessageBox.Show("On the tour, there are only spots left for"+ SelectedTour.MaxTouristNumber.ToString() +" tourists.");
            }
        }

        private void CancelButtonCLick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
