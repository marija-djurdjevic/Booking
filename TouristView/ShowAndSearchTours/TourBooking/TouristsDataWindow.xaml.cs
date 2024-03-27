using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.TouristView.Vouchers;
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
using System.Windows.Shapes;

namespace BookingApp.TouristView.TourBooking
{
    /// <summary>
    /// Interaction logic for TouristsDataWindow.xaml
    /// </summary>
    public partial class TouristsDataWindow : Window
    {
        public static ObservableCollection<TourReservation> Tourists { get; set; }
        public TourDto SelectedTour { get; set; }
        public Tourist LoggedInTourist { get; set; }
        public TourRepository TourRepository { get; set; }

        public TouristsDataWindow(int touristNumber, TourDto selectedTour, int userId)
        {
            InitializeComponent();
            DataContext = this;
            Tourists = new ObservableCollection<TourReservation>();
            TouristRepository touristRepository = new TouristRepository();
            TourRepository = new TourRepository();

            SelectedTour = selectedTour;
            LoggedInTourist = touristRepository.GetByUserId(userId);

            Tourists.Add(new TourReservation(SelectedTour.Id, LoggedInTourist));
            for (int i = 0; i < touristNumber - 1; i++)
            {
                Tourists.Add(new TourReservation(SelectedTour.Id, userId));
            }
        }

        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            TourReservationRepository reservationDataRepository = new TourReservationRepository();

            MessageBoxResult useVouchers = MessageBox.Show("Would you like to use vouchers for booking this tour?", "Vouchers", MessageBoxButton.YesNo);
            if (useVouchers == MessageBoxResult.Yes)
            {
                VouchersForReservationWindow vouchersForReservationWindow = new VouchersForReservationWindow(LoggedInTourist);
                vouchersForReservationWindow.ShowDialog();
                if (!vouchersForReservationWindow.WindowReturnValue)
                {
                    return;
                }
            }

            foreach (TourReservation data in Tourists)
            {
                reservationDataRepository.Save(data);
            }

            SelectedTour.MaxTouristNumber -= Tourists.Count();
            TourRepository.Update(SelectedTour.ToTour());

            MessageBoxResult successfullyBooked = MessageBox.Show("Reservation successfully created?", "Reservation", MessageBoxButton.OK);

            Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
