using BookingApp.Domain.Models;
using BookingApp.Aplication.Dto;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModel.GuestViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.GuestView
{
    /// <summary>
    /// Interaction logic for OwnerReview.xaml
    /// </summary>
    public partial class OwnerRating : Page
    {
        private OwnerRatingViewModel viewModel;

        public OwnerRating(PropertyReservation selectedReservation, Property selectedProperty, Guest guest)
        {
            InitializeComponent();
            viewModel = new OwnerRatingViewModel(selectedReservation, selectedProperty, guest);
            DataContext = viewModel;
        }

        private void AddPhotos_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AddPhotos();
        }

        private void Send_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            viewModel.SendReview();
            MessageBox.Show("Review sent!");
        }

        private void CleanlinessRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            viewModel.Cleanliness(sender);
        }

        private void CorrectnessRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            viewModel.Correctness(sender);
        }

        private void RemovePhotos_Click(object sender, RoutedEventArgs e)
        {
            viewModel.RemovePhotos();
        }

    }
}
