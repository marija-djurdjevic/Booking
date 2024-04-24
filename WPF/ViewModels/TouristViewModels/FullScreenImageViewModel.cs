using BookingApp.Command;
using BookingApp.Domain.Models;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace BookingApp.WPF.ViewModels.TouristViewModels
{
    /// <summary>
    /// Interaction logic for FullScreenImageWindow.xaml
    /// </summary>
    public partial class FullScreenImageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private string showingImage { get; set; }
        public int ImageIndex { get; set; }

        public List<string> ImagesPaths { get; set; }

        public string ShowingImage
        {
            get { return showingImage; }
            set
            {
                showingImage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowingImage)));
            }
        }

        public RelayCommand NextImageCommand { get; set; }
        public RelayCommand PreviousImageCommand { get; set; }
        public RelayCommand CloseWindowCommand { get; set; }

        public FullScreenImageViewModel(List<string> imagePaths, int showingIndex)
        {
            ShowingImage = imagePaths[showingIndex];
            ImagesPaths = imagePaths;
            ImageIndex = showingIndex;
            NextImageCommand = new RelayCommand(GetNextImage);
            PreviousImageCommand = new RelayCommand(GetPreviousImage);
            CloseWindowCommand = new RelayCommand(CloseWindow);
        }

        private void CloseWindow()
        {
            // Slanje poruke za zatvaranje prozora koristeći MVVM Light Messaging

            Messenger.Default.Send(new NotificationMessage("CloseFullScreenImageWindowMessage"));
        }

        public void GetNextImage()
        {
            if (ImageIndex < ImagesPaths.Count - 1)
            {
                ShowingImage = ImagesPaths[++ImageIndex];
            }
        }

        public void GetPreviousImage()
        {
            if (ImageIndex > 0)
            {
                ShowingImage = ImagesPaths[--ImageIndex];
            }
        }
    }
}
