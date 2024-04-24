using BookingApp.Repositories;
using BookingApp.View.TouristView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using BookingApp.Domain.Models;
using BookingApp.Aplication.UseCases;
using BookingApp.Aplication.Dto;
using BookingApp.Aplication;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Command;

namespace BookingApp.WPF.ViewModels.TouristViewModels
{
    public class MyToursViewModel : BindableBase
    {
        private ObservableCollection<Tuple<TourDto, Visibility, string>> tours;
        public ObservableCollection<Tuple<TourDto, Visibility, string>> Tours
        {
            get { return tours; }
            set
            {
                tours = value;
                OnPropertyChanged(nameof(Tours));
            }
        }
        private ObservableCollection<Tuple<TourDto, Visibility, string>> finishedTours;
        public ObservableCollection<Tuple<TourDto, Visibility, string>> FinishedTours
        {
            get { return finishedTours; }
            set
            {
                finishedTours = value;
                OnPropertyChanged(nameof(FinishedTours));
            }
        }
        private ObservableCollection<Tuple<TourDto, List<KeyPoint>, KeyPoint>> activeTours;
        public ObservableCollection<Tuple<TourDto, List<KeyPoint>, KeyPoint>> ActiveTours
        {
            get { return activeTours; }
            set
            {
                activeTours = value;
                OnPropertyChanged(nameof(ActiveTours));
            }
        }

        private int unreadNotificationCount;
        public int UnreadNotificationCount
        {
            get { return unreadNotificationCount; }
            set
            {
                unreadNotificationCount = value;
                OnPropertyChanged(nameof(UnreadNotificationCount));
            }
        }

        private string allToursSelectedSort;
        public string AllToursSelectedSort
        {
            get { return allToursSelectedSort; }
            set
            {
                allToursSelectedSort = value;
                OnPropertyChanged(nameof(AllToursSelectedSort));
            }
        }
        private string finishedToursSelectedSort;
        public string FinishedToursSelectedSort
        {
            get { return finishedToursSelectedSort; }
            set
            {
                finishedToursSelectedSort = value;
                OnPropertyChanged(nameof(FinishedToursSelectedSort));
            }
        }

        public User LoggedInUser { get; set; }
        private Tuple<TourDto, Visibility, string> selectedTour;
        public Tuple<TourDto, Visibility, string> SelectedTour
        {
            get { return selectedTour; }
            set
            {
                selectedTour = value;
                OnPropertyChanged(nameof(SelectedTour));
            }
        }
        private readonly MyToursService myToursService;
        private readonly TouristGuideNotificationService notificationService;

        public RelayCommand<object> RateCommand { get; set; }
        public RelayCommand InboxCommand { get; set; }
        public RelayCommand HelpCommand { get; set; }
        public RelayCommand AllToursSortingCommand { get; set; }
        public RelayCommand FinishedToursSortingCommand { get; set; }
        public MyToursViewModel(User loggedInUser)
        {
            LoggedInUser = loggedInUser;
            SelectedTour = new Tuple<TourDto, Visibility, string>(new TourDto(), Visibility.Visible, "");
            Tours = new ObservableCollection<Tuple<TourDto, Visibility, string>>();
            ActiveTours = new ObservableCollection<Tuple<TourDto, List<KeyPoint>, KeyPoint>>();
            FinishedTours = new ObservableCollection<Tuple<TourDto, Visibility, string>>();
            myToursService = new MyToursService(Injector.CreateInstance<ITourRepository>(), Injector.CreateInstance<ITourReservationRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            notificationService = new TouristGuideNotificationService(Injector.CreateInstance<ITouristGuideNotificationRepository>());

            UnreadNotificationCount= notificationService.GetUnreadNotificationCount(LoggedInUser.Id);
            FillCollections();
            HelpCommand = new RelayCommand(Help);
            InboxCommand = new RelayCommand(OpenInbox);
            RateCommand = new RelayCommand<object>(RateTour);
            AllToursSortingCommand = new RelayCommand(SortingAllToursSelectionChanged);
            FinishedToursSortingCommand = new RelayCommand(SortingFinishedToursSelectionChanged);
        }

        private void Help()
        {
            
        }

        public void FillCollections()
        {
            Tours.Clear();
            foreach (var tour in myToursService.GetMyReserved(LoggedInUser.Id))
            {
                string tourStatusMessage = myToursService.GetTourStatusMessage(LoggedInUser.Id, tour.Id);
                Tours.Add(new Tuple<TourDto, Visibility, string>(new TourDto(tour), IsRateButtonVisible(tour.Id, LoggedInUser.Id), tourStatusMessage));
            }

            FinishedTours.Clear();
            foreach (var tour in myToursService.GetMyFinishedTours(LoggedInUser.Id))
            {
                string tourStatusMessage = myToursService.GetTourStatusMessage(LoggedInUser.Id, tour.Id);
                FinishedTours.Add(new Tuple<TourDto, Visibility, string>(new TourDto(tour), IsRateButtonVisible(tour.Id, LoggedInUser.Id), tourStatusMessage));
            }

            ActiveTours.Clear();
            foreach (Tour tour in myToursService.GetMyActiveReserved(LoggedInUser.Id))
            {
                TourDto tourDto = new TourDto(tour);
                List<KeyPoint> keyPoints = tourDto.KeyPoints.Skip(1).Take(tourDto.KeyPoints.Count - 2).ToList();
                KeyPoint endPoint = tour.KeyPoints.Last();
                ActiveTours.Add(new Tuple<TourDto, List<KeyPoint>, KeyPoint>(tourDto, keyPoints, endPoint));
            }
        }
        private Visibility IsRateButtonVisible(int tourId, int userId)
        {
            return myToursService.CanTouristRateTour(userId, tourId) ? Visibility.Visible : Visibility.Collapsed;
        }
        public void OpenInbox()
        {
            new NotificationsWindow(LoggedInUser).ShowDialog();
            UnreadNotificationCount = notificationService.GetUnreadNotificationCount(LoggedInUser.Id);
        }
        public void RateTour(object sender)
        {
            Tuple<TourDto, Visibility, string> tupl = (Tuple<TourDto, Visibility, string>)sender;
            new RateTourWindow(tupl.Item1, LoggedInUser).ShowDialog();
            FillCollections();
        }
        public void SortingAllToursSelectionChanged()
        {
            myToursService.SortTours(Tours, AllToursSelectedSort);
        }

        public void SortingFinishedToursSelectionChanged()
        {
            myToursService.SortTours(FinishedTours, FinishedToursSelectedSort);
        }
    }
}