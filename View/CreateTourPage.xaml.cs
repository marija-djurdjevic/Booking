using BookingApp.DTO;
using BookingApp.Model.Enums;
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

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for CreateTourPage.xaml
    /// </summary>
    public partial class CreateTourPage : Page
    {

        private TourDto tourDto;

        public String startDateTimeInput;

        private List<DateTime> tourDates = new List<DateTime>();

        private List<String> keyPointNames = new List<String>();

        TourRepository tourRepository;
        KeyPointRepository keyPointRepository;
        public CreateTourPage()
        {
            InitializeComponent();
            tourDto = new TourDto();
            DataContext = tourDto;
            tourRepository = new TourRepository();
            keyPointRepository = new KeyPointRepository();
            tourDates = new List<DateTime>();
            keyPointNames = new List<String>();
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
                    NewDateTextBox.Text = dateAndTime.ToString();

                }
                else
                {
                    MessageBox.Show("Invalid date and time format. Please enter a valid date and time (format: M/d/yyyy h:mm:ss tt).");
                }
            }
        }





        private void AddKeyPointButtonClick(object sender, RoutedEventArgs e)
        {
            string keyPointName = KeyPointsTextBox.Text;
            keyPointNames.Add(keyPointName);
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
                TourDto newTourDto = new TourDto(tourDto.Name, tourDto.Description, tourDto.Language, tourDto.MaxTouristNumber, startDate, tourDto.Duration, tourDto.LocationDto, tourDto.ImagesPaths);
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
