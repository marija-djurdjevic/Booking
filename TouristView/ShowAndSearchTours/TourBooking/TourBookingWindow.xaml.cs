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

namespace BookingApp.TouristView.TourBooking
{
    /// <summary>
    /// Interaction logic for TourBookingWindow.xaml
    /// </summary>
    public partial class TourBookingWindow : Window
    {
        public static TourRepository TourRepository;

        public static TouristRepository TouristRepository;
        public TourDto SelectedTour { get; set; }
        public int NumberOfReservations { get; set; }
        public Tourist LoggedInTourist { get; set; }

        public TourBookingWindow(TourDto selectedTour, int userId)
        {
            InitializeComponent();
            DataContext = this;
            TourRepository = new TourRepository();
            TouristRepository = new TouristRepository();

            SelectedTour = selectedTour;
            NumberOfReservations = 1;
            LoggedInTourist = TouristRepository.GetByUserId(userId);
        }

        private void ConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            
            if (NumberOfReservations > 0 && NumberOfReservations <= SelectedTour.MaxTouristNumber)
            {
                TouristsDataWindow touristsDataWindow = new TouristsDataWindow(NumberOfReservations, SelectedTour, LoggedInTourist.Id);
                touristsDataWindow.ShowDialog();
                Close();
            }
            else if (NumberOfReservations > SelectedTour.MaxTouristNumber)
            {
                MessageBox.Show("On the tour, there are only spots left for" + SelectedTour.MaxTouristNumber.ToString() + " tourists.");
            }
        }

        private void CancelButtonCLick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
