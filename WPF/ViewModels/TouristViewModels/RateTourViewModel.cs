using BookingApp.Repositories;
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
using BookingApp.Domain.Models;
using BookingApp.Aplication.UseCases;
using BookingApp.Aplication.Dto;
using BookingApp.Aplication;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Command;
using GalaSoft.MvvmLight.Messaging;

namespace BookingApp.WPF.ViewModels.TouristViewModels
{
    public class RateTourViewModel : BindableBase
    {
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
        private TouristExperienceService touristExperienceService { get; set; }
        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand HelpCommand { get; set; }
        public RelayCommand AddImageCommand { get; set; }
        public RelayCommand ShowImageCommand { get; set; }
        public RelayCommand NextImageCommand { get; set; }
        public RelayCommand PreviousImageCommand { get; set; }
        public RelayCommand RemoveImageCommand { get; set; }

        public RateTourViewModel(TourDto selectedTour, User loggedInTourist)
        {
            TouristExperience = new TouristExperience();
            ImageService = new ImageService();
            touristExperienceService = new TouristExperienceService(Injector.CreateInstance<ITouristExperienceRepository>());
            SelectedTour = selectedTour;
            LoggedInTourist = loggedInTourist;
            TouristExperience.TouristId = LoggedInTourist.Id;
            TouristExperience.TourId = SelectedTour.Id;

            ImageIndex = -1;
            ConfirmCommand = new RelayCommand(Confirm);
            CancelCommand = new RelayCommand(CloseWindow);
            HelpCommand = new RelayCommand(Help);
            AddImageCommand = new RelayCommand(AddImage);
            ShowImageCommand = new RelayCommand(ShowImage);
            RemoveImageCommand = new RelayCommand(RemoveImage);
            NextImageCommand = new RelayCommand(GetNextImage);
            PreviousImageCommand = new RelayCommand(GetPreviousImage);
        }

        public void Confirm()
        {
            touristExperienceService.Save(TouristExperience);
            Style style = Application.Current.FindResource("MessageStyle") as Style;
            MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("You have successfully rated the tour!", "Rate", MessageBoxButton.OK, MessageBoxImage.Information, style);
            Messenger.Default.Send(new NotificationMessage("CloseRateTourWindowMessage"));
        }

        private void Help()
        {

        }

        private void CloseWindow()
        {
            // Slanje poruke za zatvaranje prozora koristeći MVVM Light Messaging
            Style style = Application.Current.FindResource("MessageStyle") as Style;
            MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("Are you sure you want to close window?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning, style);
            if (result == MessageBoxResult.Yes)
                Messenger.Default.Send(new NotificationMessage("CloseRateTourWindowMessage"));
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

                ShowingImage = TouristExperience.ImagesPaths.Last();
                ImageIndex = TouristExperience.ImagesPaths.Count - 1;
            }

        }

        public void RemoveImage()
        {
            if (TouristExperience.ImagesPaths.Count > 0)
            {
                TouristExperience.ImagesPaths.RemoveAt(ImageIndex);
                ImageIndex = TouristExperience.ImagesPaths.Count - 1;
                ShowingImage = TouristExperience.ImagesPaths.LastOrDefault();
            }
        }

        public void GetNextImage()
        {
            if (ImageIndex < TouristExperience.ImagesPaths.Count - 1)
            {
                ShowingImage = TouristExperience.ImagesPaths[++ImageIndex];
            }
        }

        public void GetPreviousImage()
        {
            if (ImageIndex > 0)
            {
                ShowingImage = TouristExperience.ImagesPaths[--ImageIndex];
            }
        }

        public void ShowImage()
        {
            if (TouristExperience.ImagesPaths.Count > 0)
            {
                new FullScreenImageWindow(TouristExperience.ImagesPaths, ImageIndex).ShowDialog();
            }
        }
    }
}
