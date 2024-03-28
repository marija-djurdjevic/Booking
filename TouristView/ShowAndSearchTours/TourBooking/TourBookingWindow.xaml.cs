using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
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
    public partial class TourBookingWindow : Window, INotifyPropertyChanged
    {
        public static TourRepository TourRepository;

        public static TouristRepository TouristRepository;

        public event PropertyChangedEventHandler? PropertyChanged;

        public TourDto SelectedTour { get; set; }
        public int NumberOfReservations { get; set; }
        public Tourist LoggedInTourist { get; set; }
        private string showingImage { get; set; }
        public int ImageIndex { get; set; }
        public KeyPoint StartPoint { get; set; }
        public KeyPoint EndPoint { get; set; }

        public string ShowingImage
        {
            get { return showingImage; }
            set
            {
                showingImage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowingImage)));
            }
        }

        public TourBookingWindow(TourDto selectedTour, int userId)
        {
            InitializeComponent();
            DataContext = this;
            TourRepository = new TourRepository();
            TouristRepository = new TouristRepository();

            SelectedTour = selectedTour;
            if (SelectedTour.KeyPoints.Count > 0)
            {
                StartPoint = SelectedTour.KeyPoints[0];
                EndPoint = SelectedTour.KeyPoints[SelectedTour.KeyPoints.Count - 1];
                SelectedTour.KeyPoints = SelectedTour.KeyPoints.Skip(1).Take(SelectedTour.KeyPoints.Count - 2).ToList();
            }

            NumberOfReservations = 1;
            ImageIndex = -1;
            LoggedInTourist = TouristRepository.GetByUserId(userId);
            GetNextImage();
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

        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {

        }

        public void GetNextImage()
        {
            if (ImageIndex < SelectedTour.ImagesPaths.Count-1)
            {
                string imagePath = SelectedTour.ImagesPaths[++ImageIndex];
                ShowingImage = imagePath;
            }
        }

        public void GetPreviousImage()
        {
            if (ImageIndex > 0)
            {
                string imagePath = SelectedTour.ImagesPaths[--ImageIndex];
                ShowingImage = imagePath;

            }
        }

        private void ImageButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void NextImageButtonClick(object sender, RoutedEventArgs e)
        {
            GetNextImage();
        }

        private void PreviousImageButtonClick(object sender, RoutedEventArgs e)
        {
            GetPreviousImage();
        }
    }
}
