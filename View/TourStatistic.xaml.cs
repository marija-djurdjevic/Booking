using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            // Pronađi ListBoxItem roditelja dugmeta
            DependencyObject parent = VisualTreeHelper.GetParent((Button)sender);
            while (!(parent is ListBoxItem))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            // Ako je roditelj ListBoxItem, pronalazi ListBox i dobija DataContext
            if (parent is ListBoxItem listBoxItem)
            {
                ListBox listBox = ItemsControl.ItemsControlFromItemContainer(listBoxItem) as ListBox;
                if (listBox != null)
                {
                    var selectedTour = listBox.ItemContainerGenerator.ItemFromContainer(listBoxItem) as Tour;
                    TouristsNumberPage touristsNumberPage = new TouristsNumberPage(selectedTour.Id);
                    NavigationService.Navigate(touristsNumberPage);
                }
            }
        }

        private void firstTourComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTourList();
        }

        private void UpdateTourList()
        {
            if (tourListBox.SelectedItem != null)
            {
                ComboBox comboBox = tourListBox.FindName("firstTourComboBox") as ComboBox;

                string selectedYear = (comboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

                if (selectedYear == "General")
                {
                    tourListBox.ItemsSource = sortedTours; 
                }
                else if (int.TryParse(selectedYear, out int year))
                {
                    // Filtriraj i sortiraj ture za izabranu godinu
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
                        sortedTours.Insert(0, sortedToursForYear[0]);
                    }

                    tourListBox.ItemsSource = sortedTours; 
                }
            }
        }





    }

}
