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

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for SideMenuPage.xaml
    /// </summary>
    public partial class SideMenuPage : Page
    {
        public SideMenuPage()
        {
            InitializeComponent();
        }

        private void NavigateBack(object sender, MouseButtonEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void NavigateToStranica1(object sender, RoutedEventArgs e)
        {

            
        }

        private void NavigateToMainPage(object sender, MouseButtonEventArgs e)
        {
            GuideMainPage guideMainPage = new GuideMainPage();
            this.NavigationService.Navigate(guideMainPage);


        }
    }
}
