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
using BookingApp.Domain.Models;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for PropertyPage.xaml
    /// </summary>
    public partial class PropertyPage : Page
    {
        public User LoggedInUser { get; set; }
        public PropertyPage(User LoggedInUser)
        {
            InitializeComponent();
            this.LoggedInUser = LoggedInUser;
        }
        private void AddProperty_Click(object sender, RoutedEventArgs e)
        {
            /*AddProperty addProperty = new AddProperty(LoggedInUser);
            
            this.NavigationService.Navigate(new Uri("View/AddProperty.xaml", UriKind.RelativeOrAbsolute));*/
            AddProperty addProperty = new AddProperty(LoggedInUser);
            this.NavigationService.Navigate(addProperty);


        }
    }
}
