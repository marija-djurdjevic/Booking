using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.View
{
    public partial class CreateTour : Window
    {
        private TourDto _tourDto;

        public String startDateTimeInput;
        
        TourRepository tourRepository;
        KeyPointsRepository keyPointsRepository;
        public CreateTour()
        {
            InitializeComponent();
            _tourDto = new TourDto();
            DataContext = _tourDto;
            tourRepository = new TourRepository();
            keyPointsRepository = new KeyPointsRepository();
            startDateTextBox.TextChanged += (sender, e) =>
            {
                startDateTimeInput = startDateTextBox.Text;
            };

        }

        private void AddImagePathButtonClick(object sender, RoutedEventArgs e)
        {
            
            string newImagePath = Microsoft.VisualBasic.Interaction.InputBox("Enter a new image path:", "Add Image Path", "");

            if (!string.IsNullOrEmpty(newImagePath))
            {
                
                _tourDto.ImagesPaths.Add(newImagePath);
            }
        }

        private bool SetKeyPoints(int tourId)
        {
            string keyPointsText = KeyPointsTextBox.Text.Trim();
            if (string.IsNullOrEmpty(keyPointsText))
            {
                MessageBox.Show("Please enter at least two key points separated by commas.");
                return false;
            }

            string[] keyPointsArray = keyPointsText.Split(',');
            if (keyPointsArray.Length < 2 || string.IsNullOrEmpty(keyPointsArray[1]))
            {
                MessageBox.Show("Please enter at least two key points separated by commas.");
                return false;
            }

            for (int i = 0; i < keyPointsArray.Length; i++)
            {
                string keyPointName = keyPointsArray[i].Trim();
                KeyPointType keyType = i == 0 ? KeyPointType.Begining : (i == keyPointsArray.Length - 1 ? KeyPointType.End : KeyPointType.Middle);
                int ordinalNumber = i + 1;
                bool isChecked = false;

                keyPointsRepository.AddKeyPoint(tourId, keyPointName, keyType, ordinalNumber, isChecked);
            }

            return true;
        }

        private void CreateTourButtonClick(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }
            
            TourDto newTourDto = new TourDto(_tourDto.Name, _tourDto.Description, _tourDto.Language, _tourDto.MaxTouristNumber, _tourDto.StartDateTime, _tourDto.Duration, _tourDto.LocationDto, _tourDto.ImagesPaths);
            DateTime i;
            DateTime.TryParseExact(startDateTimeInput, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out i);
            newTourDto.StartDateTime = i;
            tourRepository.Save(newTourDto.ToTour());
            int id = tourRepository.NextId() - 1;

            if (SetKeyPoints(id))
            {
                MessageBox.Show("Tour created successfully!");
                this.Close();
            }
        }



        private bool ValidateFields()
        {
            if (string.IsNullOrEmpty(_tourDto.Name))
            {
                MessageBox.Show("Please enter a name.");
                return false;
            }

            if (string.IsNullOrEmpty(_tourDto.Description))
            {
                MessageBox.Show("Please enter a description.");
                return false;
            }

            if (string.IsNullOrEmpty(_tourDto.Language))
            {
                MessageBox.Show("Please enter a language.");
                return false;
            }

            if (_tourDto.MaxTouristNumber < 0)
            {
                MessageBox.Show("Please enter a valid maximum number of tourists.");
                return false;
            }

            if (_tourDto.Duration <= 0)
            {
                MessageBox.Show("Please enter a valid duration.");
                return false;
            }

            if (string.IsNullOrEmpty(_tourDto.LocationDto.Country))
            {
                MessageBox.Show("Please enter a country.");
                return false;
            }

            if (string.IsNullOrEmpty(_tourDto.LocationDto.City))
            {
                MessageBox.Show("Please enter a city.");
                return false;
            }

            if (_tourDto.ImagesPaths.Count == 0)
            {
                MessageBox.Show("Please add at least one image path.");
                return false;
            }

            return true;
        }



    }
}
