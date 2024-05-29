using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Navigation;
using BookingApp.Domain.Models;
using BookingApp.Aplication.UseCases;
using BookingApp.Aplication.Dto;
using BookingApp.Aplication;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Views.OwnerView;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for Owner.xaml
    /// </summary>
    public partial class OwnerWindow : Window
    {
        public List<PropertyReservationDto> PropertyReservations { get; set; }
        public PropertyReservation SelectedReservation { get; set; }
        public User LoggedInUser { get; set; }
        public PropertyReservationRepository _propertyReservationRepository { get; set; }
        public ReviewRepository _reviewRepository;
        private List<Notification> _notifications;
        private OwnerService _ownerService;
        public OwnerRepository ownerRepository { get; set; }
        public OwnerWindow(User LoggedInUser)
        {
            InitializeComponent();
            this.LoggedInUser = LoggedInUser;
            _notifications = new List<Notification>();
            _ownerService = new OwnerService(Injector.CreateInstance<IOwnerReviewRepository>(), Injector.CreateInstance<IOwnerRepository>());
            _ownerService.UpdateOwnerPropertiesBasedOnReviews();
            ReservationsFrame.Navigate(new ReservationsPage());
            PropertyFrame.Navigate(new PropertyPage(LoggedInUser));
            GuestReviewsFrame.Navigate(new GuestReviewsPage());
            RenovationsFrame.Navigate(new RenovationsPage(LoggedInUser));
            AnalyticsFrame.Navigate(new AnalyticsPage(LoggedInUser));
            RecommendationsFrame.Navigate(new RecommendationsPage());

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReservationsFrame.CanGoBack)
            {
                ReservationsFrame.GoBack();
            }
            else if (PropertyFrame.CanGoBack)
            {
                PropertyFrame.GoBack();
            }
        }

        private void NotificationsButton_Click(object sender, RoutedEventArgs e)
        {
             NotificationService notificationManager = new NotificationService();
             var unratedGuests = notificationManager.GetUnratedGuests();
             var canceledReservations = notificationManager.GetCanceledReservations();

             var allNotifications = new List<Notification>();
             allNotifications.AddRange(unratedGuests);
             allNotifications.AddRange(canceledReservations);
             NotificationWindow notificationsWindow = new NotificationWindow(allNotifications);
             notificationsWindow.ShowDialog();
           
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();
        }

    }
}
            
                

               
            
            
            
           
        

    

