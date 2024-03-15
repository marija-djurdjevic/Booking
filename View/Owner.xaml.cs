using BookingApp.Dto;
using BookingApp.Model;
using BookingApp.Repository;
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
    }
}
