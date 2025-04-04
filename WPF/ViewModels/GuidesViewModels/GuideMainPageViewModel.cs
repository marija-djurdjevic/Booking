﻿using BookingApp.Aplication;
using BookingApp.Aplication.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.View;
using BookingApp.View.GuideView;
using BookingApp.WPF.Views.GuideView;
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
using Xceed.Wpf.Toolkit;

namespace BookingApp.WPF.ViewModels.GuidesViewModel
{
    public class GuideMainPageViewModel : BaseViewModel
    {
        private readonly TourService tourService;
        private readonly KeyPointService keyPointService;
        private readonly LiveTourService liveTourService;
        private readonly TourReservationService tourReservationService;
        private readonly VoucherService voucherService;
        private readonly TouristService touristService;
        private readonly TourCancellationService tourCancellationService;
        private ObservableCollection<Tour> todayTours;
        private ObservableCollection<Tour> upcomingTours;
        private ObservableCollection<Tour> finishedTours;
        private Tour selectedTour;
        private RelayCommand createTourClickCommand;
        private RelayCommand startTourClickCommand;
        private RelayCommand reviewTourClickCommand;
        private RelayCommand cancelTourClickCommand;
        private LiveTourRepository liveTourRepository;
        private RelayCommand sideMenuCommand;
        private RelayCommand tourDetails;
        public User LoggedInUser { get; set; }
        public GuideMainPageViewModel(User loggedInUser)
        {
            LoggedInUser = loggedInUser;
            tourService = new TourService(Injector.CreateInstance<ITourRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            keyPointService = new KeyPointService(Injector.CreateInstance<IKeyPointRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            liveTourService = new LiveTourService(Injector.CreateInstance<ILiveTourRepository>(), Injector.CreateInstance<IKeyPointRepository>());
            liveTourRepository = new LiveTourRepository();
            tourReservationService = new TourReservationService(Injector.CreateInstance<ITourReservationRepository>());
            voucherService = new VoucherService(Injector.CreateInstance<IVoucherRepository>());
            touristService = new TouristService(Injector.CreateInstance<ITouristRepository>(), Injector.CreateInstance<ITouristGuideNotificationRepository>(), Injector.CreateInstance<IVoucherRepository>());
            tourCancellationService = new TourCancellationService(liveTourService, tourReservationService, tourService, keyPointService, voucherService, touristService);
            createTourClickCommand = new RelayCommand(ExecuteCreateTourClick);
            startTourClickCommand = new RelayCommand(ExecuteStartTourClick);
            reviewTourClickCommand = new RelayCommand(ExecuteReviewTourClick);
            cancelTourClickCommand = new RelayCommand(ExecuteCancelTourClick);
            sideMenuCommand = new RelayCommand(ExecuteSideMenuClick);
            tourDetails = new RelayCommand(ExecuteTourDetails);
            LoggedInUser = loggedInUser;
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
            set
            {
                selectedTour = value;
                OnPropertyChanged();
                
            }
        }

        
        private void LoadTours()
        {
            TodayTours = new ObservableCollection<Tour>(tourService.GetTodayTours().Where(tour => tour.GuideId == LoggedInUser.Id));
            UpcomingTours = new ObservableCollection<Tour>(tourService.GetUpcomingTours().Where(tour => tour.GuideId == LoggedInUser.Id)); 
            var finishedLiveTours = liveTourRepository.GetFinishedTours();
            FinishedTours = new ObservableCollection<Tour>(finishedLiveTours.Select(tour => tourService.GetTourById(tour.TourId)).Where(tour => tour.GuideId == LoggedInUser.Id));
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


        public RelayCommand TourDetails
        {
            get { return tourDetails; }
            set
            {
                if (tourDetails != value)
                {
                    tourDetails = value;
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

            var sideManuPage=new SideMenuPage(LoggedInUser);
            GuideMainWindow.MainFrame.Navigate(sideManuPage);

        }

        


        private void ExecuteCreateTourClick()
        {
            var createTourPage = new CreateTourPage(LoggedInUser);
            GuideMainWindow.MainFrame.Navigate(createTourPage);
        }


        private void ExecuteTourDetails(object parameter)
        {
            if (parameter != null && int.TryParse(parameter.ToString(), out int tourId))
            {
                var details = new TourDetails(tourId,LoggedInUser);
                GuideMainWindow.MainFrame.Navigate(details);
            }
        }

        private void ExecuteStartTourClick(object parameter)
        {
            if (parameter != null && parameter is int tourId)
            {
                var tour = tourService.GetTourById(tourId);
                if (tour != null && liveTourService.HasLiveTour())
                {
                    liveTourService.ActivateTour(tourId);
                    liveTourService.CheckFirstKeyPoint(tourId);
                    liveTourService.SaveChanges();
                    System.Windows.MessageBox.Show("Tour successfully started.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    LiveTourPage liveTourPage = new LiveTourPage(tourId,LoggedInUser);
                    GuideMainWindow.MainFrame.Navigate(liveTourPage);
                }

                else
                {
                    System.Windows.MessageBox.Show("Completed the previously started tour.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    int id =liveTourService.GetLiveTourId();
                    LiveTourPage liveTourPage = new LiveTourPage(id, LoggedInUser);
                    GuideMainWindow.MainFrame.Navigate(liveTourPage);

                }
            }
        }
        private void ExecuteReviewTourClick(object parameter)
        {
            if (parameter != null && parameter is int tourId)
            {
                TourReview touristsReviewPage = new TourReview(tourId,LoggedInUser);
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
                tourCancellationService.CancelTour(tour, tourKeyPoints, tourReservation,LoggedInUser.Id);
               
                LoadTours();
            }
        }
    }
}
