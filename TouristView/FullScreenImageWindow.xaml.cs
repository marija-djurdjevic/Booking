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

namespace BookingApp.TouristView
{
    /// <summary>
    /// Interaction logic for FullScreenImageWindow.xaml
    /// </summary>
    public partial class FullScreenImageWindow : Window,INotifyPropertyChanged
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
        public FullScreenImageWindow(List<string> imagePaths, int showingIndex)
        {
            InitializeComponent();
            DataContext= this;
            ShowingImage = imagePaths[showingIndex];
            ImagesPaths = imagePaths;
            ImageIndex = showingIndex;
            WindowState = WindowState.Maximized; // Postavljanje prozora na maksimalnu veličinu
            //Topmost = true;
            Slika.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight-250;
        }


        public void GetNextImage()
        {
            if (ImageIndex < ImagesPaths.Count - 1)
            {
                string imagePath = ImagesPaths[++ImageIndex];
                ShowingImage = imagePath;
            }
        }

        public void GetPreviousImage()
        {
            if (ImageIndex > 0)
            {
                string imagePath = ImagesPaths[--ImageIndex];
                ShowingImage = imagePath;

            }
        }
        private void NextImageButtonClick(object sender, RoutedEventArgs e)
        {
            GetNextImage();
        }

        private void PreviousImageButtonClick(object sender, RoutedEventArgs e)
        {
            GetPreviousImage();
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
