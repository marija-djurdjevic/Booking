using BookingApp.Aplication;
using BookingApp.Aplication.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.View.TouristView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModel.TouristViewModel
{
    public class VoucherViewModel : INotifyPropertyChanged
    {
        public static ObservableCollection<Tuple<Voucher, string>> Vouchers { get; set; }
        public User LoggedInUser { get; set; }

        private bool isNoVoucherTextVisible;

        public bool IsNoVoucherTextVisible
        {
            get { return isNoVoucherTextVisible; }
            set
            {
                isNoVoucherTextVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNoVoucherTextVisible)));
            }
        }

        private readonly VoucherService voucherService;

        public VoucherViewModel(User loggedInUser)
        {
            voucherService = new VoucherService(Injector.CreateInstance<IVoucherRepository>());
            Vouchers = new ObservableCollection<Tuple<Voucher, string>>();

            LoggedInUser = loggedInUser;
            IsNoVoucherTextVisible = false;
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
            if (Vouchers.Count() < 1)
            {
                IsNoVoucherTextVisible = true;
            }
        }

        public void OpenInbox()
        {
            NotificationsWindow notificationsWindow = new NotificationsWindow(LoggedInUser);
            notificationsWindow.ShowDialog();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
