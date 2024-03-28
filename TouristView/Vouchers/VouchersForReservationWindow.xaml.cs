using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BookingApp.TouristView.Vouchers
{
    /// <summary>
    /// Interaction logic for VouchersForReservationWindow.xaml
    /// </summary>
    public partial class VouchersForReservationWindow : Window
    {
        public static ObservableCollection<Tuple<Voucher,string>> Vouchers { get; set; }
        public User LoggedInUser { get; set; }

        private readonly VoucherRepository repository;

        public bool WindowReturnValue;
        public Tuple<Voucher,string> SelectedVoucher { get; set; }

        public VouchersForReservationWindow(User loggedInUser)
        {
            InitializeComponent();
            DataContext = this;

            repository = new VoucherRepository();
            Vouchers = new ObservableCollection<Tuple<Voucher, string>>();

            LoggedInUser = loggedInUser;
            WindowReturnValue = false;
            GetMyVouchers();
        }

        public void GetMyVouchers()
        {
            Vouchers.Clear();
            int number = 0;
            foreach (var voucher in repository.GetByToueristId(LoggedInUser.Id))
            {
                var voucherName = "Voucher " + (++number).ToString();
                Vouchers.Add(new Tuple<Voucher, string>(voucher, voucherName));
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            VoucherRepository voucherRepository = new VoucherRepository();

            if (!voucherRepository.UseVoucher(SelectedVoucher.Item1.Id, LoggedInUser.Id))
            {
                MessageBox.Show("Unable to use voucher","Something went wrong");
                Close();
                return;
            }

            WindowReturnValue = true;
            Close();
        }

        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
