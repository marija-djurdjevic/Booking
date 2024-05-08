using BookingApp.Domain.Models;
using BookingApp.Repositories;
using BookingApp.View;
using BookingApp.WPF.ViewModels;
using BookingApp.WPF.ViewModels.GuidesViewModel;
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

namespace BookingApp.View.GuideView
{
    /// <summary>
    /// Interaction logic for TourReviewPage1.xaml
    /// </summary>
    public partial class TourReview : Page
    {
        
        public TourReview(int tourId)
        {
            InitializeComponent();
            DataContext = new TourReviewViewModel(tourId);
        }


        private void NavigateToSideMenuPage(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
