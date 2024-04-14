using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using BookingApp.Command;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;

namespace BookingApp.ViewModel
{
    public class GuestReviewsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<KeyValuePair<OwnerReview, PropertyReservation>> OwnerReviews { get; set; }
        public ICommand ShowPreviousImageCommand { get; }
        public ICommand ShowNextImageCommand { get; }
        public int CurrentImageIndex { get; set; }
        public BitmapImage CurrentImage { get; set; }
        public int TotalImagesCount { get; set; }

        
        private OwnerReviewService _reviewService;

        public event PropertyChangedEventHandler PropertyChanged;

        public GuestReviewsViewModel()
        {
            _reviewService = new OwnerReviewService();
            ShowPreviousImageCommand = new RelayCommand(ShowPreviousImage);
            ShowNextImageCommand = new RelayCommand(ShowNextImage);

            LoadOwnerReviewsFromRepository();
        }

        private void LoadOwnerReviewsFromRepository()
        {
            OwnerReviews = _reviewService.LoadOwnerReviewsWithReservations();

            if (OwnerReviews.Any())
            {
                LoadImagesForReview(0);
            }
        }

        public void ShowNextImage()
        {
            if (TotalImagesCount > 0 && CurrentImageIndex < TotalImagesCount - 1)
            {
                CurrentImageIndex++;
                LoadImagesForReview(CurrentImageIndex);
            }
        }

        public void ShowPreviousImage()
        {
            if (TotalImagesCount > 0 && CurrentImageIndex > 0)
            {
                CurrentImageIndex--;
                LoadImagesForReview(CurrentImageIndex);
            }
        }

        public void LoadImagesForReview(int reviewIndex)
        {
            if (reviewIndex >= 0 && reviewIndex < OwnerReviews.Count)
            {
                var ownerReview = OwnerReviews[reviewIndex].Key;
                var imagesPaths = ownerReview.ImagesPaths;
                TotalImagesCount = imagesPaths.Count;

                if (TotalImagesCount > 0)
                {
                    CurrentImageIndex = 0;
                    var imagePath = Path.Combine("Resources/Images/GuestExperienceImages", imagesPaths[0]);
                    UpdateCurrentImage(imagePath);
                }
                else
                {
                    CurrentImageIndex = -1;
                    CurrentImage = null;
                }
            }
        }

        private void UpdateCurrentImage(string imagePath)
        {
            var bitmapImage = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            CurrentImage = bitmapImage;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
