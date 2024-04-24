using BookingApp.Aplication;
using BookingApp.Aplication.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using GalaSoft.MvvmLight.Messaging;
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

        public Tuple<Voucher, string> SelectedVoucher { get; set; }
        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand HelpCommand { get; set; }

        public VouchersForReservationViewModel(User loggedInUser)
        {
            voucherService = new VoucherService(Injector.CreateInstance<IVoucherRepository>());
            Vouchers = new ObservableCollection<Tuple<Voucher, string>>();

            LoggedInUser = loggedInUser;
            GetMyVouchers();
            ConfirmCommand = new RelayCommand(Confirm);
            CancelCommand = new RelayCommand(CloseWindow);
            HelpCommand = new RelayCommand(Help);
        }
        private void Help()
        {

        }

        private void CloseWindow()
        {
            // Slanje poruke za zatvaranje prozora koristeći MVVM Light Messaging
            Style style = Application.Current.FindResource("MessageStyle") as Style;
            MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("Are you sure you want to close window?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning, style);
            if (result == MessageBoxResult.Yes)
                Messenger.Default.Send(new NotificationMessage("CloseVouchersForReservationWindowMessage"));
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

        public void Confirm()
        {
            if (SelectedVoucher == null)
            {
                Style style = Application.Current.FindResource("MessageStyle") as Style;
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("Please choose the voucher you want to use!", "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Information, style);
                return;
            }

            if (!voucherService.UseVoucher(SelectedVoucher.Item1.Id, LoggedInUser.Id))
            {
                Style style = Application.Current.FindResource("MessageStyle") as Style;
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("Unable to use voucher!", "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Information, style);
                Messenger.Default.Send(new NotificationMessage("CloseVouchersForReservationWindowMessage"));
                return;
            }

            Messenger.Default.Send(new NotificationMessage("CloseVouchersForReservationWindowMessage"));
            Messenger.Default.Send(new NotificationMessage("SaveReservations"));
        }
    }
}
