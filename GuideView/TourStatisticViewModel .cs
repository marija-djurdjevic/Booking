using BookingApp.Model;
using BookingApp.Repository;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using BookingApp.View;
using System.Windows;
using BookingApp.Commands;



namespace BookingApp.GuideView
{
    public class TourStatisticViewModel : BaseViewModel
    {
        private readonly TourRepository tourRepository;
        private readonly LiveTourRepository liveTourRepository;
        private readonly TouristExperienceRepository touristExperienceRepository;
        private ObservableCollection<Tour> finishedTours;
        private ObservableCollection<Tour> sortedTours;
        private RelayCommand _touristsButtonClickCommand;


        public TourStatisticViewModel()
        {
            tourRepository = new TourRepository();
            liveTourRepository = new LiveTourRepository();
            touristExperienceRepository = new TouristExperienceRepository();
            _touristsButtonClickCommand = new RelayCommand(ExecuteTouristsButtonClick);

            LoadData();
           // SelectedYear = "2023";
        }

        private void LoadData()
        {
            var finishedLiveTours = liveTourRepository.GetAllLiveTours().Where(t => !t.IsLive).ToList();

            finishedTours = new ObservableCollection<Tour>();
            foreach (var tour in finishedLiveTours)
            {
                var finishedTour = tourRepository.GetTourById(tour.TourId);
                finishedTours.Add(finishedTour);
            }

            sortedTours = new ObservableCollection<Tour>(finishedTours.OrderByDescending(t => GetNumberOfTouristsForTour(t.Id)));
        }

        private int GetNumberOfTouristsForTour(int tourId)
        {
            return touristExperienceRepository.GetNumberOfTouristsForTour(tourId);
        }

        

        public ObservableCollection<Tour> SortedTours
        {
            get { return sortedTours; }
            set
            {
                sortedTours = value;
                OnPropertyChanged();
            }
        }


        private Tour selectedTour;

        public Tour SelectedTour
        {
            get { return selectedTour; }
            set
            {
                if (selectedTour != value)
                {
                    selectedTour = value;
                    OnPropertyChanged(nameof(SelectedTour));
                }
            }
        }



        private string selectedYear;

        public string SelectedYear
        {
            get { return selectedYear; }
            set
            {
                if (selectedYear != value)
                {
                    selectedYear = value;
                    OnPropertyChanged(nameof(SelectedYear));
                    UpdateTourList(); 
                }
            }
        }





        private void UpdateTourList()
        {
            if (SelectedYear == "General")
            {
                SortedTours = new ObservableCollection<Tour>(finishedTours);
            }
            else if (int.TryParse(SelectedYear, out int year))
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
                    SortedTours = new ObservableCollection<Tour>(tours);

                }


            }
        }


       

        public RelayCommand TouristsButtonClickCommand
        {

            get { return _touristsButtonClickCommand; }
            set
            {
                if (_touristsButtonClickCommand != value)
                {
                    _touristsButtonClickCommand = value;
                    OnPropertyChanged();
                }
            }

        }


        private void ExecuteTouristsButtonClick(object parameter)
        {
            if (parameter != null && int.TryParse(parameter.ToString(), out int tourId))
            {
                var touristsNumberPage1 = new TouristsNumberPage1(tourId);
                GuideMainWindow.MainFrame.Navigate(touristsNumberPage1);
            }
        }



    }

}
