using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.View.TouristView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using BookingApp.Service;
using System.Windows.Controls;

namespace BookingApp.ViewModel.TouristView
{
    public class MyToursViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Tuple<TourDto, Visibility>> tours;
        public ObservableCollection<Tuple<TourDto, Visibility>> Tours
        {
            get { return tours; }
            set
            {
                tours = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Tuple<TourDto, List<KeyPoint>, KeyPoint>> activeTours;
        public ObservableCollection<Tuple<TourDto, List<KeyPoint>, KeyPoint>> ActiveTours
        {
            get { return activeTours; }
            set
            {
                activeTours = value;
                OnPropertyChanged();
            }
        }

        private Visibility noMyToursTextVisibility;
        public Visibility NoMyToursTextVisibility
        {
            get { return noMyToursTextVisibility; }
            set
            {
                noMyToursTextVisibility = value;
                OnPropertyChanged(nameof(noMyToursTextVisibility));
            }
        }
        
        private Visibility noActiveToursTextVisibility;
        public Visibility NoActiveToursTextVisibility
        {
            get { return noActiveToursTextVisibility; }
            set
            {
                noActiveToursTextVisibility = value;
                OnPropertyChanged(nameof(noActiveToursTextVisibility));
            }
        }

        public User LoggedInUser { get; set; }
        public TourDto SelectedTour { get; set; }

        private readonly TourRepository tourRepository;
        private readonly TouristExperienceRepository touristExperienceRepository;
        private readonly TourReservationRepository reservationRepository;
        private readonly TourService tourService;

        public MyToursViewModel(User loggedInUser)
        {
            LoggedInUser = loggedInUser;
            SelectedTour = new TourDto();
            Tours = new ObservableCollection<Tuple<TourDto, Visibility>>();
            ActiveTours = new ObservableCollection<Tuple<TourDto, List<KeyPoint>, KeyPoint>>();

            tourRepository = new TourRepository();
            reservationRepository = new TourReservationRepository();
            touristExperienceRepository = new TouristExperienceRepository();
            tourService = new TourService();

            GetMyTours();
            GetMyActiveTours();
        }

        public void GetMyTours()
        {
            Tours.Clear();
            foreach (var tour in tourService.GetMyReserved(LoggedInUser.Id))
            {
                Tours.Add(new Tuple<TourDto, Visibility>(new TourDto(tour), IsRateButtonVisible(tour.Id, LoggedInUser.Id)));
            }
            if (Tours.Count() < 1)
            {
                NoMyToursTextVisibility = Visibility.Visible;
            }
            else
            {
                NoMyToursTextVisibility = Visibility.Collapsed;
            }
        }

        public void GetMyActiveTours()
        {
            ActiveTours.Clear();
            foreach (Tour tour in tourService.GetMyActiveReserved(LoggedInUser.Id))
            {
                TourDto tourDto = new TourDto(tour);
                List<KeyPoint> keyPoints = tourDto.KeyPoints.Skip(1).Take(tourDto.KeyPoints.Count - 2).ToList();
                KeyPoint endPoint = tour.KeyPoints.Last();
                ActiveTours.Add(new Tuple<TourDto, List<KeyPoint>, KeyPoint>(tourDto, keyPoints, endPoint));
            }
            if (ActiveTours.Count() < 1)
            {
                NoActiveToursTextVisibility = Visibility.Visible;
            }
            else
            {
                NoActiveToursTextVisibility = Visibility.Collapsed;
            }
        }

        private Visibility IsRateButtonVisible(int tourId, int userId)
        {
            Visibility visibility = Visibility.Hidden;
            List<TourReservation> finishedReservationsAttendedByUser = reservationRepository.GetFinishedReservationsAttendedByUser(userId);

            if (finishedReservationsAttendedByUser.Find(t => t.TourId == tourId) != null && !touristExperienceRepository.IsTourRatedByUser(tourId, userId))
            {
                visibility = Visibility.Visible;
            }
            return visibility;
        }

        public void OpenInbox()
        {
            NotificationsWindow notificationsWindow = new NotificationsWindow(LoggedInUser);
            notificationsWindow.ShowDialog();
        }

        public void RateTour(object sender)
        {
            Button rateButton = (Button)sender;
            Tuple<TourDto, Visibility> tupl = (Tuple<TourDto, Visibility>)rateButton.DataContext;
            SelectedTour = tupl.Item1;
            RateTourWindow rateTourWindow = new RateTourWindow(SelectedTour, LoggedInUser);
            rateTourWindow.ShowDialog();
            GetMyTours();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
