using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.TouristView.TourBooking;
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

namespace BookingApp.TouristView.Vouchers
{
    /// <summary>
    /// Interaction logic for VoucherPage.xaml
    /// </summary>
    public partial class VoucherPage : Page
    {
        public static ObservableCollection<Voucher> Vouchers { get; set; }
        public User LoggedInUser { get; set; }

        private readonly VoucherRepository repository;

        public VoucherPage(User loggedInUser)
        {
            InitializeComponent();
            DataContext = this;

            repository = new VoucherRepository();
            Vouchers = new ObservableCollection<Voucher>();

            LoggedInUser = loggedInUser;
            GetMyVouchers();
        }

        public void GetMyVouchers()
        {
            Vouchers.Clear();
            int number = 0;
            foreach (var voucher in repository.GetByToueristId(LoggedInUser.Id))
            {
                voucher.VoucherNumber = "Voucher "+(++number).ToString();
                Vouchers.Add(voucher);
            }
        }

        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
