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
using System.Windows.Shapes;

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
        public PropertyReservationRepository PropertyReservationRepository { get; set; }
        public Owner()
        {
            InitializeComponent();
            DataContext = this;
            SelectedReservation = new PropertyReservation();
            PropertyReservationRepository = new PropertyReservationRepository();
            ReservationDataGrid.ItemsSource = PropertyReservationRepository.GetAllPropertyReservation();
            PropertyReservations = new List<PropertyReservationDto>();
        }

        private void AddProperty_Click(object sender, RoutedEventArgs e)
        {
            AddProperty addProperty = new AddProperty();
            //MainFrame.Navigate(addProperty);
            addProperty.Show();
        }
        private void RateGuestButton_Click(object sender, RoutedEventArgs e)
        {
            PropertyReservation selectedReservation = ReservationDataGrid.SelectedItem as PropertyReservation;
            if (selectedReservation != null)
            {
                if (ValidateGuestReviewFormAvailability(selectedReservation))
                {
                    PropertyReservationDto propertyReservationDto = new PropertyReservationDto();

                    propertyReservationDto.GuestFirstName = selectedReservation.GuestFirstName;
                    propertyReservationDto.GuestLastName = selectedReservation.GuestLastName;

                    GuestReviewForm guestReviewForm = new GuestReviewForm(propertyReservationDto);
                    guestReviewForm.Show();
                }
            }
            else
            {
                MessageBox.Show("Please select a reservation before rating the guest.");
            }
        }
        private bool ValidateGuestReviewFormAvailability(PropertyReservation reservation)
        {
           
            DateTime currentDate = DateTime.Now;
            if (currentDate < reservation.EndDate)
            {
                MessageBox.Show("Reservation has not yet ended.");
                return false;
            }

            TimeSpan difference = currentDate - reservation.EndDate;
            if (difference.TotalDays > 5)
            {
                MessageBox.Show("More than 5 days have passed since the end of the reservation.");
                return false;
            }

            return true;
        }




    }
}
            
                

               
            
            
            
           
        

    

