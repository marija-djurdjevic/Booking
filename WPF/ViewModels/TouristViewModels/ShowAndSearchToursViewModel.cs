using BookingApp.Repositories;
using BookingApp.View.TouristView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using BookingApp.Domain.Models;
using BookingApp.Aplication.UseCases;
using BookingApp.Aplication.Dto;
using BookingApp.Aplication;
using BookingApp.Domain.RepositoryInterfaces;
using GalaSoft.MvvmLight.Messaging;
using BookingApp.Command;

namespace BookingApp.WPF.ViewModels.TouristViewModels
{
    public class ShowAndSearchToursViewModel : BindableBase
    {
        public static ObservableCollection<TourDto> Tours { get; set; }
        public User LoggedInUser { get; set; }
        public TourDto SelectedTour { get; set; }

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

        private readonly TourService tourService;
        private readonly KeyPointService keyPointService;
        private readonly TouristGuideNotificationService notificationService;

        private bool _isShowAllButtonVisible;

        public RelayCommand SearchCommand { get; set; }
        public RelayCommand ShowAllCommand { get; set; }
        public RelayCommand InboxCommand { get; set; }
        public RelayCommand HelpCommand { get; set; }
        public RelayCommand ScrollToTopCommand { get; private set; }
        public RelayCommand ScrollToBottomCommand { get; private set; }
        public RelayCommand ScrollDownCommand { get; private set; }
        public RelayCommand ScrollUpCommand { get; private set; }
        public RelayCommand<object> SelectedCardCommand { get; set; }

        public ShowAndSearchToursViewModel(User loggedInUser)
        {
            tourService = new TourService(Injector.CreateInstance<ITourRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            keyPointService = new KeyPointService(Injector.CreateInstance<IKeyPointRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            notificationService = new TouristGuideNotificationService(Injector.CreateInstance<ITouristGuideNotificationRepository>());
            Tours = new ObservableCollection<TourDto>();
            SelectedTour = new TourDto();

            IsShowAllButtonVisible = false;
            LoggedInUser = loggedInUser;
            GetAllTours();
            UnreadNotificationCount = notificationService.GetUnreadNotificationCount(LoggedInUser.Id);
            Messenger.Default.Register<NotificationMessage>(this, ShowAllButton);

            SearchCommand = new RelayCommand(Search);
            ShowAllCommand = new RelayCommand(ShowAllTours);
            InboxCommand = new RelayCommand(OpenInbox);
            HelpCommand = new RelayCommand(Help);
            ScrollToTopCommand = new RelayCommand(ScrollToTop);
            ScrollToBottomCommand = new RelayCommand(ScrollToBottom);
            ScrollDownCommand = new RelayCommand(ScrollDown);
            ScrollUpCommand = new RelayCommand(ScrollUp);
            SelectedCardCommand = new RelayCommand<object>(SelectedTourCard);
        }
        private void ScrollUp()
        {
            Messenger.Default.Send(new NotificationMessage("ScrollToursUp"));
        }

        private void ScrollDown()
        {
            Messenger.Default.Send(new NotificationMessage("ScrollToursDown"));
        }

        private void ScrollToBottom()
        {
            Messenger.Default.Send(new NotificationMessage("ScrollToursToBottom"));
        }

        private void ScrollToTop()
        {
            Messenger.Default.Send(new NotificationMessage("ScrollToursToTop"));
        }

        public bool IsShowAllButtonVisible
        {
            get => _isShowAllButtonVisible;
            set
            {
                if (value != _isShowAllButtonVisible)
                {
                    _isShowAllButtonVisible = value;
                    OnPropertyChanged("IsShowAllButtonVisible");
                }
            }
        }
        private void Help()
        {

        }

        public void GetAllTours()
        {
            Tours.Clear();
            foreach (var tour in tourService.GetAllSorted())
            {
                Tours.Add(new TourDto(tour));
            }
        }

        public void SelectedTourCard(object sender)
        {
            SelectedTour = (TourDto)sender;
            SelectedTour.KeyPoints = keyPointService.GetTourKeyPoints(SelectedTour.Id);
            if (SelectedTour.MaxTouristNumber > 0)
            {
                TourBookingWindow tourBookingWindow = new TourBookingWindow(SelectedTour, LoggedInUser.Id);
                tourBookingWindow.ShowDialog();
            }
            else
            {
                Style style = Application.Current.FindResource("MessageStyle") as Style;
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("The tour is fully booked. Please select an alternative tour from this city!", "Booking", MessageBoxButton.OK, MessageBoxImage.Information, style);
                ShowUnbookedToursInCity();
            }
        }

        private void ShowAllButton(NotificationMessage message)
        {
            if (message.Notification == "ShowAllButtonMessage")
            {
                IsShowAllButtonVisible=true;
            }
        }

        public void Search()
        {
            new SearchWindow(Tours).ShowDialog();
        }

        public void OpenInbox()
        {
            NotificationsWindow notificationsWindow = new NotificationsWindow(LoggedInUser);
            notificationsWindow.ShowDialog();
            UnreadNotificationCount = notificationService.GetUnreadNotificationCount(LoggedInUser.Id);
        }

        public void ShowAllTours()
        {
            IsShowAllButtonVisible = false;
            GetAllTours();
        }

        private void ShowUnbookedToursInCity()
        {
            List<Tour> unBookedToursInCity = tourService.GetUnBookedToursInCity(SelectedTour.LocationDto.City);

            if (unBookedToursInCity.Count > 0)
            {
                IsShowAllButtonVisible = true;
                Tours.Clear();
                foreach (var tour in unBookedToursInCity)
                {
                    Tours.Add(new TourDto(tour));
                }
            }
            else
            {
                Style style = Application.Current.FindResource("MessageStyle") as Style;
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("There are no tours from that city!", "Booking", MessageBoxButton.OK, MessageBoxImage.Information, style);
            }
        }

        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
