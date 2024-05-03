using BookingApp.Domain.Models;
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
using BookingApp;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookingApp.WPF.Views.TouristView
{
    /// <summary>
    /// Interaction logic for TouristWizardMainWindow.xaml
    /// </summary>
    public partial class TouristWizardMainWindow : Window, INotifyPropertyChanged
    {
        private int CurrentImage;
        private string showingImage;
        public string ShowingImage
        {
            get { return showingImage; }
            set
            {
                showingImage = value;
                OnPropertyChanged(nameof(ShowingImage));
            }
        }
        public List<string> imagePaths;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TouristWizardMainWindow(User LoggedInUser)
        {
            InitializeComponent();
            DataContext = this;
            CurrentImage = -1;
            imagePaths = new List<string>();
            imagePaths.Add(@"Resources\Images\TouristWizardImages\1.1.png");
            imagePaths.Add(@"Resources\Images\TouristWizardImages\1.2.png");
        }

        private void WelcomeButtonClick(object sender, RoutedEventArgs e)
        {
            WelcomeText.Visibility = Visibility.Visible;
            ImageContent.Visibility = Visibility.Collapsed;
            CurrentImage = -1;
        }

        private void MainWindowButtonClick(object sender, RoutedEventArgs e)
        {
            WelcomeText.Visibility = Visibility.Collapsed;
            ImageContent.Visibility = Visibility.Visible;
            ShowingImage = imagePaths[0];
        }

        private void ReservateButtonClick(object sender, RoutedEventArgs e)
        {
            WelcomeText.Visibility = Visibility.Collapsed;
            ImageContent.Visibility = Visibility.Visible;
        }

        private void MyToursButtonClick(object sender, RoutedEventArgs e)
        {
            WelcomeText.Visibility = Visibility.Collapsed;
            ImageContent.Visibility = Visibility.Visible;
        }

        private void RequestsButtonClick(object sender, RoutedEventArgs e)
        {
            WelcomeText.Visibility = Visibility.Collapsed;
            ImageContent.Visibility = Visibility.Visible;
        }

        private void VouchersButtonClick(object sender, RoutedEventArgs e)
        {
            WelcomeText.Visibility = Visibility.Collapsed;
            ImageContent.Visibility = Visibility.Visible;
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            if (CurrentImage > 0)
            {
                ShowingImage = imagePaths[--CurrentImage];
            }
            else if(CurrentImage==0)
            {
                WelcomeText.Visibility = Visibility.Visible;
                ImageContent.Visibility = Visibility.Collapsed;
                CurrentImage--;
            }
        }

        private void NextButtonClick(object sender, RoutedEventArgs e)
        {
            if (CurrentImage == -1)
            {
                WelcomeText.Visibility = Visibility.Collapsed;
                ImageContent.Visibility = Visibility.Visible;
                ShowingImage = imagePaths[++CurrentImage];
            }
            else
            {
                if (CurrentImage < imagePaths.Count - 1)
                    ShowingImage = imagePaths[++CurrentImage];
            }
        }

        private void FinishButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
