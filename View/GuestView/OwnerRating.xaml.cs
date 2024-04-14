using BookingApp.Dto;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.UseCases;
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
    public partial class OwnerRating : Page, INotifyPropertyChanged
    {
        public PropertyReservation SelectedReservation { get; set; }
        public Property SelectedProperty { get; set; }
        public Guest LoggedInGuest { get; set; }
        public OwnerReviewDto OwnerReview { get; set; }
        public OwnerReviewRepository OwnerReviewRepository { get; set; }
        public ImageService ImageService { get; set; }
        private ObservableCollection<BitmapImage> images {  get; set; }
        public List<string> ImagesPaths { get; set; }

        private string showingImage;
        public string ShowingImage
        {
            get { return showingImage; }
            set
            {
                showingImage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowingImage)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public OwnerRating(PropertyReservation selectedReservation, Property selectedProperty, Guest guest)
        {
            InitializeComponent();
            DataContext = this;
            OwnerReview = new OwnerReviewDto();
            OwnerReview.OwnerId = selectedProperty.OwnerId;
            SelectedReservation = selectedReservation;
            ImageService = new ImageService();
            OwnerReview.ReservationId = SelectedReservation.Id;
            SelectedProperty = selectedProperty;
            ImagesPaths = new List<string>();
            images = new ObservableCollection<BitmapImage>();
            OwnerReviewRepository = new OwnerReviewRepository();
            LoggedInGuest = guest;
            imageListBox.ItemsSource = images;
        }

        private void AddPhotos_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            openFileDialog.Title = "Select an Image File";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                string[] selectedImages = openFileDialog.FileNames;
                string relativePath = "Resources\\Images\\GuestExperienceImages";
                ImagesPaths.AddRange(ImageService.SaveImages(selectedImages, relativePath));
                ShowingImage = ImageService.GetAbsolutePath(ImagesPaths.Last());
                BitmapImage bitmap = new BitmapImage(new Uri(ShowingImage));
                images.Add(bitmap);
            }

        }

        private void Send_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OwnerReview.ImagesPaths = ImagesPaths;
            OwnerReviewRepository.AddOwnerReview(OwnerReview.ToOwnerReview());
            MessageBox.Show("Review sent!");
        }

        private void CleanlinessRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                if (int.TryParse(radioButton.Content.ToString(), out int cleanlinessValue))
                {
                    OwnerReview.Cleanliness = cleanlinessValue;
                }
                else
                {
                    OwnerReview.Cleanliness = 0;
                }
            }
        }

        private void CorrectnessRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                if (int.TryParse(radioButton.Content.ToString(), out int correctnessValue))
                {
                    OwnerReview.Correctness = correctnessValue;
                }
                else
                {
                    OwnerReview.Correctness = 0;
                }
            }
        }

        private void RemovePhotos_Click(object sender, RoutedEventArgs e)
        {
            var selectedImages = imageListBox.SelectedItems.Cast<BitmapImage>().ToList();

            foreach (var image in selectedImages)
            {
                int index = images.IndexOf(image);

                if (index != -1)
                {
                    images.RemoveAt(index);
                    if (index < ImagesPaths.Count)
                    {
                        ImagesPaths.RemoveAt(index);
                    }
                }
            }
        }


    }
}
