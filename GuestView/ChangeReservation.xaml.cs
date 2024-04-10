using BookingApp.Model;
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
    /// Interaction logic for ChangeReservation.xaml
    /// </summary>
    public partial class ChangeReservation : Page
    {

        public PropertyReservation SelectedReservation { get; set; }
        public Property SelectedProperty { get; set; }
        public Guest LoggedInGuest { get; set; }
        public ChangeReservation(PropertyReservation selectedReservation, Property selectedProperty, Guest guest)
        {
            InitializeComponent();
            DataContext = this;
            SelectedReservation = selectedReservation;
            LoggedInGuest = guest;
            SelectedProperty = selectedProperty;

        }
    }
}
