using BookingApp.Dto;
using BookingApp.DTO;
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

namespace BookingApp.Guest
{
    /// <summary>
    /// Interaction logic for PropertyBooking.xaml
    /// </summary>
    public partial class PropertyBooking : Window
    {
        public static PropertyRepository PropertyRepository = new PropertyRepository();
        public PropertyDto SelectedProperty { get; set; }
        public User LoggedInUser { get; set; }
        public PropertyBooking(PropertyDto selectedProperty, User loggedInUser)
        {
            InitializeComponent();
            DataContext = this;
            SelectedProperty = selectedProperty;
            LoggedInUser = loggedInUser;
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DatePicker datePicker)
            {
            DateTime selectedDate = datePicker.SelectedDate ?? DateTime.Now;
            //Student.DatumRodjenja = selectedDate;
            DateOnly dateOnly = DateOnly.FromDateTime(selectedDate);
            //Student.DatumRodjenja = dateOnly;

            }
        }
    }

    
}
