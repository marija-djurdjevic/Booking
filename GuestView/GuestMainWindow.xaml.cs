using BookingApp.Dto;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
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

namespace BookingApp.Guest
{
    /// <summary>
    /// Interaction logic for Guest.xaml
    /// </summary>
    public partial class GuestMainWindow : Window
    {
        public static PropertyRepository PropertyRepository = new PropertyRepository();
        public User LoggedInUser { get; set; }
        public GuestMainWindow(User user)
        {
            InitializeComponent();
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;

            double windowWidth = screenWidth * 3 / 4;
            double windowHeight = screenHeight * 3 / 4;

            double left = (screenWidth - windowWidth) / 2;
            double top = (screenHeight - windowHeight) / 2;

            Width = windowWidth;
            Height = windowHeight;
            Left = left;
            Top = top;
            LoggedInUser = user;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            PropertyView propertyview = new PropertyView(LoggedInUser);
            propertyview.Owner = this;
            propertyview.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            propertyview.ShowDialog();
            Close();
        }
    }
}
