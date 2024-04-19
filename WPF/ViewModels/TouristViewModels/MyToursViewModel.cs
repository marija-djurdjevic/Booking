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

namespace BookingApp.WPF.ViewModel.TouristViewModel
{
    public class MyToursViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Tuple<TourDto, Visibility, string>> tours;
        public ObservableCollection<Tuple<TourDto, Visibility, string>> Tours
        {
            get { return tours; }
            set
            {
                tours = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Tuple<TourDto, Visibility, string>> finishedTours;
        public ObservableCollection<Tuple<TourDto, Visibility, string>> FinishedTours
        {
            get { return finishedTours; }
            set
            {
                finishedTours = value;
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
                OnPropertyChanged(nameof(NoMyToursTextVisibility));
            }
        }
        private Visibility noFinishedToursTextVisibility;
        public Visibility NoFinishedToursTextVisibility
        {
            get { return noFinishedToursTextVisibility; }
            set
            {
                noFinishedToursTextVisibility = value;
                OnPropertyChanged(nameof(NoFinishedToursTextVisibility));
            }
        }
        private Visibility noActiveToursTextVisibility;
        public Visibility NoActiveToursTextVisibility
        {
            get { return noActiveToursTextVisibility; }
            set
            {
                noActiveToursTextVisibility = value;
                OnPropertyChanged(nameof(NoActiveToursTextVisibility));
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
        public MyToursViewModel(User loggedInUser)
        {
            LoggedInUser = loggedInUser;
            SelectedTour = new Tuple<TourDto, Visibility, string>(new TourDto(), Visibility.Visible, "");
            Tours = new ObservableCollection<Tuple<TourDto, Visibility, string>>();
            ActiveTours = new ObservableCollection<Tuple<TourDto, List<KeyPoint>, KeyPoint>>();
            FinishedTours = new ObservableCollection<Tuple<TourDto, Visibility, string>>();
            myToursService = new MyToursService(Injector.CreateInstance<ITourRepository>(), Injector.CreateInstance<ITourReservationRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            FillCollections();
        }
        public void FillCollections()
        {
            Tours.Clear();
            foreach (var tour in myToursService.GetMyReserved(LoggedInUser.Id))
            {
                string tourStatusMessage = myToursService.GetTourStatusMessage(LoggedInUser.Id, tour.Id);
                Tours.Add(new Tuple<TourDto, Visibility, string>(new TourDto(tour), IsRateButtonVisible(tour.Id, LoggedInUser.Id), tourStatusMessage));
            }
            NoMyToursTextVisibility = Tours.Count() < 1 ? Visibility.Visible : Visibility.Collapsed;

            FinishedTours.Clear();
            foreach (var tour in myToursService.GetMyFinishedTours(LoggedInUser.Id))
            {
                string tourStatusMessage = myToursService.GetTourStatusMessage(LoggedInUser.Id, tour.Id);
                FinishedTours.Add(new Tuple<TourDto, Visibility, string>(new TourDto(tour), IsRateButtonVisible(tour.Id, LoggedInUser.Id), tourStatusMessage));
            }
            NoFinishedToursTextVisibility = FinishedTours.Count() < 1 ? Visibility.Visible : Visibility.Collapsed;

            ActiveTours.Clear();
            foreach (Tour tour in myToursService.GetMyActiveReserved(LoggedInUser.Id))
            {
                TourDto tourDto = new TourDto(tour);
                List<KeyPoint> keyPoints = tourDto.KeyPoints.Skip(1).Take(tourDto.KeyPoints.Count - 2).ToList();
                KeyPoint endPoint = tour.KeyPoints.Last();
                ActiveTours.Add(new Tuple<TourDto, List<KeyPoint>, KeyPoint>(tourDto, keyPoints, endPoint));
            }
            NoActiveToursTextVisibility = ActiveTours.Count() < 1 ? Visibility.Visible : Visibility.Collapsed;
        }
        private Visibility IsRateButtonVisible(int tourId, int userId)
        {
            return myToursService.CanTouristRateTour(userId, tourId) ? Visibility.Visible : Visibility.Collapsed;
        }
        public void OpenInbox()
        {
            new NotificationsWindow(LoggedInUser).ShowDialog();
        }
        public void RateTour(object sender)
        {
            Button rateButton = (Button)sender;
            Tuple<TourDto, Visibility, string> tupl = (Tuple<TourDto, Visibility, string>)rateButton.DataContext;
            new RateTourWindow(tupl.Item1, LoggedInUser).ShowDialog();
            FillCollections();
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
    }
}