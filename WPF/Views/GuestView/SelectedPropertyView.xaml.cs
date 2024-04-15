using BookingApp.Domain.Models;
using BookingApp.Aplication.Dto;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.GuestView
{
    /// <summary>
    /// Interaction logic for SelectedPropertyView.xaml
    /// </summary>
    public partial class SelectedPropertyView : Page
    {
        public static PropertyRepository PropertyRepository = new PropertyRepository();
        public static PropertyReservationRepository PropertyReservationRepository = new PropertyReservationRepository();
        public Guest LoggedInGuest { get; set; }
        public Property SelectedProperty { get; set; }
        public SelectedPropertyView(Property selectedProperty, Guest guest, PropertyRepository propertyRepository, PropertyReservationRepository propertyReservationRepository)
        {
            InitializeComponent();
            DataContext = this;
            SelectedProperty = selectedProperty;
            LoggedInGuest = guest;
            PropertyRepository = propertyRepository;
            PropertyReservationRepository = propertyReservationRepository;
        }

        private void MakeReservation_Clik(object sender, RoutedEventArgs e)
        {
            PropertyBooking propertybooking = new PropertyBooking(SelectedProperty, LoggedInGuest, PropertyRepository, PropertyReservationRepository);
            NavigationService.Navigate(propertybooking);
        }
    }
}
