using BookingApp.Aplication;
using BookingApp.Aplication.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.View.TouristView;
using GalaSoft.MvvmLight.CommandWpf;
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
    public class VoucherViewModel : BindableBase
    {
        public static ObservableCollection<Tuple<Voucher, string>> Vouchers { get; set; }
        public User LoggedInUser { get; set; }

        private int unreadNotificationCount;
        public int UnreadNotificationCount
        {
            get { return unreadNotificationCount; }
            set
            {
                unreadNotificationCount = value;
                OnPropertyChanged(nameof(UnreadNotificationCount));
            }
        }

        private readonly VoucherService voucherService;
        private readonly TouristGuideNotificationService notificationService;

        public RelayCommand HelpCommand { get; set; }
        public RelayCommand InboxCommand { get; set; }
         
        public VoucherViewModel(User loggedInUser)
        {
            voucherService = new VoucherService(Injector.CreateInstance<IVoucherRepository>());
            notificationService = new TouristGuideNotificationService(Injector.CreateInstance<ITouristGuideNotificationRepository>());
            Vouchers = new ObservableCollection<Tuple<Voucher, string>>();

            LoggedInUser = loggedInUser;
            GetMyVouchers();
            UnreadNotificationCount = notificationService.GetUnreadNotificationCount(LoggedInUser.Id);
            HelpCommand = new RelayCommand(Help);
            InboxCommand = new RelayCommand(OpenInbox);
        }

        private void Help()
        {

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

        public void OpenInbox()
        {
            NotificationsWindow notificationsWindow = new NotificationsWindow(LoggedInUser);
            notificationsWindow.ShowDialog();
            UnreadNotificationCount = notificationService.GetUnreadNotificationCount(LoggedInUser.Id);
        }
    }
}
