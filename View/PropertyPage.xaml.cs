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
    /// Interaction logic for PropertyPage.xaml
    /// </summary>
    public partial class PropertyPage : Page
    {
        public PropertyPage()
        {
            InitializeComponent();
        }
        private void AddProperty_Click(object sender, RoutedEventArgs e)
        {
            AddProperty addProperty = new AddProperty();
            //MainFrame.Navigate(addProperty);
            //this.NavigationService.Navigate(new Uri("View/AddProperty.xaml", UriKind.RelativeOrAbsolute));
            //addProperty.Show();
            // MainFrame.NavigationService.Navigate(new Uri("View/Owner.xaml", UriKind.Relative));
            //MainFrame.NavigationService.Navigate(new Uri("View/AddProperty.xaml", UriKind.Relative));
            //MainFrame.NavigationService.Navigate(new Uri("View/AddProperty.xaml", UriKind.Relative));
            this.NavigationService.Navigate(new Uri("View/AddProperty.xaml", UriKind.RelativeOrAbsolute));
            //AddPropertyFrame.Navigate(new Uri("View/AddProperty.xaml", UriKind.Relative));

        }
    }
}
