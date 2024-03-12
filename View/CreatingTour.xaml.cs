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

        private void SetKeyPoints(int tourId)
        {
          
            
            string keyPointsText = KeyPointsTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(keyPointsText))
            {
                string[] keyPointsArray = keyPointsText.Split(',');
                int totalKeyPoints = keyPointsArray.Length;

                for (int i = 0; i < totalKeyPoints; i++)
                {
                    string keyPointName = keyPointsArray[i].Trim();
                    KeyPoint keyType = i == 0 ? KeyPoint.Begining : (i == totalKeyPoints - 1 ? KeyPoint.End : KeyPoint.Middle);
                    int ordinalNumber = i + 1;
                    bool isChecked = false;

                    keyPointsRepository.AddKeyPoint(tourId, keyPointName, keyType, ordinalNumber, isChecked);
                }
            }

          
        }

        

        private void CreateTourButton_Click(object sender, RoutedEventArgs e)
        {
           
                TourDto newTourDto = new TourDto(_tourDto.Name, _tourDto.Description, _tourDto.Language, _tourDto.MaxTouristNumber, _tourDto.StartDate, _tourDto.Duration, _tourDto.LocationDto,_tourDto.ImagesPaths);
                tourRepository.AddTour(newTourDto.ToTour());
                int id = tourRepository.NextId()-1;
                SetKeyPoints(id);
                MessageBox.Show("Tour created successfully!");
                this.Close();
            
            
        }

      

    }
}
