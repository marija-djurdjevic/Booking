using BookingApp.Domain.Models;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels.TouristViewModels;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow(User loggedInUser)
        {
            InitializeComponent();
            DataContext = new SettingsViewModel(loggedInUser);
            Messenger.Default.Register<NotificationMessage>(this, CloseWindow);
        }

        private void CloseWindow(NotificationMessage message)
        {
            if (message.Notification == "CloseSettingsWindowMessage")
            {
                this.Close();
            }
        }
    }
}
