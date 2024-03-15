using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.TouristView.TourBooking;

namespace BookingApp.TouristView
{
    /// <summary>
    /// Interaction logic for TouristMainWindow.xaml
    /// </summary>
    public partial class TouristMainWindow : Window, INotifyPropertyChanged
    {
        public static ObservableCollection<TourDto> Tours { get; set; }
        public User LoggedInUser { get; set; }
        public TourDto SelectedTour { get; set; }

        private readonly TourRepository repository;

        private bool _isCancelSearchButtonVisible;

        public TouristMainWindow(User loggedInUser)
        {
            InitializeComponent();
            DataContext = this;

            repository = new TourRepository();
            Tours = new ObservableCollection<TourDto>();
            SelectedTour = new TourDto();

            IsCancelSearchButtonVisible = false;
            LoggedInUser = loggedInUser;
            GetAllTours();
        }

        public bool IsCancelSearchButtonVisible
        {
            get => _isCancelSearchButtonVisible;
            set
            {
                if (value != _isCancelSearchButtonVisible)
                {
                    _isCancelSearchButtonVisible = value;
                    OnPropertyChanged("IsCancelSearchButtonVisible");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void GetAllTours()
        {
            Tours.Clear();
            foreach (var tour in repository.GetAll())
            {
                Tours.Add(new TourDto(tour));
            }
        }

        private void SelectedTourCard(object sender, MouseButtonEventArgs e)
        {
            Border border = (Border)sender;
            SelectedTour = (TourDto)border.DataContext;
            if (SelectedTour.MaxTouristNumber > 0)
            {
                TourBookingWindow tourBookingWindow = new TourBookingWindow(SelectedTour, LoggedInUser.Id);
                tourBookingWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("The tour is fully booked. Please select an alternative tour from this city.");
                ShowUnbookedToursInCity();
            }
        }

        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            SearchWindow searchWindow = new SearchWindow(Tours);
            searchWindow.ShowDialog();

            IsCancelSearchButtonVisible = searchWindow.IsCancelSearchButtonVisible;
        }

        private void CancelSearchButtonClick(object sender, RoutedEventArgs e)
        {
            IsCancelSearchButtonVisible = false;
            GetAllTours();
        }

        private void ShowUnbookedToursInCity()
        {

            TourDto searchCriteria = new TourDto();
            searchCriteria.LocationDto.City = SelectedTour.LocationDto.City;
            searchCriteria.MaxTouristNumber = 1;

            List<Tour> unBookedToursInCity = repository.getMatchingTours(searchCriteria);
            unBookedToursInCity.RemoveAll(t => t.MaxTouristsNumber <= 0);

            if (unBookedToursInCity.Count > 0)
            {
                IsCancelSearchButtonVisible = true;
                Tours.Clear();
                foreach (var tour in unBookedToursInCity)
                {
                    Tours.Add(new TourDto(tour));
                }
            }
            else
            {
                MessageBox.Show("There are no tours from that city");
            }
        }
    }
}
