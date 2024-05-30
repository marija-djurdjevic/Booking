using BookingApp.Aplication;
using BookingApp.Aplication.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.View;
using BookingApp.WPF.Views.GuestView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.GuestView
{
    /// <summary>
    /// Interaction logic for Guest.xaml
    /// </summary>
    public partial class GuestMainWindow : Window
    {
        public static PropertyRepository PropertyRepository = new PropertyRepository();
        public static GuestRepository GuestRepository = new GuestRepository();
        public static PropertyReservationRepository PropertyReservationRepository = new PropertyReservationRepository();
        public static GuestNotificationsRepository GuestNotificationRepository = new GuestNotificationsRepository();
        public List<PropertyReservation> GuestsReservations { get; set; }
        public GuestNotification GuestNotification { get; set; }
        public GuestService guestService;

        public User LoggedInUser { get; set; }
        public Guest Guest { get; set; }
        private Stack<Page> pageStack = new Stack<Page>();
        public GuestMainWindow(User user)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            guestService = new GuestService(Injector.CreateInstance<IGuestRepository>(), Injector.CreateInstance<IGuestNotificationRepository>(), Injector.CreateInstance<IPropertyReservationRepository>());
            GuestRepository = new GuestRepository();
            PropertyReservationRepository = new PropertyReservationRepository();
            GuestNotificationRepository = new GuestNotificationsRepository();
            GuestsReservations = new List<PropertyReservation>();
            GuestNotification = new GuestNotification();
            LoggedInUser = user;
            Guest = guestService.GetGuestById(LoggedInUser.Id);
            guestService.CheckSuperGuestExpiryDate(Guest, GuestsReservations);
            frame.Navigate(new PropertyView(LoggedInUser));
        }

        private void MenuBurger_Click(object sender, RoutedEventArgs e)
        {
            ActionFrame.Navigate(new ActionList(LoggedInUser));
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();
        }

        private void Notifications_Click(object sender, RoutedEventArgs e)
        {
            GuestsNotifications guestsNotifications = new GuestsNotifications(Guest);
            ActionFrame.Navigate(new GuestsNotifications(Guest));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var currentFrameContent = frame.Content as Page;
            var currentActionFrameContent = ActionFrame.Content as Page;

            if (currentFrameContent is ForumCommenting || currentActionFrameContent is ForumCommenting)
            {
                ActionFrame.Navigate(new ForumList(Guest));
            }
            else if (frame.CanGoBack)
            {
                frame.GoBack();
            }
            else if (ActionFrame.CanGoBack)
            {
                ActionFrame.GoBack();
            }
            else
            {
                frame.Navigate(new PropertyView(LoggedInUser));
            }
        }
    }
}
