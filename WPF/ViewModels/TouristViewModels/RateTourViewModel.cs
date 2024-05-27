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
using BookingApp.WPF.Views.TouristView;

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
        public TouristExperienceViewModel TouristExperienceViewModel { get; set; }

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
            TouristExperienceViewModel = new TouristExperienceViewModel();
            ImageService = new ImageService();
            touristExperienceService = new TouristExperienceService(Injector.CreateInstance<ITouristExperienceRepository>());
            SelectedTour = selectedTour;
            LoggedInTourist = loggedInTourist;
            TouristExperienceViewModel.TouristId = LoggedInTourist.Id;
            TouristExperienceViewModel.TourId = SelectedTour.Id;

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
            Style style = Application.Current.FindResource("MessageStyle") as Style;
            if (!TouristExperienceViewModel.IsValid)
            {
                MessageBoxResult errorResult = Xceed.Wpf.Toolkit.MessageBox.Show("All fields must be filled correctly!", "Error", MessageBoxButton.OK, MessageBoxImage.Error, style);
                return;
            }
            touristExperienceService.Save(TouristExperienceViewModel.ToTouristExperience());
            MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("You have successfully rated the tour!", "Rate", MessageBoxButton.OK, MessageBoxImage.Information, style);
            Messenger.Default.Send(new NotificationMessage("CloseRateTourWindowMessage"));
        }

        private void Help()
        {
            new HelpRateTourWindow().Show();
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

                TouristExperienceViewModel.ImagesPaths.AddRange(ImageService.SaveImages(selectedImages, relativPath));

                ShowingImage = TouristExperienceViewModel.ImagesPaths.Last();
                ImageIndex = TouristExperienceViewModel.ImagesPaths.Count - 1;
            }

        }

        public void RemoveImage()
        {
            if (TouristExperienceViewModel.ImagesPaths.Count > 0)
            {
                TouristExperienceViewModel.ImagesPaths.RemoveAt(ImageIndex);
                ImageIndex = TouristExperienceViewModel.ImagesPaths.Count - 1;
                ShowingImage = TouristExperienceViewModel.ImagesPaths.LastOrDefault();
            }
        }

        public void GetNextImage()
        {
            if (ImageIndex < TouristExperienceViewModel.ImagesPaths.Count - 1)
            {
                ShowingImage = TouristExperienceViewModel.ImagesPaths[++ImageIndex];
            }
        }

        public void GetPreviousImage()
        {
            if (ImageIndex > 0)
            {
                ShowingImage = TouristExperienceViewModel.ImagesPaths[--ImageIndex];
            }
        }

        public void ShowImage()
        {
            if (TouristExperienceViewModel.ImagesPaths.Count > 0)
            {
                new FullScreenImageWindow(TouristExperienceViewModel.ImagesPaths, ImageIndex).ShowDialog();
            }
        }
    }
}
