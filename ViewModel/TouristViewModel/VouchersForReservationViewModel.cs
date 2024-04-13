using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.TouristViewModel
{
    public class VouchersForReservationViewModel:INotifyPropertyChanged
    {
        public static ObservableCollection<Tuple<Voucher, string>> Vouchers { get; set; }
        public User LoggedInUser { get; set; }

        private readonly VoucherRepository repository;

        public bool WindowReturnValue;
        public Tuple<Voucher, string> SelectedVoucher { get; set; }

        public VouchersForReservationViewModel(User loggedInUser)
        {
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
            List<Voucher> sortedVouchers = repository.GetByToueristId(LoggedInUser.Id).OrderBy(x => x.ExpirationDate).ToList();
            foreach (var voucher in sortedVouchers)
            {
                var voucherName = "Voucher " + (++number).ToString();
                Vouchers.Add(new Tuple<Voucher, string>(voucher, voucherName));
            }
        }

        public bool Confirm()
        {
            VoucherRepository voucherRepository = new VoucherRepository();

            if (SelectedVoucher == null)
            {
                MessageBox.Show("Please choose the voucher you want to use.", "Something went wrong");
                return false;
            }

            if (!voucherRepository.UseVoucher(SelectedVoucher.Item1.Id, LoggedInUser.Id))
            {
                MessageBox.Show("Unable to use voucher", "Something went wrong");
                return true;
            }

            WindowReturnValue = true;
            return true;
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
