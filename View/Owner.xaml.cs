using BookingApp.Dto;
using BookingApp.Model;
using BookingApp.Repository;
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
using BookingApp.Service;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for Owner.xaml
    /// </summary>
    public partial class Owner : Window
    {
        public List<PropertyReservationDto> PropertyReservations { get; set; }
        public PropertyReservation SelectedReservation { get; set; }
        public User LoggedInUser { get; set; }
        public PropertyReservationRepository _propertyReservationRepository { get; set; }
        public ReviewRepository _reviewRepository;
        private List<Notification> _notifications;
        public Owner()
        {
            InitializeComponent();
           // DataContext = this;
            //SelectedReservation = new PropertyReservation();
            _notifications = new List<Notification>();

           // _propertyReservationRepository = new PropertyReservationRepository();
           // ReservationDataGrid.ItemsSource = _propertyReservationRepository.GetAllPropertyReservation();
            //PropertyReservations = new List<PropertyReservationDto>();
            //_reviewRepository = new ReviewRepository();
            ReservationsFrame.Navigate(new ReservationsPage());
            PropertyFrame.Navigate(new PropertyPage());
            //MainFrame.NavigationService.Navigate(new OwnerHomePage());


        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Mora se okruziti if-om radi zastite od greske, tj. exception-a kada nije moguce navigirati nazad
            /*if (MainFrame.CanGoBack)
            {
                //MainFrame.GoBack();
                MainFrame.NavigationService.GoBack();
                // ovo je skraceni zapis za MainFrame.NavigationService.GoBack();
            }*/
        }

        private void NotificationsButton_Click(object sender, RoutedEventArgs e)
        {
            NotificationService notificationManager = new NotificationService();
            var unratedGuests = notificationManager.GetUnratedGuests();

            NotificationWindow notificationsWindow = new NotificationWindow(unratedGuests);
            notificationsWindow.ShowDialog();
        }
       /* private void AddProperty_Click(object sender, RoutedEventArgs e)
        {
            AddProperty addProperty = new AddProperty();
            //MainFrame.Navigate(addProperty);
            //this.NavigationService.Navigate(new Uri("View/AddProperty.xaml", UriKind.RelativeOrAbsolute));
            //addProperty.Show();
            // MainFrame.NavigationService.Navigate(new Uri("View/Owner.xaml", UriKind.Relative));
            //MainFrame.NavigationService.Navigate(new Uri("View/AddProperty.xaml", UriKind.Relative));
            //MainFrame.NavigationService.Navigate(new Uri("View/AddProperty.xaml", UriKind.Relative));
            this.NavigationService.Navigate(new Uri("View/AddProperty.xaml", UriKind.RelativeOrAbsolute));
            //AddPropertyFrame.Navigate(new Uri("View/AddProperty.xaml", UriKind.Relative));


        }*/


       
    }
}
            
                

               
            
            
            
           
        

    

