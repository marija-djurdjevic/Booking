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

namespace BookingApp.WPF.ViewModels.TouristViewModels
{
    public class TouristNotificationsViewModel : BindableBase
    {
        public static ObservableCollection<Tuple<TouristGuideNotification, string>> Notifications { get; set; }
        public User LoggedInUser { get; set; }

        private readonly TouristGuideNotificationService touristGuideNotificationService;

        public TouristNotificationsViewModel(User loggedInUser)
        {
            touristGuideNotificationService = new TouristGuideNotificationService(Injector.CreateInstance<ITouristGuideNotificationRepository>());
            Notifications = new ObservableCollection<Tuple<TouristGuideNotification, string>>();

            LoggedInUser = loggedInUser;
            GetMyNotifications();
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
