using BookingApp.Command;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
using BookingApp.View;
using BookingApp.View.GuideView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.ViewModel.GuidesViewModel
{
    public class GuideMainPageViewModel : BaseViewModel
    {
        private readonly TourService tourService;
        private readonly KeyPointService keyPointService;
        private readonly LiveTourService liveTourService;
        private readonly TourReservationService tourReservationService;
        private readonly VoucherService voucherService;
        private readonly TouristService touristService;
        private ObservableCollection<Tour> todayTours;
        private ObservableCollection<Tour> upcomingTours;
        private ObservableCollection<Tour> finishedTours;
        private Tour selectedTour;
        private RelayCommand createTourClickCommand;
        private RelayCommand startTourClickCommand;
        private RelayCommand reviewTourClickCommand;
        private RelayCommand cancelTourClickCommand;


        public GuideMainPageViewModel()
        { 
            tourService = new TourService();
            keyPointService = new KeyPointService();
            liveTourService = new LiveTourService();
            tourReservationService = new TourReservationService();
            voucherService = new VoucherService();
            touristService = new TouristService();
            createTourClickCommand = new RelayCommand(ExecuteCreateTourClick);
            startTourClickCommand = new RelayCommand(ExecuteStartTourClick);
            reviewTourClickCommand = new RelayCommand(ExecuteReviewTourClick);
            cancelTourClickCommand = new RelayCommand(ExecuteCancelTourClick);
           
            LoadTours();
        }

        public ObservableCollection<Tour> TodayTours
        {
            get { return todayTours; }
            set { todayTours = value; OnPropertyChanged(); }
        }


        public ObservableCollection<Tour> UpcomingTours
        {
            get { return upcomingTours; }
            set { upcomingTours = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Tour> FinishedTours
        {
            get { return finishedTours; }
            set { finishedTours = value; OnPropertyChanged(); }
        }

        public Tour SelectedTour
        {
            get { return selectedTour; }
            set { selectedTour = value; OnPropertyChanged(); }
        }


        private void LoadTours()
        {
            TodayTours = new ObservableCollection<Tour>(tourService.GetTodayTours());
            UpcomingTours = new ObservableCollection<Tour>(tourService.GetUpcomingTours());

            var finishedLiveTours = liveTourService.GetAllLiveTours().Where(t => t.IsLive == false).ToList();
            FinishedTours = new ObservableCollection<Tour>(finishedLiveTours.Select(tour => tourService.GetTourById(tour.TourId)));
        }






        public RelayCommand CreateTourClickCommand
        {

            get { return createTourClickCommand; }
            set
            {
                if (createTourClickCommand != value)
                {
                    createTourClickCommand = value;
                    OnPropertyChanged();
                }
            }

        }


        public RelayCommand StartTourClickCommand
        {

            get { return startTourClickCommand; }
            set
            {
                if (startTourClickCommand != value)
                {
                    startTourClickCommand = value;
                    OnPropertyChanged();
                }
            }

        }


        public RelayCommand ReviewTourClickCommand
        {

            get { return reviewTourClickCommand; }
            set
            {
                if (reviewTourClickCommand != value)
                {
                    reviewTourClickCommand = value;
                    OnPropertyChanged();
                }
            }

        }


        public RelayCommand CancelTourClickCommand
        {

            get { return cancelTourClickCommand; }
            set
            {
                if (cancelTourClickCommand != value)
                {
                    cancelTourClickCommand = value;
                    OnPropertyChanged();
                }
            }

        }



       



       


        private void ExecuteCreateTourClick()
        {
            var createTourPage = new CreateTourPage();
            GuideMainWindow.MainFrame.Navigate(createTourPage);
        }

        private void ExecuteStartTourClick(object parameter)
        {
            if (parameter != null && parameter is int tourId)
            {
                var tour = tourService.GetTourById(tourId);
                if (tour != null)
                {
                    LiveTourPage liveTourPage = new LiveTourPage(tour);
                    GuideMainWindow.MainFrame.Navigate(liveTourPage);
                }
            }

        }

        private void ExecuteReviewTourClick(object parameter)
        {
            if (parameter != null && parameter is int tourId)
            {
                TourReview touristsReviewPage = new TourReview(tourId);
                GuideMainWindow.MainFrame.Navigate(touristsReviewPage);
            }

        }

        private void ExecuteCancelTourClick(object parameter)
        {
            if (parameter != null && parameter is int tourId)
            {
                var tour = tourService.GetTourById(tourId);

                var tourKeyPoints = keyPointService.GetTourKeyPoints(tourId);
                var tourReservation = tourReservationService.GetByTourId(tourId);

                CancelTour(tour, tourKeyPoints, tourReservation);

                LoadTours();
            }

        }




        public void CancelTour(Tour tour, List<KeyPoint> keyPoints, List<TourReservation> tourReservations)
        {

            if ((tour.StartDateTime - DateTime.Now).TotalHours <= 48)
            {

                return;
            }
            liveTourService.RemoveLiveTour(tour.Id);
            tourReservationService.DeleteByTourId(tour.Id);
            DeleteTourAndKeyPoints(tour.Id);
            GenerateVouchersForCanceledTourists(tour.Id, tourReservations);
        }

        private void DeleteTourAndKeyPoints(int tourId)
        {
            tourService.Delete(tourId);
            keyPointService.DeleteKeyPoints(tourId);
        }

        private void GenerateVouchersForCanceledTourists(int tourId, List<TourReservation> tourReservations)
        {
            var usersToReceiveVoucher = new List<int>();

            foreach (var reservation in tourReservations)
            {
                int userId = reservation.UserId;
                var tourist = touristService.GetByUserId(userId);

                if (tourist != null && IsTouristReservationMatch(tourist, reservation))
                {
                    usersToReceiveVoucher.Add(userId);
                }
            }

            foreach (var userId in usersToReceiveVoucher)
            {
                GenerateVoucher(userId);
            }
        }

        private bool IsTouristReservationMatch(Tourist tourist, TourReservation reservation)
        {
            string fullName = $"{tourist.FirstName} {tourist.LastName}";
            string reservationFullName = $"{reservation.TouristFirstName} {reservation.TouristLastName}";
            return fullName == reservationFullName;
        }

        private void GenerateVoucher(int userId)
        {
            var newVoucher = new Voucher()
            {
                TouristId = userId,
                Reason = "Tour guide cancellation",
                ExpirationDate = DateTime.Now.AddYears(1),
                IsUsed = false
            };

            voucherService.Save(newVoucher);
        }



    }
}
