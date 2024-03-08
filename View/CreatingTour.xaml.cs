using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace BookingApp.View
{
    public partial class CreatingTour : Window
    {
        private TourDto _tourDto;
        TourRepository tourRepository;
        public CreatingTour()
        {
            InitializeComponent();
            _tourDto = new TourDto();
            DataContext = this;
            tourRepository = new TourRepository();
        }

        private void CreateTourButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                TourDto newTourDto = new TourDto(_tourDto.Name, _tourDto.Description, _tourDto.Language, _tourDto.MaxTouristNumber, _tourDto.TourStartDates, _tourDto.Duration, _tourDto.ImagesPaths, _tourDto.LocationDTO);
                
                
                tourRepository.AddTour(newTourDto.ToTour());

                MessageBox.Show("Tour created successfully!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}
