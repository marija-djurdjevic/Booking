using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.View
{
    public partial class CreatingTour : Window
    {
        private TourDto _tourDto;
        
        TourRepository tourRepository;
        KeyPointsRepository keyPointsRepository;
        public CreatingTour()
        {
            InitializeComponent();
            datePickerStart.Text = DateTime.Now.Date.ToString();
            _tourDto = new TourDto();
            DataContext = _tourDto;
            tourRepository = new TourRepository();
            keyPointsRepository = new KeyPointsRepository();
           
        }

        private void AddImagePathButton_Click(object sender, RoutedEventArgs e)
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
            if (!string.IsNullOrEmpty(keyPointsText))
            {
                string[] keyPointsArray = keyPointsText.Split(',');
                int totalKeyPoints = keyPointsArray.Length;

                if (totalKeyPoints < 2 || keyPointsArray[1]=="")
                {
                    MessageBox.Show("Please enter at least two key points separated by commas.");
                    return false;
                }

                for (int i = 0; i < totalKeyPoints; i++)
                {
                    string keyPointName = keyPointsArray[i].Trim();
                    KeyPoint keyType = i == 0 ? KeyPoint.Begining : (i == totalKeyPoints - 1 ? KeyPoint.End : KeyPoint.Middle);
                    int ordinalNumber = i + 1;
                    bool isChecked = false;

                    keyPointsRepository.AddKeyPoint(tourId, keyPointName, keyType, ordinalNumber, isChecked);
                }

                return true;
            }
            else
            {
                MessageBox.Show("Please enter at least two key points separated by commas.");
                return false;
            }
        }

        private void CreateTourButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }

            TourDto newTourDto = new TourDto(_tourDto.Name, _tourDto.Description, _tourDto.Language, _tourDto.MaxTouristNumber, _tourDto.StartDate, _tourDto.Duration, _tourDto.LocationDto, _tourDto.ImagesPaths);
            tourRepository.AddTour(newTourDto.ToTour());
            int id = tourRepository.NextId() - 1;

            if (SetKeyPoints(id))
            {
                MessageBox.Show("Tour created successfully!");
                this.Close();
            }
        }



        private bool ValidateFields()
        {
            if (string.IsNullOrEmpty(_tourDto.Name) ||
                string.IsNullOrEmpty(_tourDto.Description) ||
                string.IsNullOrEmpty(_tourDto.Language) ||
                _tourDto.MaxTouristNumber <= 0 ||
                _tourDto.Duration <= 0 ||
                string.IsNullOrEmpty(_tourDto.LocationDto.Country) ||
                string.IsNullOrEmpty(_tourDto.LocationDto.City) ||
                _tourDto.ImagesPaths.Count == 0)
            {
                MessageBox.Show("Please fill in all required fields.");
                return false;
            }
            return true;
        }


    }
}
