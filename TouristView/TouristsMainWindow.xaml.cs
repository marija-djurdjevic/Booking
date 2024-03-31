﻿using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.TouristView.MyTours;
using BookingApp.TouristView.ShowAndSearchTours;
using BookingApp.TouristView.Vouchers;
using BookingApp.TouristView.ComplexTourRequests;
using BookingApp.View;
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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookingApp.TouristView
{
    /// <summary>
    /// Interaction logic for TouristsMainWindow.xaml
    /// </summary>
    public partial class TouristsMainWindow : Window,INotifyPropertyChanged
    {
        public User LoggedInUser { get; set; }
        public Tourist Tourist { get; set; }

        private readonly TouristRepository _touristRepository = new TouristRepository();

        private string activeCard;

        public string ActiveCard
        {
            get => activeCard;
            set
            {
                if (value != activeCard)
                {
                    activeCard = value;
                    OnPropertyChanged(nameof(ActiveCard));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TouristsMainWindow(User loggedInUser)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = loggedInUser;
            ActiveCard = "ShowAndSearch";
            Tourist = _touristRepository.GetByUserId(LoggedInUser.Id);
            Paige.Content = new ShowAndSearchToursPage(LoggedInUser);
        }

        private void ShowAndSearchToursButtonClick(object sender, RoutedEventArgs e)
        {
            Paige.Content = new ShowAndSearchToursPage(LoggedInUser);
            ActiveCard = "ShowAndSearch";
        }

        private void MyToursButtonClick(object sender, RoutedEventArgs e)
        {
            Paige.Content = new MyTours.MyToursPage(LoggedInUser);
            ActiveCard = "MyTours";
        }

        private void VouchersButtonClick(object sender, RoutedEventArgs e)
        {
            Paige.Content = new Vouchers.VoucherPage(LoggedInUser);
            ActiveCard = "Vouchers";
        }

        private void LogoutButtonClick(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();
        }

        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void SettingsButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
