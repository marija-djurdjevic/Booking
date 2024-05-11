using System;
using System.Collections.Generic;
using System.Windows;
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

        public TouristWizardMainWindow()
        {
            InitializeComponent();
            DataContext = this;
            imagePaths = new List<string>();
            imagePaths.Add(@"Resources\Images\TouristWizardImages\0.0.png");
            imagePaths.Add(@"Resources\Images\TouristWizardImages\1.1.png");
            imagePaths.Add(@"Resources\Images\TouristWizardImages\1.2.png");
            imagePaths.Add(@"Resources\Images\TouristWizardImages\1.3.mp4");
            imagePaths.Add(@"Resources\Images\TouristWizardImages\1.4.png");
            imagePaths.Add(@"Resources\Images\TouristWizardImages\2.1.png");
            imagePaths.Add(@"Resources\Images\TouristWizardImages\2.2.png");
            imagePaths.Add(@"Resources\Images\TouristWizardImages\3.1.png");
            imagePaths.Add(@"Resources\Images\TouristWizardImages\3.2.png");
            imagePaths.Add(@"Resources\Images\TouristWizardImages\4.1.png");
            imagePaths.Add(@"Resources\Images\TouristWizardImages\4.2.png");
            imagePaths.Add(@"Resources\Images\TouristWizardImages\5.1.png");
            CurrentImage = 0;
            ShowingImage = imagePaths[0];
        }

        private void WelcomeButtonClick(object sender, RoutedEventArgs e)
        {
            WelcomeText.Visibility = Visibility.Visible;
            ImageContent.Visibility = Visibility.Collapsed;
            VideoPleyer.Visibility = Visibility.Collapsed;
            CurrentImage = 0;
            ShowingImage = imagePaths[0];
        }

        private void MainWindowButtonClick(object sender, RoutedEventArgs e)
        {
            WelcomeText.Visibility = Visibility.Collapsed;
            ImageContent.Visibility = Visibility.Visible;
            VideoPleyer.Visibility = Visibility.Collapsed;
            ShowingImage = imagePaths[1];
            CurrentImage = 1;
            CheckType();
        }

        private void ReservateButtonClick(object sender, RoutedEventArgs e)
        {
            WelcomeText.Visibility = Visibility.Collapsed;
            ImageContent.Visibility = Visibility.Visible;
            VideoPleyer.Visibility = Visibility.Collapsed;
            ShowingImage = imagePaths[5];
            CurrentImage = 5;
            CheckType();
        }

        private void MyToursButtonClick(object sender, RoutedEventArgs e)
        {
            WelcomeText.Visibility = Visibility.Collapsed;
            ImageContent.Visibility = Visibility.Visible;
            VideoPleyer.Visibility = Visibility.Collapsed;
            ShowingImage = imagePaths[7];
            CurrentImage = 7;
            CheckType();
        }

        private void RequestsButtonClick(object sender, RoutedEventArgs e)
        {
            WelcomeText.Visibility = Visibility.Collapsed;
            ImageContent.Visibility = Visibility.Visible;
            VideoPleyer.Visibility = Visibility.Collapsed;
            ShowingImage = imagePaths[9];
            CurrentImage = 9;
            CheckType();
        }

        private void VouchersButtonClick(object sender, RoutedEventArgs e)
        {
            WelcomeText.Visibility = Visibility.Collapsed;
            ImageContent.Visibility = Visibility.Visible;
            VideoPleyer.Visibility = Visibility.Collapsed;
            ShowingImage = imagePaths[11];
            CurrentImage = 11;
            CheckType();
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            if (CurrentImage > 1)
            {
                ShowingImage = imagePaths[--CurrentImage];
                CheckType();
            }
            else if (CurrentImage == 1)
            {
                WelcomeText.Visibility = Visibility.Visible;
                ImageContent.Visibility = Visibility.Collapsed;
                VideoPleyer.Visibility = Visibility.Collapsed;
                ShowingImage = imagePaths[--CurrentImage];
            }
        }

        private void NextButtonClick(object sender, RoutedEventArgs e)
        {
            if (CurrentImage == 0)
            {
                WelcomeText.Visibility = Visibility.Collapsed;
                ImageContent.Visibility = Visibility.Visible;
                VideoPleyer.Visibility = Visibility.Collapsed;
                ShowingImage = imagePaths[++CurrentImage];
            }
            else
            {
                if (CurrentImage < imagePaths.Count - 1)
                    ShowingImage = imagePaths[++CurrentImage];
            }
            CheckType();
        }

        private void CheckType()
        {
            if (ShowingImage.Contains(".mp4"))
            {
                VideoPleyer.Visibility = Visibility.Visible;
                ImageContent.Visibility = Visibility.Collapsed;
                VideoPleyer.Play();
            }
            else
            {
                VideoPleyer.Visibility = Visibility.Collapsed;
                ImageContent.Visibility = Visibility.Visible;
            }
        }

        private void FinishButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ReplayVideo(object sender, RoutedEventArgs e)
        {
            VideoPleyer.Position = TimeSpan.Zero;
            VideoPleyer.Play();
        }
    }
}