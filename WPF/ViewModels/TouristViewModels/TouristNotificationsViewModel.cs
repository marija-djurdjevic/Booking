using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Models;
using BookingApp.Domain.Models.Enums;
using BookingApp.Aplication.UseCases;
using BookingApp.Aplication;
using BookingApp.Domain.RepositoryInterfaces;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using BookingApp.View.TouristView;
using BookingApp.Aplication.Dto;

namespace BookingApp.WPF.ViewModels.TouristViewModels
{
    public class TouristNotificationsViewModel : BindableBase
    {
        public static ObservableCollection<Tuple<TouristGuideNotification, string>> Notifications { get; set; }
        public User LoggedInUser { get; set; }

        private readonly TouristGuideNotificationService touristGuideNotificationService;
        private readonly TourService tourService; 
        public RelayCommand HelpCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public RelayCommand<object> ShowDetailsCommand { get; set; }

        public TouristNotificationsViewModel(User loggedInUser)
        {
            touristGuideNotificationService = new TouristGuideNotificationService(Injector.CreateInstance<ITouristGuideNotificationRepository>());
            tourService = new TourService(Injector.CreateInstance<ITourRepository>(),Injector.CreateInstance<ILiveTourRepository>());
            Notifications = new ObservableCollection<Tuple<TouristGuideNotification, string>>();

            HelpCommand = new RelayCommand(Help);
            CloseCommand = new RelayCommand(CloseWindow);
            ShowDetailsCommand = new RelayCommand<object>(ShowTourDetails);

            LoggedInUser = loggedInUser;
            GetMyNotifications();
        }

        private void ShowTourDetails(object parametar)
        {
            Tuple<TouristGuideNotification, string> touristGuideNotification = (Tuple<TouristGuideNotification, string>)parametar;
            Tour createdTour = tourService.GetTourById(touristGuideNotification.Item1.TourId);
            if(createdTour!=null)
                new TourBookingWindow(new TourDto(createdTour), LoggedInUser.Id).ShowDialog();
        }

        private void CloseWindow()
        {
            Messenger.Default.Send(new NotificationMessage("CloseNotificationsWindowMessage"));
        }

        private void Help()
        {

        }

        public void GetMyNotifications()
        {
            Notifications.Clear();
            foreach (var notification in touristGuideNotificationService.GetByUserId(LoggedInUser.Id))
            {
                string showingText = "";
                if (notification.Type == NotificationType.TouristJoined)
                {
                    showingText = string.Join(", ", notification.AddedPersons);
                }
                Notifications.Add(new Tuple<TouristGuideNotification, string>(notification, showingText));
            }
            touristGuideNotificationService.MarkAllUserMessagesAsRead(LoggedInUser.Id);
        }
    }
}
