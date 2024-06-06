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
using BookingApp.Aplication.Dto;
using BookingApp.Repositories;


namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for GuestReviewForm.xaml
    /// </summary>
    public partial class GuestReviewForm : Page
    {
        private PropertyReservationDto _propertyReservationDto;
        private ReviewDto _reviewDto;
        private int _reservationId;
        private int _guestId;
        
        ReviewRepository reviewRepository;
        public GuestReviewForm()
        {
            InitializeComponent();
            _reviewDto = new ReviewDto();
            DataContext = _reviewDto;
            reviewRepository = new ReviewRepository();
        }
        public GuestReviewForm(PropertyReservationDto propertyReservationDto, int reservationId, int guestId)
        {
            InitializeComponent();
            _reservationId = reservationId;
            _guestId = guestId;
            _reviewDto = new ReviewDto();
            DataContext = this;
            reviewRepository = new ReviewRepository();
            _propertyReservationDto = propertyReservationDto;

            //FirstNameTextBox.Text = _propertyReservationDto.GuestFirstName;
            //LastNameTextBox.Text = _propertyReservationDto.GuestLastName;

        }
        public string GuestFullName => $"{_propertyReservationDto.GuestFirstName} {_propertyReservationDto.GuestLastName}";
        public string PropertyName => _propertyReservationDto.PropertyName;
        public DateTime StartDate => _propertyReservationDto.StartDate;
        public DateTime EndDate => _propertyReservationDto.EndDate;
        public void SaveReview_Click(Object sender, EventArgs e)
        {
            ReviewDto newReviewDto = new ReviewDto(_reservationId, _guestId,_reviewDto.Cleanliness, _reviewDto.Rules, _reviewDto.Comment);
            reviewRepository.AddReview(newReviewDto.ToReview());
            MessageBox.Show("Review created successfully!");
            //this.Close();
        }
    }
}
