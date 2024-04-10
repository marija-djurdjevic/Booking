using BookingApp.DTO;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Build.Evaluation;
using BookingApp.Repository;
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
using System.Windows.Controls.Primitives;
using Microsoft.Win32;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using BookingApp.UseCases;
using BookingApp.ViewModel.TouristViewModel;

namespace BookingApp.View.TouristView
{
    /// <summary>
    /// Interaction logic for RateTourWindow.xaml
    /// </summary>
    public partial class RateTourWindow : Window
    {
        private RateTourViewModel rateTourViewModel;
        public RateTourWindow(TourDto selectedTour, User loggedInTourist)
        {
            InitializeComponent();
            rateTourViewModel = new RateTourViewModel(selectedTour, loggedInTourist);
            DataContext = rateTourViewModel;
        }
        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            rateTourViewModel.Confirm();
            Close();

        }
        private void AddImageButtonClick(object sender, RoutedEventArgs e)
        {
            rateTourViewModel.AddImage();
        }

        private void RemoveImageButtonClick(object sender, RoutedEventArgs e)
        {
            rateTourViewModel.RemoveImage();
        }

        private void NextImageButtonClick(object sender, RoutedEventArgs e)
        {
            rateTourViewModel.GetNextImage();
        }

        private void PreviousImageButtonClick(object sender, RoutedEventArgs e)
        {
            rateTourViewModel.GetPreviousImage();
        }

        private void ImageButtonClick(object sender, RoutedEventArgs e)
        {
            rateTourViewModel.ShowImage();
        }
    }
}
