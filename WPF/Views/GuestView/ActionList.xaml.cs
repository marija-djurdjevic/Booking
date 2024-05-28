﻿using BookingApp.Domain.Models;
using BookingApp.Repositories;
using BookingApp.View;
using BookingApp.WPF.Views.GuestView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.GuestView
{
    /// <summary>
    /// Interaction logic for ActionList.xaml
    /// </summary>
    public partial class ActionList : Page
    {
        public GuestRepository GuestRepository { get; set; }
        public Guest LoggedInGuest { get; set; }

        public ActionList(User loggedInUser)
        {
            InitializeComponent();
            GuestRepository = new GuestRepository();
            LoggedInGuest = GuestRepository.GetByUserId(loggedInUser.Id);
        }

        private void Reservations_Click(object sender, RoutedEventArgs e)
        {
            ReservationsView reservationsView = new ReservationsView(LoggedInGuest);
            NavigationService.Navigate(reservationsView);
        }

        private void ReviewScore_Click(object sender, RoutedEventArgs e)
        {
            GuestReviewScoreView guestReviewScore = new GuestReviewScoreView(LoggedInGuest);
            NavigationService.Navigate(guestReviewScore);   
        }

        private void Notifications_Click(object sender, RoutedEventArgs e)
        {
            GuestsNotifications guestsNotifications = new GuestsNotifications(LoggedInGuest);
            NavigationService.Navigate(guestsNotifications);
        }

        private void Stays_Click(object sender, RoutedEventArgs e)
        {
            PropertyView propertyView = new PropertyView(LoggedInGuest);
            NavigationService.Navigate(propertyView);
        }

        private void Forums_Click(object sender, RoutedEventArgs e)
        {
            ForumList forumList = new ForumList(LoggedInGuest);
            NavigationService.Navigate(forumList);
        }
    }
}
