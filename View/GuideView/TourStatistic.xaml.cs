using BookingApp.Model;
using BookingApp.View;
using BookingApp.ViewModel;
using BookingApp.ViewModel.GuidesViewModel;
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
    /// Interaction logic for TourStatisticPage1.xaml
    /// </summary>
    public partial class TourStatistic : Page
    {
        public TourStatistic()
        {
            InitializeComponent();
            DataContext = new TourStatisticViewModel();
        }

        private void NavigateToSideMenuPage(object sender, MouseButtonEventArgs e)
        {

        }

        
    }
}
