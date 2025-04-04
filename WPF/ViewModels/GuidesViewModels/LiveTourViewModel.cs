using BookingApp.Aplication;
using BookingApp.Aplication.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Models;
using BookingApp.Domain.Models.Enums;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.View;
using BookingApp.View.GuideView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace BookingApp.WPF.ViewModels.GuidesViewModel
{
    public class LiveTourViewModel : BaseViewModel
    {
        private int tourId;
        private bool isChecked;
        private TourReservation selectedTourist;
        private readonly LiveTourService liveTourService;
        private readonly TourReservationService tourReservationService;
        private readonly KeyPointService keyPointService;
        private readonly TourService tourService;
        private readonly TouristGuideNotificationRepository touristGuideNotificationRepository;
        private ObservableCollection<TourReservation> tourists;
        private ObservableCollection<KeyPoint> keyPoints;
        private Tour selectedTour;
        private RelayCommand finishTourClickCommand;
        private RelayCommand addTouristClickCommand;
        private RelayCommand checkCommand;
        private LiveTour liveTour;
        private RelayCommand sideMenuCommand;
        public User LoggedInUser { get; set; }
        public LiveTourViewModel(int tourId, User loggedInUser)
        {
            this.tourId = tourId;
            liveTourService = new LiveTourService(Injector.CreateInstance<ILiveTourRepository>(), Injector.CreateInstance<IKeyPointRepository>());
            touristGuideNotificationRepository = new TouristGuideNotificationRepository();
            keyPointService = new KeyPointService(Injector.CreateInstance<IKeyPointRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            tourService = new TourService(Injector.CreateInstance<ITourRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            SelectedTour = tourService.GetTourById(tourId);
            tourReservationService = new TourReservationService(Injector.CreateInstance<ITourReservationRepository>());
            liveTour = liveTourService.FindLiveTourById(tourId);
            var tourists = liveTourService.GetTouristsByTourId(tourId).Where(t => !t.IsOnTour).ToList();
            Tourists = new ObservableCollection<TourReservation>(tourists);
            KeyPoints = new ObservableCollection<KeyPoint>(liveTourService.GetTourKeyPoints(tourId));
            finishTourClickCommand = new RelayCommand(ExecuteFinishTourClick);
            addTouristClickCommand = new RelayCommand(ExecuteAddTouristClick);
            checkCommand = new RelayCommand(Check);
            sideMenuCommand = new RelayCommand(ExecuteSideMenuClick);
            LoggedInUser = loggedInUser;
        }
        public ObservableCollection<TourReservation> Tourists
        {
            get { return tourists; }
            set { tourists = value; OnPropertyChanged(); }
        }
        public ObservableCollection<KeyPoint> KeyPoints
        {
            get { return keyPoints; }
            set { keyPoints = value; OnPropertyChanged(); }
        }
        public Tour SelectedTour
        {
            get { return selectedTour; }
            set { selectedTour = value; OnPropertyChanged(); }
        }
        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; OnPropertyChanged(); }
        }
        public RelayCommand FinishTourClickCommand
        {
            get { return finishTourClickCommand; }
            set
            {
                if (finishTourClickCommand != value)
                {
                    finishTourClickCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        public RelayCommand AddTouristClickCommand
        {
            get { return addTouristClickCommand; }
            set
            {
                if (addTouristClickCommand != value)
                {
                    addTouristClickCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        public RelayCommand CheckCommand
        {
            get { return checkCommand; }
            set
            {
                if (checkCommand != value)
                {
                    checkCommand = value;
                    OnPropertyChanged();
                }
            }
        }



        public RelayCommand SideManuCommand
        {
            get { return sideMenuCommand; }
            set
            {
                if (sideMenuCommand != value)
                {
                    sideMenuCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private void ExecuteSideMenuClick()
        {

            var sideMenuPage = new SideMenuPage(LoggedInUser);
            GuideMainWindow.MainFrame.Navigate(sideMenuPage);

        }




        private bool AreAllKeyPointsChecked(List<KeyPoint> keyPoints)
        {
            int checkedKeyPointsCount = keyPoints.Count(kp => kp.IsChecked);
            return checkedKeyPointsCount == keyPoints.Count;
        }
        private void Check(object parameter)
        {
            KeyPoint keyPoint = parameter as KeyPoint;
            keyPoint.IsChecked = true;
            keyPointService.Update(keyPoint);
            liveTourService.CheckKeyPoint(tourId, keyPoint);

            var keypoints = keyPointService.GetTourKeyPoints(tourId);
            KeyPoints = new ObservableCollection<KeyPoint>(keypoints);

            
            if (AreAllKeyPointsChecked(keypoints))
            {
                ExecuteFinishTourClick();
            }
        }
        private void ExecuteFinishTourClick()
        {
            var touristsOnTour = tourReservationService.GetByTourId(tourId);
            foreach (var tourist in touristsOnTour)
            {
                tourist.IsOnTour = false;
                tourReservationService.UpdateReservation(tourist);
            }
            var keyPoints = keyPointService.GetTourKeyPoints(tourId);
            foreach (var keyPoint in keyPoints)
            {
                keyPoint.IsChecked = true;
                keyPointService.Update(keyPoint);
            }
            liveTourService.FinishTour(tourId);
            MessageBox.Show("Tour successfully finished.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            GuideMainPage main = new GuideMainPage(LoggedInUser);
            GuideMainWindow.MainFrame.Navigate(main);

        }
        public TourReservation SelectedTourist
        {
            get { return selectedTourist; }
            set { selectedTourist = value; OnPropertyChanged(); }
        }
        private void ExecuteAddTouristClick()
        {
            var keyPoint = keyPointService.GetLastActiveKeyPoint();
            selectedTourist.JoinedKeyPoint = keyPoint;
            selectedTourist.IsOnTour = true;
            tourReservationService.UpdateReservation(selectedTourist);
            MessageBox.Show($"Tourist {selectedTourist.TouristFirstName} added to tour at {keyPoint.Name}.");
            if (tourReservationService.IsUserOnTour(selectedTourist.UserId, selectedTourist.TourId))
            {
                IGuideRepository guideRepository = Injector.CreateInstance<IGuideRepository>();
                var Guide = guideRepository.GetById(LoggedInUser.Id);
                List<string> addedPersons = new List<string>();
                addedPersons.Add(selectedTourist.TouristFirstName + " " + selectedTourist.TouristLastName);
                touristGuideNotificationRepository.Save(new TouristGuideNotification(selectedTourist.UserId, Guide.Id, selectedTourist.TourId, addedPersons, System.DateTime.Now, NotificationType.TouristJoined, keyPoint.Name, Guide.FirstName+' '+Guide.LastName, SelectedTour.Name));
            }
            var tourists = liveTourService.GetTouristsByTourId(tourId).Where(t => !t.IsOnTour).ToList();
            Tourists = new ObservableCollection<TourReservation>(tourists);
        }
    }
}