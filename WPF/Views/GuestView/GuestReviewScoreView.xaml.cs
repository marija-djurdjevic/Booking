using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels.GuestViewModels;
using System.Windows.Controls;

namespace BookingApp.WPF.Views.GuestView
{
    /// <summary>
    /// Interaction logic for GuestReviewScoreView.xaml
    /// </summary>
    public partial class GuestReviewScoreView : Page
    {
        private GuestReviewScoreViewModel viewModel;

        public GuestReviewScoreView(Guest guest)
        {
            InitializeComponent();
            viewModel = new GuestReviewScoreViewModel(guest);
            DataContext = viewModel;

        }
    }
}
