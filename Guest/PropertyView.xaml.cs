using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using BookingApp.Dto;
using BookingApp.DTO;
using BookingApp.Repository;
using static System.Net.Mime.MediaTypeNames;
using System.DirectoryServices.ActiveDirectory;
using BookingApp.View;
namespace BookingApp.Guest
{
    /// <summary>
    /// Interaction logic for PropertyViewWindow.xaml
    /// </summary>
    public partial class PropertyView : Window
    {
        public List<Property> Properties { get; set; }
        public PropertyDto SelectedProperty { get; set; }
        public User LoggedInUser { get; set; }
        public PropertyRepository PropertyRepository { get; set; }
        public PropertyView(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            PropertyRepository = new PropertyRepository();
            Properties = PropertyRepository.GetAllProperties();
            PropertyDataGrid.ItemsSource = this.Properties;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {


            string input1 = SearchByName.Text.Trim();
            string input2 = SearchByLocation.Text.Trim();
            string input3 = SearchByType.Text.Trim();
            string input4 = SearchByGuests.Text.Trim();
            string input5 = SearchByDays.Text.Trim();


            if (string.IsNullOrEmpty(input1) && string.IsNullOrEmpty(input2) && string.IsNullOrEmpty(input3) && string.IsNullOrEmpty(input4) && string.IsNullOrEmpty(input5))
            {
                MessageBox.Show("Molimo unesite barem jedan parametar za pretragu.");
                return;
            }

            List<Property> rezultatiPretrage = SearchProperty(input1, input2, input3, input4, input5);


            PropertyDataGrid.ItemsSource = rezultatiPretrage;
            PropertyDataGrid.Items.Refresh();

        }
       private List<Property> SearchProperty(string input1, string input2, string input3, string input4, string input5)
        {
            var results = new List<Property>();
            PropertyRepository = new PropertyRepository();
            results = PropertyRepository.GetAllProperties();
            if (!string.IsNullOrEmpty(input1))
              {
                results = results.Where(property => property.Name.Contains(input1, StringComparison.OrdinalIgnoreCase)).ToList();
              }
              
              if (!string.IsNullOrEmpty(input2))
              {
                results = results.Where(property => (property.Location.City.Contains(input2, StringComparison.OrdinalIgnoreCase) || property.Location.Country.Contains(input2, StringComparison.OrdinalIgnoreCase))).ToList();
              }

            if (!string.IsNullOrEmpty(input3))
            {
                PropertyType result = (PropertyType)Enum.Parse(typeof(PropertyType), input3);
                results = results.Where(property => property.Type.Equals(result)).ToList();
            }

            if (!string.IsNullOrEmpty(input4))
            {
                int guests = Convert.ToInt32(input4);
                results = results.Where(property => property.MaxGuests >= guests).ToList();
            }

            if (!string.IsNullOrEmpty(input5))
            {
                int days = Convert.ToInt32(input5);
                results = results.Where(property => property.MinReservationDays <= days).ToList();
            }

            return results;
        }

        private void MakeReservation_Click(object sender, RoutedEventArgs e)
        { 
            PropertyBooking propertybooking = new PropertyBooking(SelectedProperty, LoggedInUser);
            propertybooking.Show();
            Close();
        }
    }

}
