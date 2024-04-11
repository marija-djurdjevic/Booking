using BookingApp.Model;
using System.Windows.Controls;

namespace BookingApp.GuestView
{
    /// <summary>
    /// Interaction logic for OwnerReview.xaml
    /// </summary>
    public partial class OwnerReview : Page
    {
        public PropertyReservation SelectedReservation { get; set; }
        public Property SelectedProperty { get; set; }
        public Guest LoggedInGuest { get; set; }
        public OwnerReview(PropertyReservation selectedReservation, Property selectedProperty, Guest guest)
        {
            InitializeComponent();
            SelectedReservation = selectedReservation;
            SelectedProperty = selectedProperty;
            LoggedInGuest = guest;
        }
    }
}
