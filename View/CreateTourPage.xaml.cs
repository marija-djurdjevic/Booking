using BookingApp.DTO;
using BookingApp.Model.Enums;
using System.IO;
using BookingApp.Model;
using BookingApp.Repository;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Collections.ObjectModel;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for CreateTourPage.xaml
    /// </summary>
    public partial class CreateTourPage : Page
    {

        private TourDto tourDto;

        public String startDateTimeInput;

        // private List<DateTime> tourDates = new List<DateTime>();

        private ObservableCollection<DateTime> tourDates = new ObservableCollection<DateTime>();


        //private List<String> keyPointNames = new List<String>();
        public ObservableCollection<string> keyPointNames { get; set; } = new ObservableCollection<string>();

        TourRepository tourRepository;
        KeyPointRepository keyPointRepository;

       
        public CreateTourPage()
        {
            InitializeComponent();
            LoadCitiesCountriesFromCSV();
            LoadLanguagesFromCSV();
            tourDto = new TourDto();
            DataContext = tourDto;
            tourRepository = new TourRepository();
            keyPointRepository = new KeyPointRepository();
            tourDates = new ObservableCollection<DateTime>();
            keyPointNames = new ObservableCollection<string>();
        }

        /*    private void AddDateAndTimeButtonClick(object sender, RoutedEventArgs e)
            {
                string newDateAndTime = Microsoft.VisualBasic.Interaction.InputBox("Enter a new date and time (format: M/d/yyyy h:mm:ss tt):", "Add Date and Time", "");

                if (!string.IsNullOrEmpty(newDateAndTime))
                {
                    if (DateTime.TryParseExact(newDateAndTime, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateAndTime))
                    {
                        tourDates.Add(dateAndTime);
                        NewDateTextBox.Text = dateAndTime.ToString();

                    }
                    else
                    {
                        MessageBox.Show("Invalid date and time format. Please enter a valid date and time (format: M/d/yyyy h:mm:ss tt).");
                    }
                }
            }
        */


        private void LoadLanguagesFromCSV()
        {
            string[] lines = File.ReadAllLines("../../../Resources/Data/globalLanguages.csv");
            List<string> languages = lines.Take(50).ToList();
            ComboBoxLanguage.ItemsSource = languages;
        }



        private void LoadCitiesCountriesFromCSV()
        {
            string[] lines = File.ReadAllLines("../../../Resources/Data/globalLocations.csv");
            List<string> locations = new List<string>();
            Random random = new Random();

            // Shuffle the lines array
            for (int i = lines.Length - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                string temp = lines[i];
                lines[i] = lines[j];
                lines[j] = temp;
            }

            // Take the first 50 lines
            for (int i = 0; i < 50 && i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(',');
                if (parts.Length == 2)
                {
                    string location = $"{parts[0]}, {parts[1]}";
                    locations.Add(location);
                }
            }

            ComboBoxLocation.ItemsSource = locations;
        }


        private void UploadButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg; *.jpeg; *.png";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedImagePath = openFileDialog.FileName;
                tourDto.ImagesPaths.Add(selectedImagePath);
                UpdateImage();
            }
        }



        private void RemoveKeyPointButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                string keyPointName = button.DataContext as string;
                if (!string.IsNullOrEmpty(keyPointName))
                {
                    keyPointNames.Remove(keyPointName);
                }
            }
        }


        private void AddKeyPointButtonClick(object sender, RoutedEventArgs e)
        {
            string keyPointName = KeyPointsTextBox.Text;
            keyPointNames.Add(keyPointName);
            KeyPointsTextBox.Text = string.Empty;
            RefreshKeyPointsList();
        }

        private void RefreshKeyPointsList()
        {
            ListBoxKeyPoints.ItemsSource = null; 
            ListBoxKeyPoints.ItemsSource = keyPointNames;
        }


        private bool SetKeyPoints(int tourId)
        {
           

            string[] keyPointsArray = keyPointNames.ToArray();
            if (keyPointsArray.Length < 2 || string.IsNullOrEmpty(keyPointsArray[1]))
            {
                MessageBox.Show("Please enter at least two key points separated by commas.");
                return false;
            }

            for (int i = 0; i < keyPointsArray.Length; i++)
            {
                string keyPointName = keyPointsArray[i];
                KeyPointType keyType = i == 0 ? KeyPointType.Begining : (i == keyPointsArray.Length - 1 ? KeyPointType.End : KeyPointType.Middle);
                int ordinalNumber = i + 1;
                bool isChecked = false;

                KeyPoint keyPoint = new KeyPoint(tourId, keyPointName, keyType, ordinalNumber, isChecked);
                keyPointRepository.AddKeyPoint(keyPoint);


            }

            return true;
        }




        private void CreateTourButtonClick(object sender, RoutedEventArgs e)
        {
            foreach (var startDate in tourDates)
            {
                // Dobijanje selektovane lokacije iz ComboBox-a
                string selectedLocation = ComboBoxLocation.SelectedItem as string;

                // Dobijanje selektovanog jezika iz ComboBox-a
                string selectedLanguage = ComboBoxLanguage.SelectedItem as string;

                if (selectedLocation != null && selectedLanguage != null)
                {
                    string[] locationParts = selectedLocation.Split(',');
                    if (locationParts.Length == 2)
                    {
                        string city = locationParts[0].Trim();
                        string country = locationParts[1].Trim();

                        // Kreiranje LocationDto objekta sa izabranim gradom i državom 
                        LocationDto locationDto = new LocationDto { City = city, Country = country };

                        // Kreiranje nove ture sa novom lokacijom i jezikom
                        TourDto newTourDto = new TourDto(tourDto.Name, tourDto.Description, selectedLanguage, tourDto.MaxTouristNumber, startDate, tourDto.Duration, locationDto, tourDto.ImagesPaths);
                        tourRepository.Save(newTourDto.ToTour());
                        int id = tourRepository.NextId() - 1;

                        if (!SetKeyPoints(id))
                        {
                            MessageBox.Show("Failed to create tour.");
                            return;
                        }
                    }
                }
            }
        }




        private void NavigateToMainPage(object sender, MouseButtonEventArgs e)
        {
            GuideMainPage guideMainPage = new GuideMainPage();
            this.NavigationService.Navigate(guideMainPage);


        }

        private void NavigateToSideMenuPage(object sender, MouseButtonEventArgs e)
        {

        }

        private int currentImageIndex = 0;

        private void NextImageButtonClick(object sender, RoutedEventArgs e)
        {
            if (currentImageIndex < tourDto.ImagesPaths.Count - 1)
            {
                currentImageIndex++;
                UpdateImage();
            }
        }

        private void PreviousImageButtonClick(object sender, RoutedEventArgs e)
        {
            if (currentImageIndex > 0)
            {
                currentImageIndex--;
                UpdateImage();
            }
        }

        private void DeleteImageButtonClick(object sender, RoutedEventArgs e)
        {
            if (currentImageIndex >= 0 && currentImageIndex < tourDto.ImagesPaths.Count)
            {
                tourDto.ImagesPaths.RemoveAt(currentImageIndex);
                if (currentImageIndex >= tourDto.ImagesPaths.Count)
                {
                    currentImageIndex--;
                }
                UpdateImage();
            }
        }

        private void UpdateImage()
        {
            if (tourDto.ImagesPaths.Count > 0)
            {
                string selectedImagePath = tourDto.ImagesPaths[currentImageIndex];
                BitmapImage imageSource = new BitmapImage();
                imageSource.BeginInit();
                imageSource.CacheOption = BitmapCacheOption.OnLoad;
                imageSource.UriSource = new Uri(selectedImagePath);
                imageSource.EndInit();
                ImagePreview.Source = imageSource;
            }
            else
            {
                ImagePreview.Source = null;
            }
        }

        private void AddDateAndTimeButtonClick(object sender, RoutedEventArgs e)
        {
            DateTime selectedDateTime = StartDateTimePicker.Value ?? DateTime.Now;
            tourDates.Add(selectedDateTime);
            NewDateTextBox.Text = string.Empty;
            RefreshDatesList();
        }

        private void RefreshDatesList()
        {
            ListBoxDates.ItemsSource = null;
            ListBoxDates.ItemsSource = tourDates.Select(date => date.ToString());
        }

        private void RemoveDateButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                string dateString = button.DataContext as string;
                if (!string.IsNullOrEmpty(dateString))
                {
                    DateTime dateToRemove = DateTime.Parse(dateString);
                    tourDates.Remove(dateToRemove);
                    RefreshDatesList();
                }
            }
        }



    }
}
