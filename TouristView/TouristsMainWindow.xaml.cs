using BookingApp.Model;
using BookingApp.TouristView.MyTours;
using BookingApp.TouristView.ShowAndSearchTours;
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

namespace BookingApp.TouristView
{
    /// <summary>
    /// Interaction logic for TouristsMainWindow.xaml
    /// </summary>
    public partial class TouristsMainWindow : Window
    {
        public User LoggedInUser { get; set; }

        public TouristsMainWindow(User loggedInUser)
        {
            InitializeComponent();
            LoggedInUser = loggedInUser;
            Paige.Content = new ShowAndSearchToursPaige(LoggedInUser);
        }

        private void ShowAndSearchToursButtonClick(object sender, RoutedEventArgs e)
        {
            Paige.Content = new ShowAndSearchToursPaige(LoggedInUser);
        }

        private void MyToursButtonClick(object sender, RoutedEventArgs e)
        {
            Paige.Content = new MyToursPaige(new Model.User());
        }
    }
}
