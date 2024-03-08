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
        public CreatingTour()
        {
            InitializeComponent();
            datePickerStart.Text = DateTime.Now.Date.ToString();
            _tourDto = new TourDto();
            DataContext = _tourDto;
            tourRepository = new TourRepository();
        }

        private void CreateTourButton_Click(object sender, RoutedEventArgs e)
        {
           
            /*
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
                TourDto newTourDto = new TourDto(_tourDto.Name, _tourDto.Description, _tourDto.Language, _tourDto.MaxTouristNumber, _tourDto.StartDate, _tourDto.Duration, _tourDto.ImagesPaths, _tourDto.LocationDto);

                tourRepository.AddTour(newTourDto.ToTour());

                MessageBox.Show("Tour created successfully!");
                this.Close();
            
            
        }

    }
}
