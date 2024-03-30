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


        private void NavigateToMainWindow(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Uri("GuideMainWindow.xaml", UriKind.Relative));
        }

        private void NavigateToStranica1(object sender, RoutedEventArgs e)
        {
            CreateTourPage createTourPage = new CreateTourPage();
            this.Content = createTourPage;
        }
    }
}
