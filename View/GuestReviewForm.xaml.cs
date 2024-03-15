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
using BookingApp.Dto;
using BookingApp.Repository;


namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for GuestReviewForm.xaml
    /// </summary>
    public partial class GuestReviewForm : Window
    {
        private PropertyReservationDto _propertyReservationDto;
        private ReviewDto _reviewDto;

        ReviewRepository reviewRepository;
        public GuestReviewForm()
        {
            InitializeComponent();
            _reviewDto = new ReviewDto();
            DataContext = _reviewDto;
            reviewRepository = new ReviewRepository();
        }
        public GuestReviewForm(PropertyReservationDto propertyReservationDto)
        {
            InitializeComponent();
            _reviewDto = new ReviewDto();
            DataContext = _reviewDto;
            reviewRepository = new ReviewRepository();
            _propertyReservationDto = propertyReservationDto;

            FirstNameTextBox.Text = _propertyReservationDto.GuestFirstName;
            LastNameTextBox.Text = _propertyReservationDto.GuestLastName;

        }

        public void SaveReview_Click(Object sender, EventArgs e)
        {
            ReviewDto newReviewDto = new ReviewDto(_reviewDto.ReservationId, _reviewDto.GuestId,_reviewDto.Cleanliness, _reviewDto.Rules, _reviewDto.Comment);
            reviewRepository.AddReview(newReviewDto.ToReview());
            MessageBox.Show("Review created successfully!");
            this.Close();
        }
    }
}
