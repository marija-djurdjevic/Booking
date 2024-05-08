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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using BookingApp.Domain.Models;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Collections;
using BookingApp.WPF.ViewModels.TouristViewModels;
using BookingApp.Aplication.Dto;
using GalaSoft.MvvmLight.Messaging;

namespace BookingApp.View.TouristView
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public SearchWindow(ObservableCollection<TourDto> tours)
        {
            InitializeComponent();
            DataContext = new SearchViewModel(tours);
            Messenger.Default.Register<NotificationMessage>(this, CloseWindow);
        }
        private void CloseWindow(NotificationMessage message)
        {
            if (message.Notification == "CloseSearchWindowMessage")
            {
                this.Close();
            }
        }
    }
}
