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
using System.Windows.Shapes;

namespace BookingApp.Tourist.TourBooking
{
    /// <summary>
    /// Interaction logic for TouristsDataWindow.xaml
    /// </summary>
    public partial class TouristsDataWindow : Window
    {
        public static ObservableCollection<ReservationData> Tourists { get; set; }
        public TourDto SelectedTour { get; set; }
        public User LoggedInUser { get; set; }
        public TourRepository TourRepository { get; set; }

        public TouristsDataWindow(int touristNumber, TourDto selectedTour, User user)
        {
            InitializeComponent();
            DataContext = this;
            Tourists = new ObservableCollection<ReservationData>();
            SelectedTour = selectedTour;
            LoggedInUser = user;
            TourRepository = new TourRepository();

            for (int i = 0; i < touristNumber - 1; i++)
            {
                Tourists.Add(new ReservationData(SelectedTour.Id));
            }
        }

        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            ReservationDataRepository reservationDataRepository = new ReservationDataRepository();

            foreach (ReservationData data in Tourists)
            {
                reservationDataRepository.Save(data);
            }
            reservationDataRepository.Save(new ReservationData(SelectedTour.Id, LoggedInUser.FirstName, LoggedInUser.LastName, LoggedInUser.Age));

            SelectedTour.MaxTouristNumber -= Tourists.Count() + 1;
            TourRepository.UpdateTour(SelectedTour.ToTour());
            Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
