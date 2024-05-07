using BookingApp.Domain.Models;
using BookingApp.Repositories;
using BookingApp.View.TouristView;
using BookingApp.WPF.ViewModels.TouristViewModels;
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
        public VoucherPage(User loggedInUser)
        {
            InitializeComponent();
            DataContext = new VoucherViewModel(loggedInUser);
            Items.Focus();
            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                switch (message.Notification)
                {
                    case "ScrollVoucherToTop":
                        Skrol.ScrollToTop();
                        break;
                    case "ScrollVoucherToBottom":
                        Skrol.ScrollToBottom();
                        break;
                    case "ScrollVoucherDown":
                        double newOffset = Skrol.VerticalOffset + 40; // Adjust the amount to scroll as needed
                        Skrol.ScrollToVerticalOffset(newOffset);
                        break;
                    case "ScrollVoucherUp":
                        double newOffsetUp = Skrol.VerticalOffset - 40; // Adjust the amount to scroll as needed
                        Skrol.ScrollToVerticalOffset(newOffsetUp);
                        break;
                }
            });
        }
    }
}
