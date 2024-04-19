using BookingApp.Aplication;
using BookingApp.Aplication.Dto;
using BookingApp.Aplication.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.View.TouristView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModel.TouristViewModel
{
    public class TourBookingViewModel : INotifyPropertyChanged
    {
        public static TourService TourService;

        public static TouristService TouristService;

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

        public TourBookingViewModel(TourDto selectedTour, int userId)
        {
            TourService = new TourService(Injector.CreateInstance<ITourRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            TouristService = new TouristService(Injector.CreateInstance<ITouristRepository>());

            SelectedTour = selectedTour;
            if (SelectedTour.KeyPoints.Count > 0)
            {
                StartPoint = SelectedTour.KeyPoints[0];
                EndPoint = SelectedTour.KeyPoints[SelectedTour.KeyPoints.Count - 1];
                SelectedTour.KeyPoints = SelectedTour.KeyPoints.Skip(1).Take(SelectedTour.KeyPoints.Count - 2).ToList();
            }

            NumberOfReservations = 1;
            ImageIndex = -1;
            LoggedInTourist = TouristService.GetByUserId(userId);
            GetNextImage();
        }

        public bool Confirm()
        {

            if (NumberOfReservations > 0 && NumberOfReservations <= SelectedTour.MaxTouristNumber)
            {
                TouristsDataWindow touristsDataWindow = new TouristsDataWindow(NumberOfReservations, SelectedTour, LoggedInTourist.Id);
                touristsDataWindow.ShowDialog();
                return true;
            }
            else if (NumberOfReservations > SelectedTour.MaxTouristNumber)
            {
                MessageBox.Show("On the tour, there are only spots left for" + SelectedTour.MaxTouristNumber.ToString() + " tourists.");
                return false;
            }
            return true;
        }

        public void GetNextImage()
        {
            if (ImageIndex < SelectedTour.ImagesPaths.Count - 1)
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

        public void ShowImage()
        {
            FullScreenImageWindow fullScreenImageWindow = new FullScreenImageWindow(SelectedTour.ImagesPaths, ImageIndex);
            fullScreenImageWindow.Show();
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
