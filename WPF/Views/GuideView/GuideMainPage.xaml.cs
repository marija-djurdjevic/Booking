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
using BookingApp.View;
using BookingApp.WPF.ViewModels;
using BookingApp.WPF.ViewModels.GuidesViewModel;

namespace BookingApp.View.GuideView
{
    /// <summary>
    /// Interaction logic for GuideMainPage1.xaml
    /// </summary>
    public partial class GuideMainPage1 : Page
    {
        public GuideMainPage1()
        {
            InitializeComponent();
            DataContext = new GuideMainPageViewModel();
        }

       
        private void NavigateToTourStatistic(object sender, MouseButtonEventArgs e)
        {
            TourStatistic ts=new TourStatistic();
            this.NavigationService.Navigate(ts);
        }

        private void LogOutClick(object sender, RoutedEventArgs e)
        {

            SignInForm signInWindow = new SignInForm();
            signInWindow.Show();
           
            Window mainWindow = Window.GetWindow(this);
            mainWindow.Close();

            
           
        }




    }
}
