using BookingApp.Aplication;
using BookingApp.Aplication.Dto;
using BookingApp.Aplication.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.View.TouristView;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModels.TouristViewModels
{
    public class TourBookingViewModel : BindableBase
    {
        public static TourService TourService;

        public static TouristService TouristService;
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
                OnPropertyChanged(nameof(ShowingImage));
            }
        }

        public RelayCommand CloseCommand { get; set; }
        public RelayCommand ReserveCommand { get; set; }
        public RelayCommand NextImageCommand { get; set; }
        public RelayCommand PreviousImageCommand { get; set; }
        public RelayCommand ShowImageCommand { get; set; }
        public RelayCommand HelpCommand { get; set; }

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

            CloseCommand = new RelayCommand(CloseWindow);
            ReserveCommand = new RelayCommand(Confirm);
            HelpCommand = new RelayCommand(Help);
            ShowImageCommand = new RelayCommand(ShowImage);
            NextImageCommand = new RelayCommand(GetNextImage);
            PreviousImageCommand = new RelayCommand(GetPreviousImage);
        }

        private void Help()
        {

        }

        private void CloseWindow()
        {
            // Slanje poruke za zatvaranje prozora koristeći MVVM Light Messaging
            Style style = Application.Current.FindResource("MessageStyle") as Style;
            MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("Are you sure you want to close window?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning, style);
            if (result == MessageBoxResult.Yes)
                Messenger.Default.Send(new NotificationMessage("CloseTourBookingWindowMessage"));
        }

        public void Confirm()
        {

            if (NumberOfReservations > 0 && NumberOfReservations <= SelectedTour.MaxTouristNumber)
            {
                TouristsDataWindow touristsDataWindow = new TouristsDataWindow(NumberOfReservations, SelectedTour, LoggedInTourist.Id,false,new TourRequest());
                touristsDataWindow.ShowDialog();
            }
            else if (NumberOfReservations > SelectedTour.MaxTouristNumber)
            {
                Style style = Application.Current.FindResource("MessageStyle") as Style;
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("On the tour, there are only spots left for" + SelectedTour.MaxTouristNumber.ToString() + " tourists.!", "Booking", MessageBoxButton.OK, MessageBoxImage.Information, style);
            }
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
    }
}
