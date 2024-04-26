using BookingApp.Domain.Models;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels.TouristViewModels;
using GalaSoft.MvvmLight.Messaging;
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

namespace BookingApp.View.TouristView
{
    /// <summary>
    /// Interaction logic for VouchersForReservationWindow.xaml
    /// </summary>
    public partial class VouchersForReservationWindow : Window
    {
        public VouchersForReservationWindow(User loggedInUser)
        {
            InitializeComponent();
            DataContext = new VouchersForReservationViewModel(loggedInUser);
            Messenger.Default.Register<NotificationMessage>(this, CloseWindow);
        }

        private void CloseWindow(NotificationMessage message)
        {
            if (message.Notification == "CloseVouchersForReservationWindowMessage")
            {
                this.Close();
            }
        }
    }
}
