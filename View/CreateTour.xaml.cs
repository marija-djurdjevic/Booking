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
        private TourDto tourDto;

        public String startDateTimeInput;

        private List<DateTime> tourDates = new List<DateTime>();

        TourRepository tourRepository;
        KeyPointRepository keyPointRepository;
        public CreateTour()
        {
            InitializeComponent();
            tourDto = new TourDto();
            DataContext = tourDto;
            tourRepository = new TourRepository();
            keyPointRepository = new KeyPointRepository();
            tourDates=new List<DateTime>();


        }

        private void AddImagePathButtonClick(object sender, RoutedEventArgs e)
        {
            
            string newImagePath = Microsoft.VisualBasic.Interaction.InputBox("Enter a new image path:", "Add Image Path", "");

            if (!string.IsNullOrEmpty(newImagePath))
            {
                
                tourDto.ImagesPaths.Add(newImagePath);
            }
        }


        private void AddDateAndTimeButtonClick(object sender, RoutedEventArgs e)
        {
            string newDateAndTime = Microsoft.VisualBasic.Interaction.InputBox("Enter a new date and time (format: M/d/yyyy h:mm:ss tt):", "Add Date and Time", "");

            if (!string.IsNullOrEmpty(newDateAndTime))
            {
                if (DateTime.TryParseExact(newDateAndTime, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateAndTime))
                {
                    tourDates.Add(dateAndTime);
                    DatesAndTimes.Items.Add(dateAndTime.ToString());
                }
                else
                {
                    MessageBox.Show("Invalid date and time format. Please enter a valid date and time (format: M/d/yyyy h:mm:ss tt).");
                }
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

                KeyPoint keyPoint = new KeyPoint(tourId, keyPointName, keyType, ordinalNumber, isChecked);
                keyPointRepository.AddKeyPoint(keyPoint);

              
            }

            return true;
        }

        private void CreateTourButtonClick(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }

            foreach (var startDate in tourDates)
            {
                TourDto newTourDto = new TourDto(tourDto.Name, tourDto.Description, tourDto.Language, tourDto.MaxTouristNumber, startDate, tourDto.Duration, tourDto.LocationDto, tourDto.ImagesPaths);
                tourRepository.Save(newTourDto.ToTour());
                int id = tourRepository.NextId() - 1;

                if (!SetKeyPoints(id))
                {
                    MessageBox.Show("Failed to create tour.");
                    return;
                }
            }

            MessageBox.Show("ActiveTours created successfully!");
            this.Close();
        }




        private bool ValidateFields()
        {
            if (string.IsNullOrEmpty(tourDto.Name))
            {
                MessageBox.Show("Please enter a name.");
                return false;
            }

            if (string.IsNullOrEmpty(tourDto.Description))
            {
                MessageBox.Show("Please enter a description.");
                return false;
            }

            if (string.IsNullOrEmpty(tourDto.Language))
            {
                MessageBox.Show("Please enter a language.");
                return false;
            }

            if (tourDto.MaxTouristNumber < 0)
            {
                MessageBox.Show("Please enter a valid maximum number of tourists.");
                return false;
            }

            if (tourDto.Duration <= 0)
            {
                MessageBox.Show("Please enter a valid duration.");
                return false;
            }

            if (string.IsNullOrEmpty(tourDto.LocationDto.Country))
            {
                MessageBox.Show("Please enter a country.");
                return false;
            }

            if (string.IsNullOrEmpty(tourDto.LocationDto.City))
            {
                MessageBox.Show("Please enter a city.");
                return false;
            }

            if (tourDto.ImagesPaths.Count == 0)
            {
                MessageBox.Show("Please add at least one image path.");
                return false;
            }

            return true;
        }



    }
}
