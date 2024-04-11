using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using BookingApp.GuideView;

namespace BookingApp.View
{

    public partial class TourStatistic : Page
    {
        private TourDto tourDto;
        private readonly TourRepository tourRepository;
        private Tour selectedTour;
        private readonly KeyPointRepository keyPointRepository;
        private readonly LiveTourRepository liveTourRepository;
        private readonly TouristRepository touristRepository;
        private readonly TouristExperienceRepository touristExperienceRepository;
        private ComboBox comboBox;
        private List<Tour> finishedTours;
        private List<Tour> sortedTours;
        private string selectedYear;
        public TourStatistic()
        {
            InitializeComponent();
            tourRepository = new TourRepository();
            keyPointRepository = new KeyPointRepository();
            liveTourRepository = new LiveTourRepository();
            touristRepository = new TouristRepository();
            touristExperienceRepository = new TouristExperienceRepository();
            selectedTour = new Tour();
            tourDto = new TourDto();
            DataContext = tourDto;
            finishedTours=new List<Tour>();

            

           
            var finishedLiveTours = liveTourRepository.GetAllLiveTours().Where(t => t.IsLive == false).ToList();

           
            foreach (var tour in finishedLiveTours)
            {
               
                var finishedTour = tourRepository.GetTourById(tour.TourId);

                
                finishedTours.Add(finishedTour);
            }

            var touristCounts = new Dictionary<int, int>();

            if (touristExperienceRepository != null)
            {
                foreach (var tour in finishedTours)
                {
                    int numberOfTourists = touristExperienceRepository.GetNumberOfTouristsForTour(tour.Id);
                    touristCounts.Add(tour.Id, numberOfTourists);
                }
            }
             sortedTours = finishedTours.OrderByDescending(t => touristCounts[t.Id]).ToList();


            tourListBox.ItemsSource = sortedTours;
          

        }

        
       
        private void NavigateToSideMenuPage(object sender, MouseButtonEventArgs e)
        {

        }

        private void TouristsButtonClick(object sender, RoutedEventArgs e)
        {
            
            DependencyObject parent = VisualTreeHelper.GetParent((Button)sender);
            while (!(parent is ListBoxItem))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            
            if (parent is ListBoxItem listBoxItem)
            {
                ListBox listBox = ItemsControl.ItemsControlFromItemContainer(listBoxItem) as ListBox;
                if (listBox != null)
                {
                    var selectedTour = listBox.ItemContainerGenerator.ItemFromContainer(listBoxItem) as Tour;
                    TouristsNumberPage1 touristsNumberPage1 = new TouristsNumberPage1(selectedTour.Id);
                    NavigationService.Navigate(touristsNumberPage1);
                }
            }
        }

        private void firstTourComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.SelectedItem != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
                string selectedValue = selectedItem.Content.ToString();
              
                if (selectedValue != selectedYear)
                {
                    selectedYear = selectedValue;
                    UpdateTourList();
                    comboBox.SelectedItem = selectedItem;
                }
            }
        }





        private void UpdateTourList()
        {

           

            if (selectedYear == "General")
            {
                tourListBox.ItemsSource = sortedTours;
               // tourListBox.SelectedItem = sortedTours.FirstOrDefault();
            }
            else if (int.TryParse(selectedYear, out int year))
            {

                var toursForYear = finishedTours.Where(t => t.StartDateTime.Year == year).ToList();
                var touristCounts = new Dictionary<int, int>();

                foreach (var tour in toursForYear)
                {
                    int numberOfTourists = touristExperienceRepository.GetNumberOfTouristsForTour(tour.Id);
                    touristCounts.Add(tour.Id, numberOfTourists);
                }

                var sortedToursForYear = toursForYear.OrderByDescending(t => touristCounts[t.Id]).ToList();

                if (sortedToursForYear.Count > 0)
                {
                    var tours = new List<Tour>(sortedTours);
                    tours.Insert(0, sortedToursForYear[0]);
                    tourListBox.ItemsSource = tours;
                }


            }

        }



    }

}
