using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BookingApp.Dto;
using BookingApp.Repository;

namespace BookingApp.GuestView
{
    /// <summary>
    /// Interaction logic for PropertyViewWindow.xaml
    /// </summary>
    public partial class PropertyView : Window
    {
        public List<PropertyDto> Properties { get; set; }
        public Property SelectedProperty { get; set; }
        public Guest LoggedInGuest { get; set; }
        public GuestRepository GuestRepository { get; set; }
        public PropertyRepository PropertyRepository { get; set; }
        public PropertyReservationRepository PropertyReservationRepository { get; set; }
        public PropertyView(User user)
        {
            InitializeComponent();
            DataContext = this;
            GuestRepository = new GuestRepository();
            LoggedInGuest = GuestRepository.GetByUserId(user.Id);
            SelectedProperty = new Property();
            Properties = new List<PropertyDto>();
            PropertyRepository = new PropertyRepository();
            PropertyReservationRepository = new PropertyReservationRepository();
            PropertyDataGrid.ItemsSource = PropertyRepository.GetAllProperties();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {

            string name = SearchByName.Text.Trim();
            string location = SearchByLocation.Text.Trim();
            string type = SearchByType.Text.Trim();
            string guests = SearchByGuests.Text.Trim();
            string days = SearchByDays.Text.Trim();


            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(location) && string.IsNullOrEmpty(type) && string.IsNullOrEmpty(guests) && string.IsNullOrEmpty(days))
            {
                MessageBox.Show("Please enter at least one search parameter.");
                return;
            }

            List<Property> rezultatiPretrage = SearchProperty(name, location, type, guests, days);


            PropertyDataGrid.ItemsSource = rezultatiPretrage;
            PropertyDataGrid.Items.Refresh();

        }
       private List<Property> SearchProperty(string name, string location, string type, string guests, string days)
        {
            var results = new List<Property>();
            PropertyRepository = new PropertyRepository();
            results = PropertyRepository.GetAllProperties();
            if (!string.IsNullOrEmpty(name))
              {
                results = results.Where(property => property.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
              }
              
              if (!string.IsNullOrEmpty(location))
              {
                results = results.Where(property => (property.Location.City.Contains(location, StringComparison.OrdinalIgnoreCase) || property.Location.Country.Contains(location, StringComparison.OrdinalIgnoreCase))).ToList();
              }

            if (!string.IsNullOrEmpty(type))
            {
                PropertyType result = (PropertyType)Enum.Parse(typeof(PropertyType), type);
                results = results.Where(property => property.Type.Equals(result)).ToList();
            }

            if (!string.IsNullOrEmpty(guests))
            {
                int guestsNumber = Convert.ToInt32(guests);
                results = results.Where(property => property.MaxGuests >= guestsNumber).ToList();
            }

            if (!string.IsNullOrEmpty(days))
            {
                int daysNumber = Convert.ToInt32(days);
                results = results.Where(property => property.MinReservationDays <= daysNumber).ToList();
            }

            return results;
        }

        private void MakeReservation_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedProperty == null)
            {
                MessageBox.Show("Please choose a property to make a reservation!");
            }
            else
            {
                PropertyBooking propertybooking = new PropertyBooking(SelectedProperty, LoggedInGuest, PropertyRepository, PropertyReservationRepository);
                propertybooking.Owner = this;
                propertybooking.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                propertybooking.ShowDialog();
                Close();
            }
            
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            PropertyDataGrid.ItemsSource = PropertyRepository.GetAllProperties();
            PropertyDataGrid.Items.Refresh();
        }
    }

}
