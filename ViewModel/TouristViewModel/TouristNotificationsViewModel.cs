using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BookingApp.Model.Enums;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.TouristViewModel
{
    public class TouristNotificationsViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public static ObservableCollection<Tuple<TouristGuideNotification,string>> Notifications { get; set; }
        public User LoggedInUser { get; set; }

        private readonly TouristGuideNotificationRepository touristGuideNotificationRepository;

        public TouristNotificationsViewModel(User loggedInUser)
        {
            touristGuideNotificationRepository = new TouristGuideNotificationRepository();
            Notifications = new ObservableCollection<Tuple<TouristGuideNotification, string>>();

            LoggedInUser = loggedInUser;
            GetMyNotifications();
        }

        public void GetMyNotifications()
        {
            Notifications.Clear();
            foreach (var notification in touristGuideNotificationRepository.GetByUserId(LoggedInUser.Id))
            {
                string showingText = "";
                if (notification.Type == NotificationType.TouristJoined)
                {
                    showingText=string.Join(", ", notification.AddedPersons);
                }
                Notifications.Add(new Tuple<TouristGuideNotification,string>(notification,showingText));
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
