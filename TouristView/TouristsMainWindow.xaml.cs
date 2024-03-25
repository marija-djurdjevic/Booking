using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.TouristView.MyTours;
using BookingApp.TouristView.ShowAndSearchTours;
using BookingApp.View;
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
        public Tourist Tourist { get; set; }

        private readonly TouristRepository _touristRepository = new TouristRepository();

        public TouristsMainWindow(User loggedInUser)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = loggedInUser;
            Tourist = _touristRepository.GetByUserId(LoggedInUser.Id);
            Paige.Content = new ShowAndSearchToursPage(LoggedInUser);
        }

        private void ShowAndSearchToursButtonClick(object sender, RoutedEventArgs e)
        {
            Paige.Content = new ShowAndSearchToursPage(LoggedInUser);
        }

        private void MyToursButtonClick(object sender, RoutedEventArgs e)
        {
            Paige.Content = new MyToursPage(new Model.User());
        }

        private void VouchersButtonClick(object sender, RoutedEventArgs e)
        {
            Paige.Content = new Vouchers.VoucherPage(LoggedInUser);
        }

        private void LogoutButtonClick(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();
        }
    }
}
