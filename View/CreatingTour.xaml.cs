using BookingApp.DTO;
using BookingApp.Model;
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



        private void AddMiddleKeyPoint_Click(object sender, RoutedEventArgs e)
        {
            string newMiddleKeyPoint = Microsoft.VisualBasic.Interaction.InputBox("Enter a new middle key point:", "Add Middle Key Point", "");

            if (!string.IsNullOrEmpty(newMiddleKeyPoint))
            {
                _tourDto.MiddleKeyPoints.Add(newMiddleKeyPoint);
            }
        }


        private void CreateTourButton_Click(object sender, RoutedEventArgs e)
        {
           
                TourDto newTourDto = new TourDto(_tourDto.Name, _tourDto.Description, _tourDto.Language, _tourDto.MaxTouristNumber, _tourDto.StartDate, _tourDto.Duration, _tourDto.LocationDto/* _tourDto.KeyPointId*/,_tourDto.ImagesPaths);
                tourRepository.AddTour(newTourDto.ToTour());
                int id = tourRepository.NextId()-1;
                keyPointsRepository.AddKeyPoint(id, _tourDto.StartKeyPoint, _tourDto.MiddleKeyPoints, _tourDto.EndKeyPoint);

                MessageBox.Show("Tour created successfully!");
                this.Close();
            
            
        }

        
    }
}
