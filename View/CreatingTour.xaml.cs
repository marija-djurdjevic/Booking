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
            {/*
                if (string.IsNullOrWhiteSpace(_tourDto.Name))
                {
                    MessageBox.Show("Name is required.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(_tourDto.Description))
                {
                    MessageBox.Show("Description is required.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(_tourDto.Language))
                {
                    MessageBox.Show("Language is required.");
                    return;
                }

                if (_tourDto.MaxTouristNumber <= 0)
                {
                    MessageBox.Show("Max Tourists Number must be greater than 0.");
                    return;
                }

                if (_tourDto.TourStartDates == null || _tourDto.TourStartDates.Count == 0)
                {
                    MessageBox.Show("At least one Tour Start Date must be provided.");
                    return;
                }

                if (_tourDto.Duration <= 0)
                {
                    MessageBox.Show("Duration must be greater than 0.");
                    return;
                }

                if (_tourDto.ImagesPaths == null || _tourDto.ImagesPaths.Count == 0)
                {
                    MessageBox.Show("At least one Image Path must be provided.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(_tourDto.LocationDTO.City))
                {
                    MessageBox.Show("City is required for Location.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(_tourDto.LocationDTO.Country))
                {
                    MessageBox.Show("Country is required for Location.");
                    return;
                }
                */
                TourDto newTourDto = new TourDto(_tourDto.Name, _tourDto.Description, _tourDto.Language, _tourDto.MaxTouristNumber, _tourDto.TourStartDates, _tourDto.Duration, _tourDto.ImagesPaths, _tourDto.LocationDto);

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
