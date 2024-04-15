using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Windows.Controls;
using BookingApp.Domain.Models;
using BookingApp.Aplication.UseCases;
using BookingApp.Aplication.Dto;

namespace BookingApp.WPF.ViewModel.GuestViewModel
{
    public class OwnerRatingViewModel : INotifyPropertyChanged
    {
        public PropertyReservation SelectedReservation { get; set; }
        public Property SelectedProperty { get; set; }
        public Guest LoggedInGuest { get; set; }
        public OwnerReviewDto OwnerReview { get; set; }
        public ImageService ImageService { get; set; }
        public OwnerReviewService ownerReviewService;
        public BitmapImage SelectedImage { get; set; }

        private ObservableCollection<BitmapImage> addedImages;
        public ObservableCollection<BitmapImage> AddedImages
        {
            get { return addedImages; }
            set
            {
                addedImages = value;
                OnPropertyChanged(nameof(AddedImages));
            }
        }
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

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public OwnerRatingViewModel(PropertyReservation selectedReservation, Property selectedProperty, Guest guest)
        {
            OwnerReview = new OwnerReviewDto();
            OwnerReview.OwnerId = selectedProperty.OwnerId;
            SelectedReservation = selectedReservation;
            ImageService = new ImageService();
            OwnerReview.ReservationId = SelectedReservation.Id;
            SelectedProperty = selectedProperty;
            ImagesPaths = new List<string>();
            AddedImages = new ObservableCollection<BitmapImage>();
            SelectedImage = new BitmapImage();
            ownerReviewService = new OwnerReviewService();
            LoggedInGuest = guest;
        }

        public void AddPhotos()
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
                AddedImages.Add(bitmap);
            }
        }

        public void SendReview()
        {
            OwnerReview.ImagesPaths = ImagesPaths;
            ownerReviewService.SaveReview(OwnerReview);
        }

        public void Cleanliness(object sender)
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

        public void Correctness(object sender)
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

        public void RemovePhotos()
        {
            if (SelectedImage != null)
            {
                int index = AddedImages.IndexOf(SelectedImage);
                if (index != -1)
                {
                    AddedImages.RemoveAt(index);
                    if (index < ImagesPaths.Count)
                    {
                        ImagesPaths.RemoveAt(index);
                    }
                }
            }
        }


    }


}
