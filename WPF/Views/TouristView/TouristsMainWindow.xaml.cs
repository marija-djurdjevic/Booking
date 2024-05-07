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
using BookingApp.Command;

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
        public RelayCommand ShowAndSearchCommand { get; private set; }
        public RelayCommand MyToursCommand { get; private set; }
        public RelayCommand TourRequestCommand { get; private set; }
        public RelayCommand ComplexCommand { get; private set; }
        public RelayCommand VouchersCommand { get; private set; }
        public RelayCommand SettingsCommand { get; private set; }
        public RelayCommand LogoutCommand { get; private set; }

        public TouristsMainWindow(User loggedInUser)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = loggedInUser;
            ActiveCard = "ShowAndSearch";

            Tourist = _touristService.GetByUserId(LoggedInUser.Id);
            Paige.Content = new ShowAndSearchToursPage(LoggedInUser);
            ((App)Application.Current).globalVariables.ShowTooltips = Tourist.ShowTooltips;

            ShowAndSearchCommand = new RelayCommand(ShowAndSearchExecuteCommand);
            MyToursCommand = new RelayCommand(MyToursExecuteCommand);
            TourRequestCommand = new RelayCommand(TourRequestsExecuteCommand);
            ComplexCommand = new RelayCommand(ComplexTourRequestsExecuteCommand);
            VouchersCommand = new RelayCommand(VouchersExecuteCommand);
            SettingsCommand = new RelayCommand(SettingsExecuteCommand);
            LogoutCommand = new RelayCommand(LogoutExecuteCommand);

            if (Tourist.ShowWizard)
            {
                new TouristWizardMainWindow().ShowDialog();
                Tourist.ShowWizard = false;
                _touristService.Update(Tourist);
            }
        }

        private void ShowAndSearchExecuteCommand()
        {
            Paige.Content = new ShowAndSearchToursPage(LoggedInUser);
            ActiveCard = "ShowAndSearch";
        }

        private void MyToursExecuteCommand()
        {
            Paige.Content = new MyToursPage(LoggedInUser);
            ActiveCard = "MyTours";
        }

        private void TourRequestsExecuteCommand()
        {
            Paige.Content = new TourRequestsPage(LoggedInUser);
            ActiveCard = "TourRequests";
        }
        private void ComplexTourRequestsExecuteCommand()
        {
            Paige.Content = new ComplexTourRequestsPage(LoggedInUser);
            ActiveCard = "Complex";
        }

        private void VouchersExecuteCommand()
        {
            Paige.Content = new VoucherPage(LoggedInUser);
            ActiveCard = "Vouchers";
        }

        private void LogoutExecuteCommand()
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();
        }

        private void SettingsExecuteCommand()
        {
            SettingsWindow settingsWindow = new SettingsWindow(LoggedInUser);
            settingsWindow.ShowDialog();
        }
    }
}
