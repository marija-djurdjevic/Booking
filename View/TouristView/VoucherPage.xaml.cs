using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.View.TouristView;
using BookingApp.ViewModel.TouristViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace BookingApp.View.TouristView
{
    /// <summary>
    /// Interaction logic for VoucherPage.xaml
    /// </summary>
    public partial class VoucherPage : Page
    {
        private VoucherViewModel voucherViewModel;
        public VoucherPage(User loggedInUser)
        {
            InitializeComponent();
            voucherViewModel = new VoucherViewModel(loggedInUser);
            DataContext = voucherViewModel;
        }
        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void InboxButtonClick(object sender, RoutedEventArgs e)
        {
            voucherViewModel.OpenInbox();
        }
    }
}
