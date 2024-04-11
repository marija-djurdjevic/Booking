using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.UseCases;
using BookingApp.View.TouristView;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows;
using System.Runtime.CompilerServices;

namespace BookingApp.ViewModel.TouristViewModel
{
    public class RateTourViewModel:INotifyPropertyChanged
    {
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

        public ImageService ImageService { get; set; }
        public int ImageIndex { get; set; }
        public TourDto SelectedTour { get; set; }
        public User LoggedInTourist { get; set; }
        public TouristExperience TouristExperience { get; set; }

        private int tourInterestingness;
        public int TourInterestingness
        {
            get { return tourInterestingness; }
            set
            {
                tourInterestingness = value;
                TouristExperience.TourInterestingesRating = value;
                OnPropertyChanged(nameof(TourInterestingness));
            }
        }
        private int guideLanguage;
        public int GuideLanguage
        {
            get { return guideLanguage; }
            set
            {
                guideLanguage = value;
                TouristExperience.GuideLanguageRating = value;
                OnPropertyChanged(nameof(GuideLanguage));
            }
        }
        private int guideKnowledge;
        public int GuideKnowledge
        {
            get { return guideKnowledge; }
            set
            {
                guideKnowledge = value;
                TouristExperience.GuideKnowledgeRating = value;
                OnPropertyChanged(nameof(GuideKnowledge));
            }
        }
        private TouristExperienceRepository touristExperienceRepository { get; set; }

        public RateTourViewModel(TourDto selectedTour, User loggedInTourist)
        {
            TouristExperience = new TouristExperience();
            ImageService = new ImageService();
            touristExperienceRepository = new TouristExperienceRepository();
            SelectedTour = selectedTour;
            LoggedInTourist = loggedInTourist;
            TouristExperience.TouristId = LoggedInTourist.Id;
            TouristExperience.TourId = SelectedTour.Id;

            ImageIndex = -1;
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Confirm()
        {
            touristExperienceRepository.Save(TouristExperience);
        }

        public void AddImage()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            openFileDialog.Title = "Select an Image File";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                string[] selectedImages = openFileDialog.FileNames;
                string relativPath = "Resources\\Images\\TouristExperienceImages";

                TouristExperience.ImagesPaths.AddRange(ImageService.SaveImages(selectedImages, relativPath));

                ShowingImage = ImageService.GetAbsolutePath(TouristExperience.ImagesPaths.Last());
                ImageIndex = TouristExperience.ImagesPaths.Count - 1;
            }

        }

        public void RemoveImage()
        {
            if (TouristExperience.ImagesPaths.Count > 0)
            {
                TouristExperience.ImagesPaths.RemoveAt(ImageIndex);
                ImageIndex = TouristExperience.ImagesPaths.Count - 1;
                ShowingImage = ImageService.GetAbsolutePath(TouristExperience.ImagesPaths.LastOrDefault());
            }
        }

        public void GetNextImage()
        {
            if (ImageIndex < TouristExperience.ImagesPaths.Count - 1)
            {
                string imagePath = ImageService.GetAbsolutePath(TouristExperience.ImagesPaths[++ImageIndex]);
                ShowingImage = imagePath;
            }
        }

        public void GetPreviousImage()
        {
            if (ImageIndex > 0)
            {
                string imagePath = ImageService.GetAbsolutePath(TouristExperience.ImagesPaths[--ImageIndex]);
                ShowingImage = imagePath;

            }
        }

        public void ShowImage()
        {
            if (TouristExperience.ImagesPaths.Count > 0)
            {
                FullScreenImageWindow fullScreenImageWindow = new FullScreenImageWindow(TouristExperience.ImagesPaths, ImageIndex);
                fullScreenImageWindow.Show();
            }
        }
    }
}
