using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels.TouristViewModels;
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

namespace BookingApp.View.TouristView
{
    /// <summary>
    /// Interaction logic for FullScreenImageWindow.xaml
    /// </summary>
    public partial class FullScreenImageWindow : Window
    {
        private FullScreenImageViewModel viewModel;
        public FullScreenImageWindow(List<string> imagePaths, int showingIndex)
        {
            InitializeComponent();
            viewModel = new FullScreenImageViewModel(imagePaths, showingIndex);
            DataContext= viewModel;

            Slika.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight-100;
        }

        private void NextImageButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.GetNextImage();
        }

        private void PreviousImageButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.GetPreviousImage();
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
