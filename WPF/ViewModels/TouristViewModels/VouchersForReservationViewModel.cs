using BookingApp.Aplication;
using BookingApp.Aplication.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModels.TouristViewModels
{
    public class VouchersForReservationViewModel : BindableBase
    {
        public static ObservableCollection<Tuple<Voucher, string>> Vouchers { get; set; }
        public User LoggedInUser { get; set; }

        private readonly VoucherService voucherService;

        public bool WindowReturnValue;
        public Tuple<Voucher, string> SelectedVoucher { get; set; }

        public VouchersForReservationViewModel(User loggedInUser)
        {
            voucherService = new VoucherService(Injector.CreateInstance<IVoucherRepository>());
            Vouchers = new ObservableCollection<Tuple<Voucher, string>>();

            LoggedInUser = loggedInUser;
            WindowReturnValue = false;
            GetMyVouchers();
        }

        public void GetMyVouchers()
        {
            Vouchers.Clear();
            int number = 0;
            List<Voucher> sortedVouchers = voucherService.GetByToueristId(LoggedInUser.Id).OrderBy(x => x.ExpirationDate).ToList();
            foreach (var voucher in sortedVouchers)
            {
                var voucherName = "Voucher " + (++number).ToString();
                Vouchers.Add(new Tuple<Voucher, string>(voucher, voucherName));
            }
        }

        public bool Confirm()
        {
            if (SelectedVoucher == null)
            {
                MessageBox.Show("Please choose the voucher you want to use.", "Something went wrong");
                return false;
            }

            if (!voucherService.UseVoucher(SelectedVoucher.Item1.Id, LoggedInUser.Id))
            {
                MessageBox.Show("Unable to use voucher", "Something went wrong");
                return true;
            }

            WindowReturnValue = true;
            return true;
        }
    }
}
