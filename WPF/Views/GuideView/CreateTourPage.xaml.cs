using System.IO;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using BookingApp.View.GuideView;
using BookingApp.Aplication.UseCases;
using BookingApp.Aplication.Dto;

namespace BookingApp.View
{
    public partial class CreateTourPage : Page
    {
        private TourDto tourDto;
        public String startDateTimeInput;
        private TourService tourService;
        private KeyPointService keyPointService;
        private int currentImageIndex = 0;
        private readonly TourRepository tourRepository;
        KeyPointRepository keyPointRepository;
        private ObservableCollection<DateTime> tourDates = new ObservableCollection<DateTime>();
        public ObservableCollection<string> keyPointNames { get; set; } = new ObservableCollection<string>();
        public CreateTourPage()
        {
            InitializeComponent();
            tourService = new TourService();
            keyPointService = new KeyPointService();
            LoadCitiesCountriesFromCSV();
            LoadLanguagesFromCSV();
            tourDto = new TourDto();
            DataContext = tourDto;
            tourRepository = new TourRepository();
            keyPointRepository = new KeyPointRepository();
            tourDates = new ObservableCollection<DateTime>();
            keyPointNames = new ObservableCollection<string>();
        }
        private void LoadLanguagesFromCSV() {
            string[] lines = File.ReadAllLines("../../../Resources/Data/globalLanguages.csv");
            List<string> languages = lines.Take(50).ToList();
            ComboBoxLanguage.ItemsSource = languages;
        }
        private void LoadCitiesCountriesFromCSV() {
            string filePath = "../../../Resources/Data/globalLocations.csv";
            int maxLines = 50;
            List<string> locations = tourService.GetCitiesCountriesFromCSV(filePath, maxLines);
            ComboBoxLocation.ItemsSource = locations;
        }
        private void UploadButtonClick(object sender, RoutedEventArgs e)
    {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg; *.jpeg; *.png";
            if (openFileDialog.ShowDialog() == true) {
                string selectedImagePath = openFileDialog.FileName;
                string[] images = { selectedImagePath };
                string relativePath = "Resources\\Images\\TourImages\\";
                ImageService imageService = new ImageService(); 
                List<string> relativeImagePaths = imageService.SaveImages(images, relativePath);
                foreach (var relativeImagePath in relativeImagePaths) {
                    tourDto.ImagesPaths.Add(relativeImagePath);
                }
                currentImageIndex = tourDto.ImagesPaths.Count - 1;
                UpdateImage();
            }
        }
        private void RemoveKeyPointButtonClick(object sender, RoutedEventArgs e) {
            Button button = sender as Button;
            if (button != null) {
                string keyPointName = button.DataContext as string;
                if (!string.IsNullOrEmpty(keyPointName)) {
                    keyPointNames.Remove(keyPointName);
                }
            }
        }
        private void AddKeyPointButtonClick(object sender, RoutedEventArgs e) {
            string keyPointName = KeyPointsTextBox.Text;
            keyPointNames.Add(keyPointName);
            KeyPointsTextBox.Text = string.Empty;
            RefreshKeyPointsList();
        }
        private void RefreshKeyPointsList() {
            ListBoxKeyPoints.ItemsSource = null; 
            ListBoxKeyPoints.ItemsSource = keyPointNames;
        }
        private bool SetKeyPoints(int tourId) {
            string[] keyPointsArray = keyPointNames.ToArray();
            if (keyPointService.SetKeyPoints(tourId, keyPointNames))
            { return true; }
            return false;
        }
        private void CreateTourButtonClick(object sender, RoutedEventArgs e) {
            foreach (var startDate in tourDates){
                LocationDto locationDto = GetLocationDto();
                string selectedLanguage = ComboBoxLanguage.SelectedItem as string;
                TourDto newTourDto = CreateNewTourDto(locationDto, startDate, selectedLanguage);
                CreateTourService createTourService = new CreateTourService();
                bool success = createTourService.CreateTour(newTourDto, keyPointNames, startDate);
                if (!success)   { return; }
            }
        }
        private LocationDto GetLocationDto() {
            string selectedLocation = ComboBoxLocation.SelectedItem as string;
            string selectedLanguage = ComboBoxLanguage.SelectedItem as string;
            string[] locationParts = selectedLocation.Split(',');
            string city = locationParts[0].Trim();
            string country = locationParts[1].Trim();
            return new LocationDto { City = city, Country = country };
        }
        private TourDto CreateNewTourDto(LocationDto locationDto, DateTime startDate, string selectedLanguage){
            return new TourDto(tourDto.Name, tourDto.Description, selectedLanguage, tourDto.MaxTouristNumber, startDate, tourDto.Duration, locationDto, tourDto.ImagesPaths);
        }
        private bool SaveNewTour(TourDto tourDto) {
            tourRepository.Save(tourDto.ToTour());
            return true;
        }
        private void NavigateToMainPage(object sender, MouseButtonEventArgs e){
            GuideMainPage1 guideMainPage = new GuideMainPage1();
            this.NavigationService.Navigate(guideMainPage);
        }
        private void NavigateToSideMenuPage(object sender, MouseButtonEventArgs e){         }
        private void NextImageButtonClick(object sender, RoutedEventArgs e) {
            if (currentImageIndex < tourDto.ImagesPaths.Count - 1) {
                currentImageIndex++;
                UpdateImage();
            }
        }
        private void PreviousImageButtonClick(object sender, RoutedEventArgs e) {
            if (currentImageIndex > 0) {
                currentImageIndex--;
                UpdateImage();
            }
        }
        private void DeleteImageButtonClick(object sender, RoutedEventArgs e)  {
            if (currentImageIndex >= 0 && currentImageIndex < tourDto.ImagesPaths.Count)  {
                tourDto.ImagesPaths.RemoveAt(currentImageIndex);
                if (currentImageIndex >= tourDto.ImagesPaths.Count){
                    currentImageIndex--;
                }
                UpdateImage();
            }
        }
        private void UpdateImage()
        {
            if (tourDto.ImagesPaths.Count > 0) {
                string selectedImagePath = ImageService.GetAbsolutePath(tourDto.ImagesPaths[currentImageIndex]);
                BitmapImage imageSource = new BitmapImage();
                imageSource.BeginInit();
                imageSource.CacheOption = BitmapCacheOption.OnLoad;
                imageSource.UriSource = new Uri(selectedImagePath);
                imageSource.EndInit();
                ImagePreview.Source = imageSource;
            }
            else { ImagePreview.Source = null;}
        }
        private void AddDateAndTimeButtonClick(object sender, RoutedEventArgs e) {
            DateTime selectedDateTime = StartDateTimePicker.Value ?? DateTime.Now;
            tourDates.Add(selectedDateTime);
            NewDateTextBox.Text = string.Empty;
            RefreshDatesList();
        }
        private void RefreshDatesList() {
            ListBoxDates.ItemsSource = null;
            ListBoxDates.ItemsSource = tourDates.Select(date => date.ToString());
        }
        private void RemoveDateButtonClick(object sender, RoutedEventArgs e) { 
            Button button = sender as Button;
            if (button != null) {
                string dateString = button.DataContext as string;
                if (!string.IsNullOrEmpty(dateString))  {
                    DateTime dateToRemove = DateTime.Parse(dateString);
                    tourDates.Remove(dateToRemove);
                    RefreshDatesList();
                }
            }
        }
    }
}
