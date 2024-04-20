using BookingApp.Repositories;
using BookingApp.View.TouristView;
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
using BookingApp.Domain.Models;
using BookingApp.Aplication.UseCases;
using BookingApp.Aplication;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Views.TouristView;

namespace BookingApp.View.TouristView
{
    /// <summary>
    /// Interaction logic for TouristsMainWindow.xaml
    /// </summary>
    public partial class TouristsMainWindow : Window, INotifyPropertyChanged
    {
        public User LoggedInUser { get; set; }
        public Tourist Tourist { get; set; }

        private readonly TouristService _touristService = new TouristService(Injector.CreateInstance<ITouristRepository>());

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
            Tourist = _touristService.GetByUserId(LoggedInUser.Id);
            Paige.Content = new ShowAndSearchToursPage(LoggedInUser);
        }

        private void ShowAndSearchToursButtonClick(object sender, RoutedEventArgs e)
        {
            Paige.Content = new ShowAndSearchToursPage(LoggedInUser);
            ActiveCard = "ShowAndSearch";
        }

        private void MyToursButtonClick(object sender, RoutedEventArgs e)
        {
            Paige.Content = new MyToursPage(LoggedInUser);
            ActiveCard = "MyTours";
        }

        private void TourRequestsButtonClick(object sender, RoutedEventArgs e)
        {
            Paige.Content = new TourRequestsPage(LoggedInUser);
            ActiveCard = "TourRequests";
        }

        private void VouchersButtonClick(object sender, RoutedEventArgs e)
        {
            Paige.Content = new VoucherPage(LoggedInUser);
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
            SettingsWindow settingsWindow = new SettingsWindow(LoggedInUser);
            settingsWindow.ShowDialog();
        }
    }
}
