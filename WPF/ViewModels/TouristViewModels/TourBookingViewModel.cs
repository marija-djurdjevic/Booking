using BookingApp.Aplication;
using BookingApp.Aplication.Dto;
using BookingApp.Aplication.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.View.TouristView;
using BookingApp.WPF.Validations;
using BookingApp.WPF.Views.TouristView;
using GalaSoft.MvvmLight.Messaging;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace BookingApp.WPF.ViewModels.TouristViewModels
{
    public class TourBookingViewModel : BindableBase, IDataErrorInfo
    {
        public static TourService TourService;

        public static KeyPointService KeyPointService;

        public static TouristService TouristService;
        public TourDto SelectedTour { get; set; }

        private int numberOfReservations;
        public int NumberOfReservations
        {
            get => numberOfReservations;
            set
            {
                if (value != numberOfReservations)
                {
                    numberOfReservations = value;
                    OnPropertyChanged(nameof(NumberOfReservations));
                }
            }
        }
        public Tourist LoggedInTourist { get; set; }
        private string showingImage { get; set; }
        public int ImageIndex { get; set; }
        public KeyPoint StartPoint { get; set; }
        public KeyPoint EndPoint { get; set; }
        public string Error => null;

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
            TouristService = new TouristService(Injector.CreateInstance<ITouristRepository>(), Injector.CreateInstance<ITouristGuideNotificationRepository>(), Injector.CreateInstance<IVoucherRepository>());
            KeyPointService = new KeyPointService(Injector.CreateInstance<IKeyPointRepository>(),Injector.CreateInstance<ILiveTourRepository>());

            SelectedTour = selectedTour;
            SelectedTour.KeyPoints = KeyPointService.GetTourKeyPoints(SelectedTour.Id);

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

        //Verification
        public Verifications verifications = new Verifications();
        public string this[string columnName]
        {
            get
            {
                if (columnName == "NumberOfReservations")
                {
                    if (NumberOfReservations < 1)
                        return "Please enter positive number!";
                    else if (NumberOfReservations > SelectedTour.MaxTouristNumber)
                        return "On the tour, there are only spots left for " + SelectedTour.MaxTouristNumber.ToString() + " tourists.";

                }
                return null;
            }
        }
        
        private readonly string[] _validatedProperties = { "NumberOfReservations" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }
        //Verification
        private void Help()
        {
            new HelpTourBookingWindow().Show();
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
            if (!IsValid)
            {
                Style style = Application.Current.FindResource("MessageStyle") as Style;
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("Field must be filled correctly!", "Error", MessageBoxButton.OK, MessageBoxImage.Error, style);
                return;
            }
            if (NumberOfReservations > 0 && NumberOfReservations <= SelectedTour.MaxTouristNumber)
            {
                TouristsDataWindow touristsDataWindow = new TouristsDataWindow(NumberOfReservations, SelectedTour, LoggedInTourist.Id,false,new TourRequestViewModel(),false,new ComplexTourRequest());
                touristsDataWindow.ShowDialog();
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
