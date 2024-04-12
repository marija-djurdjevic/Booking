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
using BookingApp.ViewModel;


namespace BookingApp.View.GuideView
{
    /// <summary>
    /// Interaction logic for TouristsNumberPage1.xaml
    /// </summary>
    public partial class TouristsNumber : Page
    {
        public TouristsNumber(int tourId)
        {
            InitializeComponent();
            DataContext = new TouristsNumberPageViewModel(tourId);
        }

        private void NavigateToSideMenuPage(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
