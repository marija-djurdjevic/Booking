using BookingApp.Repositories;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using BookingApp.View;
using System.Windows;
using BookingApp.View.GuideView;
using BookingApp.Command;
using BookingApp.Domain.Models;
using BookingApp.Aplication.UseCases;
using BookingApp.Aplication;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.WPF.ViewModels.GuidesViewModel
{
    public class TourStatisticViewModel : BaseViewModel
    {
        private string selectedYear;
        private Tour selectedTour;
        private readonly TourService tourService;
        private readonly LiveTourService liveTourService;
        private readonly TourReservationService tourReservationService;
        private readonly TouristExperienceService touristExperienceService;
        private ObservableCollection<Tour> finishedTours;
        private ObservableCollection<Tour> sortedTours;
        private RelayCommand touristsButtonClickCommand;
        private RelayCommand sideMenuCommand;
        public User LoggedInUser { get; set; }
        public TourStatisticViewModel(User loggedInUser)
        {
            tourService = new TourService(Injector.CreateInstance<ITourRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            liveTourService = new LiveTourService(Injector.CreateInstance<ILiveTourRepository>(), Injector.CreateInstance<IKeyPointRepository>());
            tourReservationService = new TourReservationService(Injector.CreateInstance<ITourReservationRepository>());
            touristExperienceService = new TouristExperienceService(Injector.CreateInstance<ITouristExperienceRepository>());
            touristsButtonClickCommand = new RelayCommand(ExecuteTouristsButtonClick);
            sideMenuCommand = new RelayCommand(ExecuteSideMenuClick);
            LoggedInUser = loggedInUser;
            LoadData();
            
        }
        private void LoadData()
        {
            var finishedLiveTours = liveTourService.GetFinishedTours();

            finishedTours = new ObservableCollection<Tour>();
            foreach (var tour in finishedLiveTours)
            {
                var finishedTour = tourService.GetTourById(tour.TourId);

                if (finishedTour.GuideId == LoggedInUser.Id)
                {
                    finishedTours.Add(finishedTour);
                }
            }
            sortedTours = new ObservableCollection<Tour>(finishedTours.OrderByDescending(t => tourReservationService.GetTouristsForTour(t.Id)));
        }
        private int GetNumberOfTouristsForTour(int tourId)
        {
            return touristExperienceService.GetNumberOfTouristsForTour(tourId);
        }
        public ObservableCollection<Tour> SortedTours
        {
            get { return sortedTours; }
            set { sortedTours = value; OnPropertyChanged(); }
        }
        public Tour SelectedTour
        {
            get { return selectedTour; }
            set
            {
                if (selectedTour != value)
                {
                    selectedTour = value;
                    OnPropertyChanged();
                }
            }
        }
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
                var generalTours = finishedTours.OrderByDescending(t => tourReservationService.GetTouristsForTour(t.Id));
                SortedTours = new ObservableCollection<Tour>(generalTours);
            }
            else if (int.TryParse(SelectedYear, out int year))
            {
                var toursForYear = finishedTours.Where(t => t.StartDateTime.Year == year).ToList();
                var touristCounts = new Dictionary<int, int>();
                foreach (var tour in toursForYear)
                {
                    int numberOfTourists = tourReservationService.GetTouristsForTour(tour.Id);
                    touristCounts.Add(tour.Id, numberOfTourists);
                }
                var sortedToursForYear = toursForYear.OrderByDescending(t => touristCounts[t.Id]).ToList();
                if (sortedToursForYear.Count > 0 && sortedToursForYear[0] != sortedTours[0])
                {
                    var tours = new List<Tour>(sortedTours);
                    tours.Insert(0, sortedToursForYear[0]);
                    SortedTours = new ObservableCollection<Tour>(tours);
                }
            }
        }
        public RelayCommand TouristsButtonClickCommand
        {
            get { return touristsButtonClickCommand; }
            set
            {
                if (touristsButtonClickCommand != value)
                {
                    touristsButtonClickCommand = value;
                    OnPropertyChanged();
                }
            }
        }
       
        private void ExecuteTouristsButtonClick(object parameter)
        {
            if (parameter != null && int.TryParse(parameter.ToString(), out int tourId))
            {
                var touristsNumberPage1 = new TouristsNumber(tourId,LoggedInUser);
                GuideMainWindow.MainFrame.Navigate(touristsNumberPage1);
            }
        }
       

        public RelayCommand SideManuCommand
        {
            get { return sideMenuCommand; }
            set
            {
                if (sideMenuCommand != value)
                {
                    sideMenuCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private void ExecuteSideMenuClick()
        {

            var sideMenuPage = new SideMenuPage(LoggedInUser);
            GuideMainWindow.MainFrame.Navigate(sideMenuPage);

        }


    }
}
